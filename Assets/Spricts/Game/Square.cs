using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
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
    public int HeightScale = 300;

    /// <summary>
    /// 地形の横スケール
    /// </summary>
    public int TerrainWidthScale = 35;

    /// <summary>
    /// 穴の横スケール
    /// </summary>
    public int HoleWidthScaleLow = 30;

    /// <summary>
    /// ランダム
    /// </summary>
    System.Random rand;

    public Square()
    {
        rand = new System.Random();
    }

    /// <summary>
    /// 矩形の台生成
    /// </summary>
    /// <param name="Refmax"></param>
    /// <param name="writter_y"></param>
    /// <returns></returns>
    public float[] SquareDraw(int Refmax, float writter_y)
    {
        // bezier配列どこまで要素が埋まっているか
        int Embedded = 0;

        // returnで返す矩形ライン
        float[] square = new float[Refmax];

        float beforeheight = writter_y;

        Vector2 squarewrittervec = new Vector2(0.0f, writter_y);

        bool isTerrain = true;
        // Refmax分の長さの台と谷を生成する。
        while (Embedded < square.Length)
        {
            float Height = 0.0f;
            int Width = 0;
            if (isTerrain == true)
            {
                Height = squarewrittervec.y + ((rand.Next(HeightScale) / 100) - (MaxHeight + Mathf.Abs(LowHeight)) / 2);
                Height = (Height > MaxHeight) ? Height * 0.7f : Height;
                Height = (Height < LowHeight) ? Height * 0.7f : Height;

                Width = rand.Next(TerrainWidthScale / 3) + 2 * TerrainWidthScale / 3;
            }
            else
            {
                // -10.0fは谷底
                Height = -10.0f;
                Width = rand.Next(4 * HoleWidthScaleLow / 6) + 2 * HoleWidthScaleLow / 6;
            }
            isTerrain ^= true;

            for (int i = 0; i < Width; i++)
            {
                if (Embedded < square.Length)
                {
                    square[Embedded] = Height;
                    Embedded++;
                }
                else
                {
                    break;
                }
            }
        }
        return square;
    }
    
   
}
