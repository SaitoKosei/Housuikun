using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : SingletonMonoBehaviour<Fade>
{
    public Image _fadeImage;                    //フェードのパネルのUI

    public bool _isFadeOut;                     //フェードアウト中か管理
    public bool _isFadeIn;                      //フェードイン中か管理

    private BGM _bgm;
    private SE _se;

    private int _fadeTimer;                     //フェードのタイマー
    private float _fadeSpeed;                   //フェードのスピード
    private float _red, _green, _blue, _alfa;   //フェードパネルのRGBA

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        _bgm = FindObjectOfType<BGM>();
        _se = FindObjectOfType<SE>();

        _fadeSpeed = 0.02f;
        _fadeTimer = 0;
        
        _red = _fadeImage.color.r;
        _green = _fadeImage.color.g;
        _blue = _fadeImage.color.b;
        _alfa = _fadeImage.color.a;

        _isFadeOut = false;
        _isFadeIn = false;
    }

    void Update()
    {
        //フェードインがtrueなら
        if (_isFadeIn)
            //フェードタイマー加算
            ++_fadeTimer;
        if (_fadeTimer > 150)
            //フェードインスタート
                StartFadeIn();
        //フェードアウトがtrueなら
        if (_isFadeOut)
            //フェードアウトスタート
            StartFadeOut();
    }

    /// <summary>
    /// フェードイン
    /// </summary>
    public void StartFadeIn()
    {
        //パネルのalpha値を減らす
        _alfa -= _fadeSpeed;
        //alpha値設定
        SetAlpha();
        //alpha値が０以下で
        if (_alfa <= 0)
        {
            //フェードイン中にする
            _isFadeIn = true;
            //パネルを非表示
            _fadeImage.enabled = false;
            //タイマーをリセット
            _fadeTimer = 0;
        }
    }

    /// <summary>
    /// フェードアウト
    /// </summary>
    public void StartFadeOut()
    {
        //パネルを表示
        _fadeImage.enabled = true;
        //パネルのalpha値を減らす
        _alfa += _fadeSpeed;
        //alpha値設定
        SetAlpha();
        //alpha値が１以上で
        if (_alfa >= 1)
            //フェードアウト中にする
            _isFadeOut = true;

        //AudioSource _audioSource = _bgm.GetComponent<AudioSource>();
    }

    /// <summary>
    /// alpha値設定
    /// </summary>
    private void SetAlpha()
    {
        //パネルのRGBAを設定
        _fadeImage.color = new Color(_red, _green, _blue, _alfa);
    }
}
