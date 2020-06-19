using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField]
    private Text _pushtext = default;                 //点滅するテキスト

    [SerializeField]
    private CanvasGroup _pushTextCanvas = default;    //点滅するテキスト用キャンバス
    [SerializeField]
    private CanvasGroup _commandCanvas = default;     //はじめから・つづきから用キャンバス

    private BGM _bgm;
    private SE _se;

    private float _flashingtime;
    private float _flashingSecond;
    private float _setAlpha;

    private bool _pushButton;          //ボタンが押されたか管理
    private bool _selectButton;        //はじめから・つづきからを選択したかどうか管理

    void Start()
    {
        _pushtext = GameObject.Find("PushText").GetComponent<Text>();
        _bgm = FindObjectOfType<BGM>();
        _se = FindObjectOfType<SE>();
        _flashingtime = 0.0f;
        _flashingSecond = 4.0f;
        _setAlpha = 0.5f;
    }

    void Update()
    {
        //テキスト点滅処理
        _flashingtime += Time.deltaTime * _flashingSecond;
        var coler = _pushtext.color;
        coler.a = Mathf.Sin(_flashingtime) * _setAlpha + _setAlpha;
        _pushtext.color = coler;

        //PushText画面でボタンまたはキーボードが押されたら
        if (Input.GetButtonDown("Decision")||Input.anyKey)
            //ボタンが押されたら
            _pushButton = true;

        //UIをはじめから・つづきからに変更
        TitleCommand(_pushButton);

        //はじめから・つづきからをどちらか選択したら
        if (_selectButton)
        {
            //フェードインをスタート
            Fade.Instance.StartFadeOut();
            //フェードイン中なら
            if (Fade.Instance._isFadeOut)
                //ステージセレクトへ遷移
                SceneManager.LoadScene("StageSelect");
        }
    }

    /// <summary>
    /// UIの切り替え
    /// </summary>
    /// <param name="pushB"></param>
    private void TitleCommand(bool pushB)
    {
        //ボタンが押されたら
        if (pushB)
        {
            //プッシュテキストキャンバスを非表示
            _pushTextCanvas.alpha = 0;
            //選択用キャンバスを表示
            _commandCanvas.alpha = 1;
            //選択用キャンバスの入力受付を有効
            _commandCanvas.interactable = true;
        }
    }

    /// <summary>
    /// 選択したボタンに応じて処理変更
    /// </summary>
    /// <param name="buttonName"></param>
    public void CommandSelect(string buttonName)
    {
        //ボタンの番号を見て変更
        switch (buttonName)
        {
            //はじめからを選択
            case "Begin":
                //データをすべて消す
                PlayerPrefs.DeleteAll();
                _selectButton = true;
                break;

            //つづきからを選択
            case "Continued":
                _selectButton = true;
                break;
        }
    }
}
