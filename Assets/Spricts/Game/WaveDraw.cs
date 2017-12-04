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
	public List<Gimic> gimics;
    public Transform Tr_Combine;

    public int NumOfLaps = 0;
    public int nowstage = 1;
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

    int refStageGimic = 0;
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
    public int DotVelosity = 150;

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
		gimics.Add(GameObject.Find("star").GetComponent<Gimic>());
		nowstage = 1;
		refStageTemplate = 0;

		Gc = 0;
	}

    /// <summary>
    /// 物理計算を伴うので、FixedUpdateを使用
    /// </summary>
    private void FixedUpdate()
    {
		for (int i = 0; i < gimics.Count; i++)
		{
			if (gimics[i] != null)
				gimics[i].Move();
			else
				gimics.Remove(gimics[i]);
		}// return;
        /*
        if (!t)
        {
            vec_Writter -= new Vector2(0, 0.1f);
            if (vec_Writter.y < 0.5f) t = true;
        }
        else
        {
            vec_Writter += new Vector2(0, 0.1f);
            if (vec_Writter.y > 3.5f) t = false;
        }
        */
        // 曲線取り出し
        if (Distance == 0 || Nowref >= Distance)
        {
            Nowref = 0;
            Distance = 1 + (int)(rand.Next(82, 100) * 0.01f * (maxDistance - 1));

            DotHeight = CurveDraw(Distance);
        }

        // 崖以外なら一つ前の座標に代入
        if (vec_Writter.y > -6.0f) vec_BeforeWritter = vec_Writter;
        vec_Writter.y = DotHeight[Nowref];

        // 陸地生成
        var ob = Instantiate(prefab_Dot, vec_Writter, Quaternion.identity, Tr_Combine);
        ob.GetComponent<Rigidbody2D>().AddForce(Vector3.left * DotVelosity, ForceMode2D.Force);

        //ob = Instantiate(prefab_Dot, vec_Writter, Quaternion.identity);
        //ob.GetComponent<Rigidbody2D>().AddForce(Vector3.left * DotVelosity, ForceMode2D.Force);

        // 曲線をUnicycleに渡す
        if (unicycle != null)
        {
            for (int i = unicycle.dotSpline.Length - 2; i >= 0; i--)
                unicycle.dotSpline[i + 1] = unicycle.dotSpline[i];

            unicycle.dotSpline[0] = vec_Writter.y;
        }

        // ゲームスピードが早くなったらたまに配列の添字を一つ飛ばす。
        Nowref = GetCurveRef(Nowref, Gc);
        if (gameMode == GameMode.Game) Gc++;
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
		print(n);
		return n;
    }
}
