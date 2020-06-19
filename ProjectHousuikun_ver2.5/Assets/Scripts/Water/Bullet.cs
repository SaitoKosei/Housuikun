using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float _bulletsSpeed;         //放水した水の進むスピード

    public float _rightX;               //右スティックのX方向の入力
    public float _rightY;               //右スティックのY方向の入力

    private Rigidbody2D _rigid;

    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();

        //コントローラーの右スティックの方向のベクトルを入れる
        //物理に従って落下していく
        _rigid.velocity = new Vector2(_rightX * _bulletsSpeed, _rightY * _bulletsSpeed);

        //コントローラーの傾きの大きさで水の大きさを変更
        //var size = _rigid.velocity.magnitude / _bulletsSpeed;
        //transform.localScale = new Vector3(size, size, 0.0f);
    }

    void Update()
    {
        //水のオブジェクトが進行方向に向けてオブジェクトの向きを補正
        var angle = Mathf.Atan2(_rigid.velocity.y, _rigid.velocity.x) * Mathf.Rad2Deg - 90.0f;
        transform.eulerAngles = new Vector3(0.0f, 0.0f, angle);
        //4秒経つと破壊
        Destroy(gameObject, 4.0f);
    }

}
