using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameObject _waterObj;      //水のオブジェクト

    public int _selectStageNum;             //選択したステージの番号を保存

    private Rigidbody2D _player_rb;         //プレイヤー用
    private Rigidbody2D _moveBlock_rb;      //リフト用
    private CameraController _camera;
    private AudioSource _audio;
    private GameManager _game;
    private ObjectPool _pool;
    private Animator _animator = null;

    private bool _onMoveBlock;                    //リフトに乗ったかどうか判断

    //プレイヤーのパラメータ
    private bool _inWater;                              //水中かどうかを管理

    private float _direction;                           //向き
    private float _water;                               //保持する水の量
    private float _oldTime;                             //時間保存用

    private const float SCALE_MAX = 2.5f;               //大きさの最大値
    private const float SCALE_MIN = 0.7f;               //大きさの最小値
    private const float MASS_MAX = 1.5f;                //重さの最大値
    private const float MASS_MIN = 0.5f;                //重さの最小値
    private const float WATER_MAX = 100.0f;             //保持する水の最大量
    private const float WATER_MIN = 0.0f;               //保持する水の最小量
    private const float WALK_SPEED = 5.0f;              //歩くスピード
    private const float INCREASE_SPEED = 50.0f;         //増える水のスピード
    private const float DECREASE_SPEED = 20.0f;         //減る水のスピード
    private const float WATER_SPEED_LIMIT = 6.0f;       //移動量の最大値
    private const float DISCHARGE_INTERVAL = 0.01f;     //放水の間隔
    private const float DISCHARGE_POWER = 100.0f;       //放水でプレイヤーにかかる力

    void Start()
    {
        _game = FindObjectOfType<GameManager>();
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        _camera = GetComponent<CameraController>();
        _player_rb = GetComponent<Rigidbody2D>();

        _water = _game._nowWater;
        _inWater = false;
        _direction = 1.0f;

        //_pool = GetComponent<ObjectPool>();
        //_pool.CreatePool(_bullet, 20);

        SlimeAnimation(_animator, false);
    }

    void Update()
    {
        var rightX = Input.GetAxis("RHorizontal");
        var rightY = Input.GetAxis("RVertical");

        //放水処理
        if ((rightX != 0 || rightY != 0) && Time.time - _oldTime > DISCHARGE_INTERVAL)
        {
            var pos = transform.position;
            var angle = Mathf.Atan2(rightY, rightX) * Mathf.Rad2Deg - 90.0f;
            var rot = Quaternion.Euler(0.0f, 0.0f, angle);
            if (_water != 0)
            {
                //放水の水を生成
                var obj = Instantiate(_waterObj, pos, rot);
                var bullet = obj.GetComponent<Bullet>();
                bullet._rightX = rightX;
                bullet._rightY = rightY;

                //放水によるプレイヤーの挙動
                var rb = GetComponent<Rigidbody2D>();
                Vector3 force = new Vector3(-rightX, -rightY, 0) * DISCHARGE_POWER;
                rb.AddForce(force, ForceMode2D.Force);
                //移動量制限
                if (rb.velocity.magnitude >= WATER_SPEED_LIMIT)
                    rb.velocity = rb.velocity.normalized * WATER_SPEED_LIMIT;
            }
            _oldTime = Time.time;
        }

        if (rightX != 0 || rightY != 0)
        {
            //放水によって保持する水量を減少
            _water = WaterDecrease(_water, WATER_MIN, WATER_MAX);
        }

        //水中・陸上に応じてプレイヤーの挙動を変更
        InWaterMove(_inWater, _player_rb);
        //水中の場合プレイヤーの保持する水の量を増加
        _water = WaterIncrease(_inWater, _water, WATER_MIN, WATER_MAX);

        //保持する水量に応じて大きさを更新
        SlimeSetScale();
        //保持する水量に応じて重さを更新
        SlimeSetMass();

        if (IsGround())
        {
            //接地判定がtrueで歩く
            if(Input.GetAxis("LHorizontal") != 0)
                SlimeWalk(transform.position);
        }
        else
        {
            SlimeAnimation(_animator, false);
        }

        //プレイヤーの向き更新
        DirectionUpdate();

        //GameManagerの_nowWaterを上書き
        _game._nowWater = _water;
    }

    /// <summary>
    /// キャラの向き切り替え
    /// </summary>
    private void DirectionUpdate()
    {
        if (Input.GetAxis("LHorizontal") != 0)
            _direction = Mathf.Sign(Input.GetAxis("LHorizontal"));
    }

    /// <summary>
    /// 陸上・水中に応じキャラの動きを変更
    /// </summary>
    /// <param name="inwater"></param>
    /// <param name="rigidbody2"></param>
    private void InWaterMove(bool inwater, Rigidbody2D rigidbody2D)
    {
        //陸上・水中に応じて重力・空気抵抗を調整
        if (inwater)
        {
            rigidbody2D.gravityScale = 0.5f;
            rigidbody2D.drag = 10.0f;
        }
        else
        {
            rigidbody2D.gravityScale = 1;
            rigidbody2D.drag = 0;
        }
    }

    /// <summary>
    /// キャラのアニメーション
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="trigger"></param>
    private void SlimeAnimation(Animator animator, bool trigger)
    {
        animator.SetBool("walk", trigger);
    }

    /// <summary>
    /// キャラの歩き
    /// </summary>
    /// <param name="nowPos"></param>
    private void SlimeWalk(Vector3 nowPos)
    {
        var walk = Vector3.zero;
        //Lスティックで移動
        walk.x = Input.GetAxis("LHorizontal") * WALK_SPEED;
        nowPos += walk * Time.deltaTime;
        _player_rb.position = nowPos;
        SlimeAnimation(_animator, true);
    }

    /// <summary>
    /// 接地判定
    /// </summary>
    /// <returns></returns>
    public bool IsGround()
    {
        //下にRayを飛ばし当たったオブジェクトのLayerが"Stage"なら接地
        int layerMask = 1 << LayerMask.NameToLayer("Stage");
        Ray ray = new Ray(transform.position, Vector3.down);
        Vector3 boxsize = transform.localScale;
        //プレイヤー向きに対応
        boxsize.x = Mathf.Abs(boxsize.x);
        //段差に対応、小ジャンプでも接地と見られないよう長さは短めに設定
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            ray.origin, boxsize / 1.5f, 0, ray.direction, transform.localScale.y / 7.0f, layerMask);
        if (raycastHit.collider) return true;
        else return false;
    }

    /// <summary>
    /// キャラが保持する水を増やす
    /// </summary>
    /// <param name="inwater"></param>
    /// <param name="water"></param>
    /// <param name="watermax"></param>
    /// <returns></returns>
    private float WaterIncrease(bool inwater, float water, float watermin, float watermax)
    {
            //保持する水が水量の最大値未満で水中だったら水量を増加
            if(water < watermax && inwater)
            {
                water += Time.deltaTime * INCREASE_SPEED;
                //最小値と最大値の範囲で増加させる
                water = Mathf.Clamp(water, watermin, watermax);
            }
        //増加させた水量を返す
        return water;
    }

    /// <summary>
    /// キャラが保持する水を減らす
    /// </summary>
    /// <param name="water"></param>
    /// <param name="watermax"></param>
    /// <returns></returns>
    private float WaterDecrease(float water, float watermin  ,float watermax)
    {

        //保持する水が水量の最小値より大きかったら水量を減少
        if (water > watermin)
        {
            water -= Time.deltaTime * DECREASE_SPEED;
            //最小値と最大値の範囲で減少させる;
            water = Mathf.Clamp(water, watermin, watermax);
        }
        //減少させた水量を返す
        return water;
    }

    /// <summary>
    /// 保持する水に応じてキャラの大きさを変える
    /// </summary>
    /// <returns></returns>
    private void SlimeSetScale()
    {
        //今保持している水の割合を取得
        var waterRatio = GetWaterRatio();
        //保持する水の割合とプレイヤーの向きに沿ってプレイヤーの大きさを変更
        Vector3 slimeScale = 
            Mathf.Lerp(SCALE_MIN, SCALE_MAX, waterRatio) * new Vector3(_direction,1,1);
        transform.localScale = slimeScale;
    }

    /// <summary>
    /// 保持する水に応じてキャラの重さを変える
    /// </summary>
    private void SlimeSetMass()
    {
        //今保持している水の割合を取得
        var waterRatio = GetWaterRatio();
        //保持する水の割合に沿ってプレイヤーの重さを変更
        _player_rb.mass = Mathf.Lerp(MASS_MIN, MASS_MAX, waterRatio);
    }

    /// <summary>
    /// 保持する水の割合を出す
    /// </summary>
    /// <returns></returns>
    public float GetWaterRatio()
    {
        return _water / WATER_MAX;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //即死ブロックに当たったら
        if (collision.transform.tag == "damagemap")
        {
            //保持する水量を初期値に戻す
            _water = WATER_MAX / 2.0f;
            //ゲームオーバーシーンへ遷移
            SceneManager.LoadScene("GameOver");
        }
        //リフトに乗ったらリフトの子オブジェクトにする
        if (collision.transform.tag == "MoveFloor")
            transform.SetParent(collision.transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //水中ならtrue
        if (collision.transform.tag == "inwater")
            _inWater = true;
        //ゴールしたらクリアシーンへ遷移
        if (collision.transform.tag == "Goal")
            SceneManager.LoadScene("GameClear");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "StageSelect")
        {
            //選択中のパイプが持っている数字を取得
            Pipe collisionPipe = collision.GetComponent<Pipe>();
            //選択したパイプの数字を代入
            _selectStageNum = collisionPipe._stageNum;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //水中判定をfalseにする
        if (collision.transform.tag == "inwater")
            _inWater = false;
        //選択していたパイプの数字を0に
        if (collision.transform.tag == "StageSelect")
            _selectStageNum = 0;

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //リフトから降りるとリフトの子オブジェクトを解除
        if (collision.transform.tag == "MoveFloor")
            transform.SetParent(null);
    }
}
