using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private Player _player;

    private string _sceneName;  //現在のシーン名を取得する変数

    void Start()
    {
        _player = FindObjectOfType<Player>();
        //現在のシーン名を取得
        _sceneName = PlayerPrefs.GetString("Retry", _sceneName);
    }

    void Update()
    {
        
    }

    /// <summary>
    /// 引数のボタンの名前に応じて処理
    /// </summary>
    /// <param name="buttom"></param>
    public void PushBottom(string buttom)
    {
        switch (buttom)
        {
            //リトライボタン
            case "Retry":
                SceneManager.LoadScene(_sceneName);
                break;
            //ギブアップボタン
            case "GiveUp":
                SceneManager.LoadScene("StageSelect");
                break;
        }
    }
}
