using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMnager : MonoBehaviour
{
    public bool _audioPlay;     //BGMを一度再生したかどうかを管理

    private BGM _bgm;
    private SE _se;
    private GameManager _game;

    //今のゲームの状態
    public enum GAME_MODE
    {
        NULL,
        TITLE,          //1
        STAGESELECT,    //2
        STAGE1,         //3
        STAGE2,         //4
        STAGE3,         //5
        STAGE4,         //6
        STAGE5,         //7
        GAMECLEAR,      //8
        GAMEOVER,       //9
    }

    void Start()
    {
        _bgm = FindObjectOfType<BGM>();
        _se = FindObjectOfType<SE>();
        _game = FindObjectOfType<GameManager>();
        _audioPlay = false;

        //シーンに応じてBGMを再生
        SelectBGM(_game._sceneName);
    }

    private void Update()
    {
    }

    /// <summary>
    /// 今のシーンに応じて流すBGMを変更
    /// </summary>
    /// <param name="sceneName"></param>
    private void SelectBGM(string sceneName)
    {
        //今タイトルシーンだったら
        if (sceneName == "Title")
        {
            //タイトルBGMを流す
            _bgm.PlayBGM((int)GAME_MODE.TITLE);
            _audioPlay = true;
        }
        //今ステージセレクトシーンだったら
        if (sceneName == "StageSelect")
        {
            //ステージセレクトBGMを流す
            _bgm.PlayBGM((int)GAME_MODE.STAGESELECT);
            _audioPlay = true;
        }

    }
}
