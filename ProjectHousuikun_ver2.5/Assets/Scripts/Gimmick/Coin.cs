using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    private Player _player;
    private SE _se;

    public bool _getCoin;           //コインを獲得したかどうかを管理

    void Start()
    {
        _player = FindObjectOfType<Player>();
        _se = FindObjectOfType<SE>();
        _getCoin = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //コインと当たったらオブジェクトを非表示
        if (collision.transform.tag == "Player")
        {
            //獲得フラグをtrue
            _getCoin = true;
            //コインのSEを再生
            _se.PlaySE(SE.SE_TYPE.COIN);
            //獲ったコインは非表示
            gameObject.SetActive(false);
        }
    }
}
