using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE : SingletonMonoBehaviour<SE>
{
    [SerializeField]
    private AudioSource _seSource = default;
    [SerializeField]
    private AudioClip[] _se = default;

    //SEの種類
    public enum SE_TYPE
    {
        NULL,
        COIN,
        DECISION,
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// 引数の種類のSEを再生
    /// </summary>
    /// <param name="setype"></param>
    public void PlaySE(SE_TYPE setype)
    {
        //選んだ種類に応じた番号のAudioClipを再生
        _seSource.PlayOneShot(_se[(int)setype]);
    }
}
