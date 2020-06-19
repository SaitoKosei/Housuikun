using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : MonoBehaviour
{
    [SerializeField]
    private Vector3[] _movePos = default;     //経路の座標を管理
    [SerializeField]
    private float _moveSpeed = default;           //リフトの移動速度

    private Rigidbody2D _rb;

    private float _amountMovement;  //移動量を管理
    private float _near;            //近いと判定できる数値
    private int _num;               //_movePosの添え字管理

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _num = 0;
        _near = 0.05f;
    }

    void Update()
    {
        //目的地との距離を算出
        var vec = _movePos[_num] - transform.position;
        //距離が_nearより大きかったら
        if (vec.magnitude > _near)
            //リフトを移動させる
            Move(_movePos[_num] - transform.position);
        else
        {
            //距離が_nearより小さくなったら
            //子オブジェクトにプレイヤーがあればvelocityを0に
            foreach (Transform childTransform in transform)
            {
                if(childTransform.tag == "Player")
                    childTransform.GetComponent<Rigidbody2D>().velocity = _rb.velocity * 0.0f;
            }
            //目標のポジションを超えないようにする
            _rb.MovePosition(_movePos[_num]);
            //添え字を更新
            _num = (_num + 1) % _movePos.Length;
        }
    }

    /// <summary>
    /// リフトの移動
    /// </summary>
    /// <param name="wayPos"></param>
    private void Move(Vector3 moveVec)
    {
        //次のポジションを算出
        var nextFramePos = transform.position + moveVec.normalized * _moveSpeed * Time.deltaTime;
        //移動量から今まで移動してきた分を引く
        _amountMovement -=  _moveSpeed * Time.deltaTime;
        //次のポジションまで移動させる
        _rb.MovePosition(nextFramePos);
    }
}
