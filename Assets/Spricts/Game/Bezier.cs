using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ベジェ曲線描画クラス
/// </summary>
public class Bezier : MonoBehaviour
{
    readonly float wavespan = 0.14f;

    /// <summary>
    /// 最大の高さ
    /// </summary>
    public float MaxHeight = 3.00f;

    /// <summary>
    /// 最大の低さ
    /// </summary>
    public float LowHeight = -2.40f;

    /// <summary>
    /// 縦スケール
    /// </summary>
    public int BezierHeightScale = 3;

    /// <summary>
    /// 横スケール
    /// </summary>
    public int BezierWidthScale = 80;
    
    /// <summary>
    /// 横スケール最小
    /// </summary>
    public int BezierWidthScaleLow = 30;

    System.Random rand;

    public Bezier()
    {
        rand = new System.Random();
    }

    /// <summary>
    /// ランダム制御点設定
    /// </summary>
    /// <returns></returns>
    Vector2[] RandomPointSet(float beforeheight)
    {
        Vector2 initPos = new Vector2();
        float a = rand.Next() % BezierHeightScale;
        float b = rand.Next() % BezierHeightScale;
        if (beforeheight > MaxHeight) a *= 1.5f;
        else if (beforeheight < LowHeight) b *= 1.5f;
        initPos.y = (a / 10 - (b / 10));

        Vector2[] vec = new Vector2[4];
        vec[0] = new Vector2(initPos.x, initPos.y);

        var bottom_y = (float)rand.Next() % (int)MaxHeight * 2 * 100 / 100 - 2.4f;

        vec[1] = new Vector2(initPos.x + (float)(rand.Next() % 120 * BezierHeightScale) / 100, bottom_y);
        vec[2] = new Vector2(initPos.x + (float)(rand.Next() % 240 * BezierHeightScale) / 100, initPos.y);

        vec[3] = new Vector2(initPos.x + (float)(rand.Next() % 240 * BezierHeightScale) / 100, bottom_y);
        
        return vec;
    }

    /// <summary>
    /// ランダム制御点設定
    /// </summary>
    /// <returns></returns>
    public Vector2[] RandomPointSet(int Width, float HeightSwingWidth)
    {
        Vector2[] vec = new Vector2[4];

        var high = HeightSwingWidth * rand.Next(0, 100) * 0.01f;
        var low = HeightSwingWidth * rand.Next(0, 100) * 0.01f;

        vec[0] = new Vector2(0, 0);
        vec[1] = new Vector2(0, low);
        vec[2] = new Vector2(Width, high);
        vec[3] = new Vector2(Width, low);

        return vec;
    }

    /// <summary>
    /// このクラスから呼ばれるGetBezierLine
    /// </summary>
    /// <param name="refmax"></param>
    /// <param name="wtvec"></param>
    /// <param name="beforeheight"></param>
    /// <returns></returns>
    Vector2[] GetBezierLine(Vector2 wtvec, float beforeheight)
    {
        Vector2[] points = RandomPointSet(beforeheight);

        // 分割数
        int DivNum = (int)(((points[2].x > points[3].x) ? points[2].x : points[3].x - points[0].x));
        if (DivNum > BezierWidthScale) DivNum = BezierWidthScale;
        else if (DivNum < BezierWidthScaleLow) DivNum += BezierWidthScaleLow;

        return GetBezierLineCommon(DivNum, points, beforeheight);
    }

    /// <summary>
    /// 外部からDivNumを指定して描画
    /// </summary>
    /// <param name="Width"></param>
    /// <param name="points"></param>
    /// <param name="beforeheight"></param>
    /// <returns></returns>
    public Vector2[] GetBezierLine(int Width, float HeightSwingWidth, float beforeheight)
    {
        // 分割数
        int DivNum = Width;
        Vector2[] points = RandomPointSet(Width, HeightSwingWidth);

        Vector2[] curve = new Vector2[DivNum];
        curve = BezierCurveCreate(points, DivNum);

        float fixvec = beforeheight - curve[0].y;
        for (int i = 0; i < curve.Length; i++)
        {
            curve[i].y += fixvec;
        }

        return curve;
    }


    /// <summary>
    /// ベジェ描画共通部
    /// </summary>
    /// <param name="DivNum"></param>
    /// <param name="points"></param>
    /// <param name="beforeheight"></param>
    /// <returns></returns>
    public Vector2[] GetBezierLineCommon(int DivNum, Vector2[] points, float beforeheight)
    {
        Vector2[] curve = new Vector2[DivNum];
        curve = BezierCurveCreate(points, DivNum);

        float fixvec = beforeheight - curve[0].y;
        for (int i = 0; i < curve.Length; i++)
        {
            curve[i].y += fixvec;
        }

        return curve;
    }

    
    
    /// <summary>
    /// ベジェ描画(WaveWritter)
    /// </summary>
    /// <returns></returns>
    public float[] BezierDraw(int Refmax, float writter_y)
    {
        // bezier配列どこまで要素が埋まっているか
        int Embedded = 0;
        float[] curve = new float[Refmax];
        float beforeheight = writter_y;

        Vector2 bezierwrittervec = new Vector2(0.0f, writter_y);

        // Refmax分の長さのベジェ曲線を1つ以上生成する。
        while (Embedded < curve.Length)
        {
            Vector2[] beziercurve = GetBezierLine(bezierwrittervec, beforeheight);

            for (int i = 0; i < beziercurve.Length; i++)
            {
                if (beziercurve[i] != Vector2.zero && Embedded < curve.Length)
                {
                    curve[Embedded] = beziercurve[i].y;
                    beforeheight = curve[Embedded];
                    Embedded++;
                }
                else
                {
                    break;
                }
            }
        }

        // 前に書かれた曲線と合わせる。
        if (curve[0] != writter_y)
        {
            var fixvec = writter_y - curve[0];
            for (int i = 0; i < curve.Length; i++)
                curve[i] += fixvec;
        }

        System.GC.Collect();
        return curve;
    }

    /// <summary>
    /// ベジェ曲線作成
    /// </summary>
    /// <param name="pnt"></param>
    /// <param name="divnum"></param>
    /// <returns></returns>
    public Vector2[] BezierCurveCreate(Vector2 []pnt, int divnum)
    {
        // divnum = 0 ~ 1
        // divnumはx = 0.14fずつ進む
        Vector2[] line1 = new Vector2[divnum];
        Vector2[] line2 = new Vector2[divnum];
        Vector2[] trueline = new Vector2[divnum];

        // 1つめ
        line1 = BezierGenerate(divnum, pnt[0], pnt[1], pnt[2]);

        // 2つめ
        line2 = BezierGenerate(divnum, pnt[1], pnt[2], pnt[3]);

        // ベジェ
        trueline = BezierGenerate(line1, line2);

        return trueline;
    }

    /// <summary>
    /// ベジェ曲線の基礎の曲線作成
    /// </summary>
    /// <param name="divnum"></param>
    /// <param name="pnt"></param>
    /// <param name="dot"></param>
    /// <returns></returns>
    public Vector2[] BezierGenerate(int divnum, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        float div = 0;
        Vector2[] line = new Vector2[divnum];

        while (div < divnum)
        {
            float t = div / divnum;

            // 制御点を出す
            Vector2 d1 = (1.0f - t) * p0 + p1;
            Vector2 d2 = (1.0f - t) * p1 + p2;

            // 線形補間してtの位置の点にする
            float x = Mathf.Lerp(d1.x, d2.x, t);
            float y = Mathf.Lerp(d1.y, d2.y, t);

            line[(int)div] = new Vector2(x, y);

            div++;
        }

        return line;
    }
    
    /// <summary>
    /// ベジェ曲線生成
    /// </summary>
    /// <param name="l1"></param>
    /// <param name="l2"></param>
    /// <returns></returns>
    public Vector2[] BezierGenerate(Vector2[] l1, Vector2[] l2)
    {
        float div = 0;
        Vector2[] line = new Vector2[l1.Length];
        
        while (div < l1.Length)
        {
            var t = div / l1.Length;

            // 制御点を出す
            var d1 = l1[(int)div];
            var d2 = l2[(int)div];

            // 線形補間してtの位置の点にする
            var x = Mathf.Lerp(d1.x, d2.x, t);
            var y = Mathf.Lerp(d1.y, d2.y, t);

            line[(int)div] = new Vector2(x, y);

            div++;
        }

        return line;
    }
}
