using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float _nowWater;     //現在のプレイヤーの保持する水量を管理
    public string _sceneName;   //現在のシーン名を管理

    private Player _player;

    //private void Awake()
    //{
    //}

    void Start()
    {
        _player = FindObjectOfType<Player>();
        //プレイヤーが保持できる最大量の半分
        _nowWater = 50.0f;

        //現在のシーン名を取得
        _sceneName = SceneManager.GetActiveScene().name;
        //現在のシーン名を保存
        PlayerPrefs.SetString("Retry", _sceneName);
        PlayerPrefs.Save();
    }

    void Update()
    {
        //エスケープキーでアプリケーション終了
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        //Rキーで即リトライ
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
