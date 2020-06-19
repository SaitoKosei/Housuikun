using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverJudg : MonoBehaviour
{
    [SerializeField]
    private GameObject _splashEffect = default;             //死亡時に出すパーティクル用

    private CameraController _camera;
    private Player _player;

    private bool _cameraOut;                //プレイヤーが画面外にいるかどうか管理
    private float _interval;                //GameOverSceneに遷移するまでの間隔
    private float _intervalSecond;          //GameOverSceneに遷移する時間

    void Start()
    {
        _camera = FindObjectOfType<CameraController>();
        _player = FindObjectOfType<Player>();
        _cameraOut = false;
        _interval = 0f;
        _intervalSecond = 2.0f;
    }

    void Update()
    {
        //プレイヤーが画面外に出たかどうか確認
        CameraOut();

        //プレイヤーが画面外にいたら
        if (_cameraOut)
            //_intervalを加算
            _interval += Time.deltaTime;
        else
            //画面内にいる間は_intervalは0
            _interval = 0;

        //_intervalが_intervalSecondより上で
        if (_interval > _intervalSecond)
            //ゲームオーバーシーンにシーン遷移
            SceneManager.LoadScene("GameOver");
    }

    /// <summary>
    /// 画面外かどうか判定
    /// </summary>
    private void CameraOut()
    {
        //画面外に出ていて尚且つ、今までに画面外に出ていなかったら
        if (_camera._outDisplay && !_cameraOut)
        {
            //プレイヤーを非表示
            _player.enabled = false;

            //プレイヤーのポジションから死亡エフェクトを発生
            var pos = _player.transform.position;
            var rot = _player.transform.rotation;
            Instantiate(_splashEffect, pos, rot);

            //プレイヤーが画面外にいるのでtrue
            _cameraOut = true;
        }

    }
}
