using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectCamera : MonoBehaviour
{
    private Player _player;

    void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    void Update()
    {
        //画面の中心をプレイヤーより少し上に合わせて移動
        transform.position = new Vector3(_player.transform.position.x, 
            _player.transform.position.y + 2.0f, transform.position.z);
    }
}
