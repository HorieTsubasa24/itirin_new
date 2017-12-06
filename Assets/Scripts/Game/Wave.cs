using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 凸凹描画クラス
/// </summary>
public class Wave:MonoBehaviour
{
    /// <summary>
    /// 上に行くモード(1)か下に行くモード(-1)か
    /// </summary>
    public int UpOrDown;

    /// <summary>
    /// 加速度の大きさ
    /// </summary>
    public float AcsStandardVal = 0.00001f;

    public float Acs;
    public float Vel;
    public float Vec;

    public float MaxHeight = 2.40f;

    System.Random rand;

    public Wave()
    {
        rand = new System.Random();

        Acs = 0;
        Vel = 0;
        Vec = 0;

        UpOrDown = 1;
    }

    /// <summary>
    /// Wave描画
    /// </summary>
    /// <returns></returns>
    public float [] WaveDraw(int Refmax, float writter_y)
    {
        Vec = writter_y;

        Acs = 0;

        float[] curve = new float[Refmax];

        for (int i = 0; i < Refmax; i++)
        {
            AcsChange();
            VelChange();
            VecChange();

            var y = Input.GetAxis("Vertical");
            if (y > .1f) Vec = -1.0f; 

            curve[i] = Vec;
        }
        
        return curve;
    }

    public void AcsChange()
    {
        if (rand.Next() % 2 == 0)
        {
            // 1/2の確率で加速度上下反転
            UpOrDown = -UpOrDown;
        }

        if (Vec > MaxHeight)
        {
            // 一定高さ以上で加速度を減らす
            Acs *= 0.2f;
            Vel *= 0.9f;
            UpOrDown = -1;
        }
        if (Vec < -MaxHeight)
        {
            // マイナスの一定高さ以下で加速度を減らすを減らす
            Acs *= 0.2f;
            Vel *= 0.9f;
            UpOrDown = 1;
        }
        if (Vel > 0.04f)
        {
            // 一定高さ以上で加速度を減らす
            Acs *= 0.2f;
            Vel *= 0.9f;
            UpOrDown = -1;
        }
        if (Vel < -0.04f)
        {
            // マイナスの一定高さ以下で加速度を減らす
            Acs *= 0.2f;
            Vel *= 0.9f;
            UpOrDown = 1;
        }
        Acs = Acs + AcsStandardVal * UpOrDown;
    }

    public void VelChange()
    {
        Vel += Acs;
    }

    public void VecChange()
    {
        Vec += Vel;
    }

}
