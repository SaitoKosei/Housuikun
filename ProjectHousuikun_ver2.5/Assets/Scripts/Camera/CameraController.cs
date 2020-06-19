using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool _outDisplay;           //画面外に出たかどうか管理

    private float _outPosY;     //画面外判定するY座標

    private Player _player; 
    
    //カメラの上下移動範囲
    [SerializeField]
    private float _minYPos;     //最小値
    [SerializeField]
    private float _maxYPos;     //最大値

    void Start()
    {
        _player = FindObjectOfType<Player>();
        _outPosY = _minYPos - 7.0f;
        _outDisplay = false;
    }

    void Update()
    {   
        //カメラ移動
        transform.position = new Vector3(_player.transform.position.x,
            //カメラの上下移動範囲制限
            Mathf.Clamp(_player.transform.position.y, _minYPos, _maxYPos), transform.position.z);

        if (_player.transform.position.y < _outPosY && !_outDisplay)
            //プレイヤーが画面外でtrue
            _outDisplay = true;
    }
}
