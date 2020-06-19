using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectStage : MonoBehaviour
{
    enum STAGE
    {
        NULL,
        STAGE1,
        STAGE2,
        STAGE3,
        STAGE4,
        STAGE5,
    }

    public bool _selectStage;    //ステージを選択しているか管理

    private Player _player;

    private int _loadtageNum; //選択したステージの番号を保存

    void Start()
    {
        _player = FindObjectOfType<Player>();

        //フェードイン中を解除
        Fade.Instance._isFadeOut = false;
        _selectStage = false;
        _loadtageNum = 0;
    }

    void Update()
    {
        //フェードインしていなかったら
        if(!Fade.Instance._isFadeIn)
            //フェードインスタート
            Fade.Instance.StartFadeIn();

        //パイプの前に立った状態で決定ボタンを押すと
        if (Input.GetButtonDown("Decision") && _player._selectStageNum != (int)STAGE.NULL)
        {
            //選択したのでtrue
            _selectStage = true;
            //選択したステージの番号保存
            _loadtageNum = _player._selectStageNum;
            //フェードインを解除
            Fade.Instance._isFadeIn = false;
        }

        //ステージが選択されたら
        if (_selectStage)
            //フェードアウトスタート
            Fade.Instance.StartFadeOut();
        //フェードアウト中になったら
        if (Fade.Instance._isFadeOut)
            //選んだステージに応じてシーン遷移
            SelectPipe();
    }

    /// <summary>
    /// 選んだステージに応じてステージ遷移
    /// </summary>
    private void SelectPipe()
    {
        //選んだステージの番号によって遷移するステージを変更
        switch (_loadtageNum)
        {
            case (int)STAGE.STAGE1:
                SceneManager.LoadScene("Stage1");
                break;
            case (int)STAGE.STAGE2:
                SceneManager.LoadScene("Stage2");
                break;
            case (int)STAGE.STAGE3:
                SceneManager.LoadScene("Stage3");
                break;
            case (int)STAGE.STAGE4:
                SceneManager.LoadScene("Stage4");
                break;
            case (int)STAGE.STAGE5:
                SceneManager.LoadScene("Stage5");
                break;
            default:
                break;
        }
    }  
}
