using System.Collections;
using UnityEngine;

/// <summary>
/// 使い方として、gameObjectにRendererを貼りつけておく
/// </summary>
public class PixAccess : MonoBehaviour {
    Texture2D drawTexture;
    Color[] buffer;

	// Use this for initialization
	void Start () {
        // メインのテクスチャを取得
        Texture2D mainTexture = (Texture2D)GetComponent<Renderer>().material.mainTexture;
        // ピクセルの幅のバッファ獲得
        Color[] pixels = mainTexture.GetPixels();

        buffer = new Color[pixels.Length];
        // ピクセル情報をバッファにコピー
        pixels.CopyTo(buffer, 0);
        // 描画するテクスチャ 
        drawTexture = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);
        drawTexture.filterMode = FilterMode.Point;
        /*--------------------------------------------------------------------------------------- 
         |  Point	ピクセルを 1 つ 1 つブロックのように表示する
         |  Bilinear	平均化され、滑らかに表示する
         |  Trilinear	Bilinear とほぼ同じだが、ミップマップレベルにおいてブレンドして表示する
         *--------------------------------------------------------------------------------------*/
    }

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="p"></param>
    public void Draw(Vector2 p)
    {
        buffer.SetValue(Color.black, (int)p.x + 256 * (int)p.y);
    }

    /// <summary>
    /// 地面の描画
    /// </summary>
    //public void DrawGround(float x, float y)
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Draw(hit.textureCoord * 256); // ここの256をピクセルの横サイズに変更
            }

            drawTexture.SetPixels(buffer);
            drawTexture.Apply();
            GetComponent<Renderer>().material.mainTexture = drawTexture;
        }
    }
}
