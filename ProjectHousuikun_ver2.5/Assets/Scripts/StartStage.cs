using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStage : MonoBehaviour
{
    private Player _player;

    void Start()
    {
        //フェードアウト中を解除
        Fade.Instance._isFadeOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        //フェードインしていないなら
        if (!Fade.Instance._isFadeIn)
            //フェードインをスタート
            Fade.Instance.StartFadeIn();
    }
}
