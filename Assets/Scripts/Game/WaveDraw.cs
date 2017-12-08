using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 曲線描画ライター
/// </summary>
public class WaveDraw : MonoBehaviour
{
	public AudioContainer audioctl;
	public List<GameObject> prefab_Gimics;
    public List<Gimic> gimics;
    public List<GameObject> ob_gimics;
    public Transform Tr_Combine;

    public int NumOfLaps = 0;
    public int nowstage = 0;
    int refStageTemplate = 0;

    /// <summary>
    /// ステージテンプレート
    /// 1:Wave 2:Bezire 3:Square
    /// </summary>
    static int[, ] stage = { { 1, 2, 3, 2, 1, 3, 3, 2, 1, 2, 3, 1, 2, 3, 2, 1, 3, 3, 2, 1, 2, 3 },
                            { 3, 1, 2, 1, 3, 1, 3, 1, 3, 2, 3, 3 ,2 ,1 ,3 ,2 ,1 ,3 ,1 ,2 ,3 ,2 },
                            { 2, 2, 3, 2, 1, 3, 3, 2, 2, 2, 3, 2, 2, 3, 2, 1, 3, 3, 2, 1, 2, 3 },
                            { 1, 2, 1, 3, 3, 3, 3, 2, 1, 2, 1, 1, 2, 2, 3, 1, 2, 1, 1, 2 ,1, 3 },
                            { 1, 1, 1, 2, 1, 1, 2, 1, 2, 1, 2, 3, 1, 1, 2, 2, 1, 2, 1, 1, 2, 3 },
                            { 3, 3, 1, 3, 1, 2, 3, 2, 1, 1, 2, 2, 3, 2, 1, 2, 2, 2, 2, 3, 1, 1 },
                            { 3, 2, 1, 2, 2, 3, 1, 1, 3, 3, 1, 3, 3, 2, 3, 1, 2, 3, 1, 3, 2, 1 },
                            { 2, 2, 1, 3, 1, 2, 3, 1, 2, 2, 2, 1, 1, 1, 2, 3, 1, 3, 1, 3, 3, 1 },
                            };

	/// <summary>
	/// ギミックの読み込み間隔
	/// </summary>
	public readonly int GimicTime = 300;
	private int gimiccount = 0;
    private int refStageGimic = 0;
    /// <summary>
    /// ステージギミック
    /// 1:enemy 2:warp ball 3:jump smasher 4:bridge 5:the falling table
    /// </summary>
    static int[, ] stagegimic = { { 1, 1, 3, 5, 4, 4, 4, 1, 3, 3, 1, 2, 2, 5, 1, 4, 1, 2, 1, 2, 2, 1, 4, 3 },
                                    { 3, 4, 1, 5, 5, 1, 3, 3, 4, 5, 3, 4, 5, 2, 2, 3, 4, 3, 4, 4, 1, 5, 3, 5 },
                                    { 4, 3, 2, 3, 4, 4, 3, 1, 5, 2, 4, 5, 1, 1, 2, 1, 2, 2, 1, 2, 1, 5, 5, 4 },
                                    { 3, 1, 2, 5, 3, 1, 1, 4, 2, 3, 5, 1, 1, 5, 4, 1, 4, 3, 5, 2, 1, 4, 5, 3 },
                                    { 3, 4, 3, 3, 3, 4, 3, 2, 1, 4, 1, 3, 1, 5, 2, 4, 2, 2, 4, 4, 1, 3, 5, 2 },
                                    { 5, 1, 3, 3, 1, 2, 2, 5, 3, 3, 4, 1, 3, 5, 5, 2, 5, 1, 3, 1, 2, 4, 4, 4 },
                                };

    public enum GameMode
    {
        Title, Game, GameOver
    }

    /// <summary>
    /// ゲームモード 
    /// </summary>
    public GameMode gameMode = GameMode.Title;

    /// <summary>
    /// グローバルカウンタ(m)
    /// </summary>
    public long Gc = 0;

    /// <summary>
    /// ゲームスピードの変わり目
    /// </summary>
    public int GameSpeedSpan = 2000;

    /// <summary>
    /// 曲線を描画するライター、
    /// 上下に移動しながら線分を描画していく。
    /// </summary>
    public Vector2 vec_Writter;

    /// <summary>
    /// 曲線を描画するライター、
    /// 一つ前の高さ。
    /// </summary>
    public Vector2 vec_BeforeWritter;

    /// <summary>
    /// 一輪車
    /// </summary>
    public GameObject ob_unicycle;
    public Unicycle unicycle;

    /// <summary>
    /// 線分の点、これを連続で描画して線分を描く
    /// </summary>
    public GameObject prefab_Dot;

    /// <summary>
    /// 陸地の下地、これを連続で描画して陸地を描く
    /// </summary>
    public GameObject prefab_Back;

    /// <summary>
    /// 凸凹曲線
    /// </summary>
    public Wave wave;

    /// <summary>
    /// Spline曲線
    /// </summary>
    public Bezier bezier;

    /// <summary>
    /// 矩形
    /// </summary>
    public Square square;


    public enum L_Likes
    {
        wave, bezier, square, none
    }
    /// <summary>
    /// 取り出した曲線
    /// </summary>
    public L_Likes l_like;


    /// <summary>
    /// 乱数
    /// </summary>
    System.Random rand;

    /// <summary>
    /// 計算上のy座標
    /// </summary>
    float[] DotHeight = new float[2000];

	/// <summary>
	/// ギミックを読み込んだ時のステージ読み込みウェイト
	/// </summary>
	int stageWait;

    /// <summary>
    /// 乱数の最大 + 1
    /// </summary>
    public int maxDistance = 250;
    
    /// <summary>
    /// DotHeight[]の最大参照、乱数で取得
    /// </summary>
    public int Distance = 0;

    /// <summary>
    /// DotHeight[]の現在参照
    /// </summary>
    public int Nowref = 0;

    /// <summary>
    /// デフォルトの点の移動速度
    /// DotVelosity = 1で
    /// 
    /// x = -0.0004ずれる
    /// </summary>
    public int DefaultDotVelosity = 150;

    /// <summary>
    /// 点の移動速度
    /// </summary>
    public int DotVelosity = 350;

    /// <summary>
    /// カメラ、
    /// オブジェクトが画面外に出るとオブジェクトを削除する監視用。
    /// </summary>
    public Camera cam;

    /// <summary>
    /// 背景のスプライト、
    /// 朝と夜の2パターンある。
    /// </summary>
    public Sprite sp_BackGround;

    /// <summary>
    /// 朝夜切り替わりのポイント
    /// </summary>
    public Sprite sp_Change;
    

    public void Start()
    {
		InitOnWitter();
    }

	public void InitOnWitter()
	{
		rand = new System.Random();

		gimics.Clear();
		nowstage = 1;
		refStageTemplate = 0;
        vec_Writter.y = 0.0f;
		gimiccount = 0;
		refStageGimic = 0;
		stageWait = 200;
        for (int i = 0; i < DotHeight.Length; i++)
            DotHeight[i] = 0.0f;


        Gc = 0;
	}

    /// <summary>
    /// 物理計算を伴うので、FixedUpdateを使用
    /// </summary>
    private void FixedUpdate()
    {
        if (gameMode != GameMode.Game) return;

		GimicRoutine();
        // ギミック取り出し
        SetGimic();

        // 曲線取り出し
        if (Distance == 0 || Nowref >= Distance)
        {
            SplineGet();
        }


        // 崖以外なら一つ前の座標に代入
        if (vec_Writter.y > -6.0f) vec_BeforeWritter = vec_Writter;

		// print(stageWait);
		if (stageWait == 0)
		{
            SetTerrainNormal();
        }
		else
		{
            // ギミックの陸地を読み込む
            if (gimics.Count > 0 && gimics[gimics.Count - 1].Span > 0)
            {
                try
                {
                    vec_Writter.y = gimics[gimics.Count - 1].heightline[gimics[gimics.Count - 1].Span - stageWait];
                }
                catch
                {
                    print(gimics.Count - 1);
                    print(gimics[gimics.Count - 1].Span - stageWait);
                }
                if (stageWait == 1) SplineGet();
                PassCurveToUnicycle();
            }
            else if (gimics.Count == 0) {
                // 序盤の陸陸地生成
                var ob = Instantiate(prefab_Dot, vec_Writter, Quaternion.identity, Tr_Combine);
                ob.GetComponent<Rigidbody2D>().AddForce(Vector3.left * DotVelosity, ForceMode2D.Force);
                PassCurveToUnicycle();
            }
			stageWait--;
		}
	    Gc++;
    }

    /// <summary>
    /// 読み込んだ陸地セット
    /// </summary>
    void SetTerrainNormal()
    { 
        vec_Writter.y = DotHeight[Nowref];

        // 陸地生成
        var ob = Instantiate(prefab_Dot, vec_Writter, Quaternion.identity, Tr_Combine);
        ob.GetComponent<Rigidbody2D>().AddForce(Vector3.left * DotVelosity, ForceMode2D.Force);

        PassCurveToUnicycle();

        // ゲームスピードが早くなったらたまに配列の添字を一つ飛ばす。
        Nowref = GetCurveRef(Nowref, Gc);
    }


    void SplineGet()
    {
        Nowref = 0;
        Distance = 1 + (int)(rand.Next(82, 100) * 0.01f * (maxDistance - 1));

        DotHeight = CurveDraw(Distance);
    }


    void PassCurveToUnicycle()
    {
        // 曲線をUnicycleに渡す
        if (unicycle != null)
        {
            for (int i = unicycle.dotSpline.Length - 2; i >= 0; i--)
                unicycle.dotSpline[i + 1] = unicycle.dotSpline[i];

            unicycle.dotSpline[0] = vec_Writter.y;
        }
    }


    /// <summary>
    /// 曲線のドットを一つ飛ばすか飛ばさないか
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    int GetCurveRef(int nowref, long gc)
    {
        if (gameMode == GameMode.Title)
        {
            return nowref;
        }

        int a = (int)gc / GameSpeedSpan;
        int b = a % 2;
        switch (b)
        {
            case 0:
                return nowref + a + 1;
            case 1:
                if (gc % 2 == 0) return nowref + a;
                else return nowref + a + 1;
            default:
                return nowref + 1;
        }
    }

    /// <summary>
    /// 曲線描画
    /// </summary>
    float[] CurveDraw(int dis)
    {
        var n = GetStage();
        switch (n - 1)
        {
            case 0:
                l_like = L_Likes.wave;
                return wave.WaveDraw(dis + 1, vec_BeforeWritter.y);
            case 1:
                l_like = L_Likes.bezier;
                return bezier.BezierDraw(dis, vec_BeforeWritter.y);
            case 2:
                l_like = L_Likes.square;
                return square.SquareDraw(dis, vec_BeforeWritter.y);
            default:
                return null;
        }
    }

    /// <summary>
    /// ステージブロックをゲット更新
    /// </summary>
    /// <returns></returns>
    int GetStage()
    {
        var n = stage[nowstage, refStageTemplate];
        refStageTemplate = (refStageTemplate < stage.GetLength(nowstage) - 1) ? refStageTemplate + 1 : 0;
		if (refStageTemplate == 0)
		{
			nowstage = (nowstage >= stage.Length) ? 0 : nowstage++;
			audioctl.AudioChange();
		}
        print("Stage" + n);
		return n;
    }

	/// <summary>
	/// ステージギミックをゲット更新
	/// </summary>
	/// <returns></returns>
	void SetGimic()
	{
		if (gameMode != GameMode.Game)
			return;
		
		if (stageWait > 0)
		{
			return;
		}
		if (gimiccount > 0)
		{
			gimiccount--;
			return;
		}

		var n = stagegimic[nowstage, refStageGimic];
		refStageGimic = (refStageGimic < stagegimic.GetLength(nowstage) - 1) ? refStageGimic + 1 : 0;
		// print("Gimic" + n);
		var ob = Instantiate(prefab_Gimics[n]);
		var gim = ob.GetComponent<Gimic>();

        // 初期化
		gim.Init(vec_Writter.y);

        // リスト登録
        gimics.Add(gim);
        ob_gimics.Add(ob);
        gimiccount = GimicTime;
		stageWait = gim.Span;
		// print("Aaaa" + stageWait);
		return;
	}

    public void GimicDelete()
    {
        foreach (var a in ob_gimics)
            Destroy(a);

        ob_gimics.Clear();
        gimics.Clear();
    }

	/// <summary>
	/// ギミックのルーチン
	/// </summary>
	void GimicRoutine()
	{
		for (int i = 0; i < gimics.Count; i++)
		{
			if (gimics[i] != null)
				gimics[i].Move();
			else
				gimics.Remove(gimics[i]);
		}
	}
}
