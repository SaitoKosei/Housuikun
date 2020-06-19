using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    private enum COIN
    {
        FIRST,
        SECOND,
        THIRD,
    }

    [SerializeField]
    private Image[] _uiCoin = default;        //UIのコインを管理
    [SerializeField]
    private Coin[] _coin = default;           //コインのオブジェクトを管理
    [SerializeField]
    private string _stageName = default;      //ステージ名を保存

    private bool[] _getCoin;                  //コインを獲ったかどうか管理
    private bool _saveGetCoin;                //コインの取得が保存されたかどうかを管理
    private int _getAllCoin;                  //"０" か "１" ですべてのコインを獲ったかどうか管理

    void Start()
    {
        _getAllCoin = 0;
        _getCoin = new bool[3];
        //ステージの名前ですべてのコインを獲得したかどうかを保存
        PlayerPrefs.SetInt(_stageName, _getAllCoin);
    }

    void Update()
    {
        //コイン獲得時の処理
        GetCoin();
        //コインをすべて獲得したかどうかの判断
        CompleteCoin();

        //すべて獲得した
        if (CompleteCoin())
        {
            if (!_saveGetCoin)
            {
                //ステージの名前ですべてのコインを獲得したことを "1" として保存
                PlayerPrefs.SetInt(_stageName, PlayerPrefs.GetInt(_stageName) + 1);
                //保存したのでtrue
                _saveGetCoin = true;
            }
        }
    }

    /// <summary>
    /// コイン獲得時の処理
    /// </summary>
    private void GetCoin()
    {
        for(int i = 0; i < _coin.Length; i++)
        {
            if(_coin[i]._getCoin == true)
            {
                //獲得したコインに該当するUIのコインのalfa値を最大にする
                var imageColor = _uiCoin[i].color;
                imageColor.a = 255f;
                _uiCoin[i].color = imageColor;
                //獲得したコインをtrue
                _getCoin[i] = true;
            }
        }
    }

    /// <summary>
    /// すべてのコインを獲得したかどうか判断
    /// </summary>
    /// <returns></returns>
    private bool CompleteCoin()
    {
        //すべて獲得していたらtrueを返す
        if (_getCoin[(int)COIN.FIRST] == true 
            && _getCoin[(int)COIN.SECOND] == true 
            && _getCoin[(int)COIN.THIRD] == true)
            return true;
        else return false;
    }
}
