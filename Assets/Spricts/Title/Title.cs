using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour {
    public GameSceneManager GSM;
    public WaveDraw wavedraw;
    public Unicycle unicycle;
    
    /// <summary>
    /// スタートボタンクリック
    /// </summary>
    public void ClickOnStart()
    {
        GSM.ChangeToGameMode("Game", () => {
            wavedraw.gameMode = WaveDraw.GameMode.Game;
            wavedraw.InitOnWitter();
        });
    }
    
}
