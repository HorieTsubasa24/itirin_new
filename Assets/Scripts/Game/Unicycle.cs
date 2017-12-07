using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Unicycle : MonoBehaviour
{
    public GameSceneManager GSM;
    public WaveDraw WD;

    /// <summary>
    /// 初期位置
    /// </summary>
    public Vector3 InitPos;

    /// <summary>
    /// 物理計算をする
    /// </summary>
    public bool isSimulate = true;

    /// <summary>
    /// ボタン
    /// </summary>
    public GameObject[] Buttons;

    // ボタン入力
    public float inpx = 0.0f;
    public bool jump = false;

    public bool touchjump = false;
    public bool touchright = false;
    public bool touchleft = false;

    /// <summary>
    /// オブジェクトの上にいるか
    /// </summary>
    public bool isOntheObject = false;

    /// <summary>
    /// オブジェクトと接触、上下左右( 0, 1, 2, 3, 4)
    /// </summary>
    public int isCollidDirection = 0;

    /// <summary>
    /// 一つ前のオブジェクトの接触、上下左右( 0, 1, 2, 3, 4)
    /// </summary>
    public int isBeforeCollidDirection = 0;

    /// <summary>
    /// 一つ前のベクター
    /// ObjectAction.Action()の衝突判定で使用する
    /// </summary>
    public Vector2 BeforeVec;

    /// <summary>
    /// 曲線の高さ記憶(0 ~ 180)
    /// </summary>
    [System.NonSerialized]
    public float[] dotSpline = new float[181];

    /// <summary>
    /// 曲線のy = f(x)のx
    /// </summary>
    float DistFromDot;

    /// <summary>
    /// デフォルトの座標
    /// </summary>
    public Vector2 defaultvec = new Vector2(0.0f, 0.0f);

	/// <summary>
	/// 体力
	/// </summary>
	public int Hp = 100;

    /// <summary>
    /// 一輪車の座標
    /// </summary>
    public Vector2 vec = new Vector2(0.0f, 0.0f);        // 地上部分の座標、曲線を読み込み刻々と変わる
    public Vector2 vel = new Vector2(0.0f, 0.0f);        // 速度、落下時に計算
    public Vector2 acs = new Vector2(0.0f, -0.01f);   // 加速度(重力加速度)

    // float HeightPer2;                       // 車輪の1/2の幅

    bool isBeforeJump = false;                     // 前のフレームジャンプボタンを押していたか
    public bool isJumped = false;                  // ジャンプしたか空中に浮いているか
    public bool isJumped2nd = false;               // 2段目のジャンプをした
    public Vector2 jumpvec;                        // jumpstartからジャンプした座標(ジャンプスタート時の座標からの距離)
    public Vector2 jumpstart;                      // ジャンプを開始した座標
    public Vector2 vecdiff;                        /* vecdiff.y = jumpstart.y - jumpvec.y;    // jumpとの差分
                                                      ポジション = vec + jumpvec + vecdiff */

    // 計算した座標
    public Vector2 hvec;

    /// <summary>
    /// 画面上のドット数
    /// </summary>
    const int ViewDot = 136;

    /// <summary>
    /// 画面幅
    /// </summary>
    const float Viewrange = 19.04f;

    /// <summary>
    /// ドット間の距離
    /// </summary>
    const float DotSpan = Viewrange / ViewDot;


    private void OnEnable()
    {
        Init();
        Landing();
    }

    private void Init()
    {
        gameObject.transform.position = InitPos;

        isSimulate = true;

        touchjump = false;
        touchright = false;
        touchleft = false;

        isOntheObject = false;
        isCollidDirection = 0;

        isBeforeCollidDirection = 0;
        BeforeVec = new Vector2();

        for (int i = 0; i < dotSpline.Length; i++)
            dotSpline[i] = 0.0f;

        DistFromDot = 0.0f;
        defaultvec = new Vector2(0.0f, 0.0f);

		Hp = 100;

        vec = new Vector2(0.0f, 0.0f); 
        vel = new Vector2(0.0f, 0.0f);
        acs = new Vector2(0.0f, -0.01f);

        isBeforeJump = false; 
        isJumped = false;
        isJumped2nd = false;
        jumpvec = new Vector2();
        jumpstart = new Vector2();
        vecdiff = new Vector2();

        hvec = new Vector2();
}

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
	}

	/// <summary>
	/// キーボード入力
	/// </summary>
	public void GetInput()
	{
		if (touchright == true) {
			inpx += 0.05f;
		} else if (touchleft == true) {
			inpx -= 0.05f;
		} else {
			inpx = 0;
		}
		if (Mathf.Abs (inpx) >= 1.0f)
			inpx *= 0.95f;

		if (touchjump == true) {
			jump = true;
		} else {
			jump = false;
		}
		return;
	}

	/// <summary>
	/// タッチ入力
	/// </summary>
	public void JumpButtonDown()
	{
		touchjump = true;
		return;
	}

	public void JumpButtonUp()
	{
		touchjump = false;
		return;
	}
	/// <summary>
	/// →タッチ入力
	/// </summary>
	public void RightButtonDown()
	{
		touchright = true;
		touchleft = false;
		return;
	}

	public void RightButtonUp()
	{
		touchright = false;
		return;
	}
	/// <summary>
	/// ←タッチ入力
	/// </summary>
	public void LeftButtonDown()
	{
		touchleft = true;
		touchright = false;
		return;
	}

	public void LeftButtonUp()
	{
		touchleft = false;
		return;
	}

    /// <summary>
    /// 動作全般
    /// </summary>
    void Move()
    {
		GetInput();
        

        // ジャンプの座標計算
        Jump(jump);                             // ジャンプの動作、重力計算
        // OnObject(jump);                         // オブジェクト上にいるときのジャンプの動作、重力計算
        FallDown();                             // 自然落下の計算    

        gameObject.transform.position += (Vector3)vel;  // 速度計算

        // 陸地の凹凸の取得
        vec = gameObject.transform.position;    // 計算用の座標に現在値を代入
        vec.x += inpx * 0.05f;                  // 入力の値を計算値に代入

        // 配列の添字を出す
        DistFromDot = (Viewrange / 2 - vec.x) / DotSpan;

        // 線形補間(DistanceFromDot - DotRefは小数点以下)
        var y = dotSpline[(int)DistFromDot];// + d * (DistanceFromDot - DotRef);

        Vector2 befvec = vec;
        vec.y = y;/* + HeightPer2*/              // 基準の座標(地形)

        if (isJumped)
            vecdiff.y = jumpstart.y - vec.y;    // jumpとの差分計算、落下までのy座標が刻々と変わる

        BeforeVecSet();
        
        // 代入する座標
        hvec = vec + jumpvec + vecdiff;

        // 段差を見る
        // 死亡判定
        if (CheckDead(befvec)) return;


        //print("diff" + vecdiff);
        //print("hvec" + hvec);

        if (isCollidDirection != 1)
            gameObject.transform.position = hvec;// 計算用の値を実座標に代入
        else
            gameObject.transform.position = new Vector2(hvec.x, transform.position.y);// 計算用の値を実座標に代入

        return;
    }
    


    Vector2[] befvecs = new Vector2[4];
    bool[] isGraunding = new bool[4];
    bool IsGround
    {
        get
        {
            foreach (var a in isGraunding)
                if (a == false) return false;
            return true;
        }
    }
    /// <summary>
    /// 4つの座標連続で着地しているかチェック
    /// </summary>
    void IsGroundCheck(bool isGND)
    {
        for (int i = 0; i < isGraunding.Length - 1; i++)
        {
            isGraunding[i] = isGraunding[i + 1];
        }
        isGraunding[befvecs.Length - 1] = isGND;
    }

    /// <summary>
    /// 4つ前の座標をセット
    /// </summary>
    void BeforeVecSet()
    {
        for (int i = 0; i < befvecs.Length - 1; i++)
        {
            befvecs[i] = befvecs[i + 1];
        }
        befvecs[befvecs.Length - 1] = gameObject.transform.position;

        BeforeVec = gameObject.transform.position;
    }

    bool CheckDead(Vector2 befvec)
    {
        var cp = hvec.y - BeforeVec.y;
        if (cp < -0.4f)
        {
            IsGroundCheck(true);
            if (isJumped == false && IsGround == true)
            {
                ToFly();
                hvec = hvec - vec + befvec;
            }
            return true;
        }
        else if (cp > 0.6f)
        {
            var a = hvec - vec + befvec;
            if (a.y < dotSpline[(int)DistFromDot + 2])
            {
                print("gameover");
                WD.GimicDelete();
                GSM.ChangeToGameMode("Title", () =>
                {
                    WD.gameMode = WaveDraw.GameMode.Title;
                    transform.position = InitPos;
                    print("Title");
                });
            }
            return true;
        }
        else
        {
            IsGroundCheck(false);
            return false;
        }
    }

    /// <summary>
    /// ジャンプの動作、重力計算
    /// </summary>
    /// <param name="jump"></param>
    public void Jump(bool jump)
    {
        if (jump == false)
        {
            isBeforeJump = false;
            return;
        }

        if (isJumped == false)
        {
            isBeforeJump = true;
            ToFly();
            vel = -acs * 16;   // スクリプト上のMathf.Abs(Asc)*16の加速度を一瞬を与えてジャンプ
            return;
        }

        if (isBeforeJump == false && isJumped == true && isJumped2nd == false)
        {
            isBeforeJump = true;
            vel = -acs * 16;   // スクリプト上のMathf.Abs(Asc)*16の加速度を一瞬を与えてジャンプ
            isJumped2nd = true;
        }
    }

    /// <summary>
    /// 空中に浮く
    /// </summary>
    public void ToFly()
    {
        isJumped = true;
        isOntheObject = false;
        jumpstart = new Vector2(hvec.x, dotSpline[(int)DistFromDot]);
        jumpvec = new Vector2(0.0f, transform.position.y - dotSpline[(int)DistFromDot]);
        hvec = transform.position;
        vel = new Vector2();
    }

    /// <summary>
    /// オブジェクト上にいるときのジャンプの動作、重力計算
    /// </summary>
    /// <param name="jump"></param>
    public void OnObject(bool jump)
    {
        if (isOntheObject == false) return;

        if (isJumped == false && jump == true)
        {
            isBeforeJump = false;
            isJumped = true;
            isOntheObject = false;
            jumpstart = transform.position;
            vel += -acs * 16;   // スクリプト上のMathf.Abs(Asc)*16の加速度を一瞬を与えてジャンプ
            return;
        }
    }

    /// <summary>
    /// 落下計算
    /// </summary>
    public void FallDown()
    {
        // 落下
        jumpvec.y += vel.y;
        vel.y += acs.y;

        // 着地
        if ((jumpvec.y + vecdiff.y) <= 0.0f)
        {
            Landing();
        }
    }

    /// <summary>
    /// 着地処理
    /// </summary>
    public void Landing()
    {
        isBeforeJump = false;
        isJumped = false;
        isJumped2nd = false;
        jumpvec = new Vector2(0.0f, 0.0f);
        vecdiff = new Vector2(0.0f, 0.0f);
        jumpstart = new Vector2(0.0f, 0.0f);
        vel = new Vector2(0.0f, 0.0f);
    }
}
