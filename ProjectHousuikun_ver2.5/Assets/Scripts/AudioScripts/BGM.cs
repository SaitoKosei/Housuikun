using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    [SerializeField]
    private AudioSource _bgmSource = default;
    [SerializeField]
    private AudioClip[] _bgmClip = default;

    private AudioMnager _audioManager;

    private void Start()
    {
        _audioManager = FindObjectOfType<AudioMnager>();
    }

    /// <summary>
    /// 引数の番号のBGMを再生
    /// </summary>
    /// <param name="bgmNum"></param>
    public void PlayBGM(int bgmNum)
    {
        //選んだ番号のAudioClipを再生
        _bgmSource.PlayOneShot(_bgmClip[bgmNum]);
    }

    /// <summary>
    /// BGMを停止
    /// </summary>
    public void StopBGM()
    {
        //選んだ番号のAudioClipを停止
        _bgmSource.Stop();
        //再生中を停止中にする
        _audioManager._audioPlay = false;
    }
}
