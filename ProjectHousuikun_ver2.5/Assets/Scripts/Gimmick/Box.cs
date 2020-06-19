using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private Player _player;
    private Rigidbody2D _pRigidbody2D;  //プレイヤー用
    private Rigidbody2D _bRigidbody2D;  //箱ギミック用

    private float _moveSpeed;           //箱が動くスピード
    private bool _pushPlayer;           //プレイヤーが当たっているか管理

    [SerializeField]
    private float _pushMass;            //押すために必要なプレイヤーの重さ

    void Start()
    {
        _player = FindObjectOfType<Player>();
        _pRigidbody2D = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        _bRigidbody2D = GetComponent<Rigidbody2D>();

        _moveSpeed = 0.5f;
        _pushPlayer = false;
    }

    void Update()
    {
        BoxMove();
    }

    /// <summary>
    /// 箱を移動させる
    /// </summary>
    private void BoxMove()
    {
        //今の箱の座標
        var now = transform.position;
        if (_pushPlayer)
        {
            //プレイヤーの重さに応じて動かせるか判断
            if (BoxisGround() && _pRigidbody2D.mass > _pushMass)
            {
                var pos = Vector3.zero;
                //左から押された
                if (_player.transform.position.x < transform.position.x)
                    pos.x = _moveSpeed;
                //右から押された
                if (_player.transform.position.x > transform.position.x)
                    pos.x = -_moveSpeed;
                //今の座標に動いた分を加算
                now += pos * Time.deltaTime;
                //Rigidbodyのpositionを更新
                _bRigidbody2D.position = now;
            }
        }
    }

    /// <summary>
    /// 箱の接地判定
    /// </summary>
    /// <returns></returns>
    private bool BoxisGround()
    {
        //下にRayを飛ばしゲームオブジェクトに当たったら接地
        Ray ray = new Ray(transform.position, Vector3.down);
        Vector3 boxsize = transform.localScale;
        boxsize.x = Mathf.Abs(boxsize.x);
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            ray.origin, boxsize, 0, ray.direction, transform.localScale.y / 10.0f);
        if (raycastHit.collider) return true;
        else return false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
            //座標に応じてプレイヤーが箱に当たっているか判断
            if (_player.transform.position.y < transform.position.y)
                _pushPlayer = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //リフトに乗ったらリフトの子オブジェクトにする
        if (collision.transform.tag == "MoveFloor")
            transform.SetParent(collision.transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
            _pushPlayer = false;

        //リフトに乗ったらリフトの子オブジェクトを解除
        if (collision.transform.tag == "MoveFloor")
            transform.SetParent(null);
    }

}
