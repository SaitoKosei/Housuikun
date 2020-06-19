using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public int _stageNum;       //ステージの番号を入れる変数

    [SerializeField]
    private GameObject _displayNum = default;   //ステージの番号を表示させるオブジェクト


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
            //ステージの番号を表示
            _displayNum.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
            //ステージの番号を表示
            _displayNum.SetActive(false);

    }
}
