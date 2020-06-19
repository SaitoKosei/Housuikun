using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClear : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("Decision") || Input.anyKey)
            //決定ボタンを押すとステージセレクトに遷移
            SceneManager.LoadScene("StageSelect");
    }
}
