using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameSceneManager : MonoBehaviour {
    public GameObject currentSequence;

    public GameObject Writter;
    public GameObject Title;
    public GameObject Game;
    public GameObject GameOver;

    public GameObject Loadingimg; //画面切り替え時の暗幕
    public delegate void LoadCallback(); //暗幕が表示仕切った時に表示される処理

    /// <summary>
    /// ゲームモードの変更
    /// </summary>
    public void ChangeToGameMode(string str_gamemode, LoadCallback func)
    {
        switch (str_gamemode)
        {
            case "Title"    : LoadStart(func, Title); break;
            case "Game"     : LoadStart(func, Game); break;
            case "GameOver" : LoadStart(func, GameOver); ; break;
        }
    }

    /// <summary>
    /// シーンをロード
    /// </summary>
    /// <param name="func"></param>
    /// <param name="goSequence"></param>
    public void LoadStart(LoadCallback func, GameObject goSequence)
    {
        GameObject obj = Loadingimg;
        obj.SetActive(true);
        SpriteRenderer sp = obj.GetComponent<SpriteRenderer>();
        sp.color = new Color(0, 0, 0, 0);
        Sequence seq = DOTween.Sequence();

        seq.Append(sp.DOFade(1f, 0.5f));
        seq.AppendCallback(() => {
            currentSequence.SetActive(false);
            goSequence.SetActive(true);
            currentSequence = goSequence;
            if (func != null) func();
        });
        seq.Append(sp.DOFade(0f, 0.5f));
        seq.AppendCallback(() => { obj.SetActive(false); });
    }

    /// <summary>
    /// オブジェクトの非アクティブ化
    /// </summary>
    public void GotoObjectDisnable()
    {
        Writter.SetActive(false);
        Title.SetActive(false);
        Game.SetActive(false);
        GameOver.SetActive(false);
    }

}
