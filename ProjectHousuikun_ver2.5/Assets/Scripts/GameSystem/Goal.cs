using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private string _clearStageName = default;   //今のステージの名前を管理
    [SerializeField]
    private int _stageNum = default;        //今のステージの番号を管理

    private bool _goal;         //ゴールしたかどうかを管理

    void Start()
    {
        _goal = false;        
    }

    void Update()
    {
        //ゴールした時クリア情報を保存
        StageClear(_goal);    
    }

    /// <summary>
    /// ゴールしたらゴールしたステージの番号を保存
    /// </summary>
    /// <param name="clear"></param>
    private void StageClear(bool clear)
    {
        //プレイヤーがゴールしたら
        if (clear)
            //ステージの番号をステージ名で保存
            PlayerPrefs.SetInt(_clearStageName, _stageNum);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ゴールにプレイヤーが当たったら
        if (collision.transform.tag == "Player")
            _goal = true;
    }
}
