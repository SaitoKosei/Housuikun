using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
	enum COIN_NUM{
		NOT_ALL,
		ALL,
	}

    //保存データ呼び出し用
    [SerializeField]
    private string _stageName;          //コインをすべて獲得したかどうかを呼び出す
    [SerializeField]
    private string _stageClearCheck;     //ステージをクリアしたかどうかを呼び出す

	//画像
	[SerializeField]
	private List<Sprite> _flagSprites=new List<Sprite>();
	private Dictionary<string, Sprite> _flagSpritesDic=new Dictionary<string, Sprite>();

    [SerializeField]
    private SpriteRenderer _spriteRenderer = default;

    [SerializeField]
    private int  _stageNum = default;         //ステージの番号

    //保存した値を受け取る
    private int _getFlag;       //コインをすべて獲得したかどうかを管理
    private int _clearFlag;     //クリアしたステージの番号を管理

    void Start()
    {
		CheckTheFlag();
	}

    void Update()
    {

    }

	/// <summary>
	/// クリア状況を調べて旗を立てる
	/// </summary>
	void CheckTheFlag()
	{
		//Listの内容をDictionaryに移し替える
		foreach (var data in _flagSprites){
			_flagSpritesDic.Add(data.name, data);
		}

		//コインをすべて獲得したかどうかを管理
		_getFlag = PlayerPrefs.GetInt(_stageName);
        //クリアしたステージの番号を管理
        _clearFlag = PlayerPrefs.GetInt(_stageClearCheck);

		//ステージの番号とクリアしたステージの番号が同じだったら
		if (_stageNum == _clearFlag)
		{
			//ステージのコインをすべて獲得したら
			if (_getFlag == (int)COIN_NUM.NOT_ALL)
				//青の旗を立てる
				_spriteRenderer.sprite = _flagSpritesDic["BlueFlag"];
			else
				//金の旗を立てる
				_spriteRenderer.sprite = _flagSpritesDic["GoldFlag"];
		}
	}
}
