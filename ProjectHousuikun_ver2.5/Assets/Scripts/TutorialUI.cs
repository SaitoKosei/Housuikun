using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    //チュートリアルUIの種類
    private enum TUTORIAL_UI
    {
        NULL,
        WALK,
        SUPPLY,
        DISCHARGE,
        SMALLSIZE,
        DANGER,
    }

    //チュートリアルの場所
    private enum TUTORIAL_POSITION
    {
        SUPPLY_S,
        DISCHARGE_S,
        FIRST_NOT_VIEW,
        SMALLSIZE_S,
        SECOND_NOT_VIEW,
        DANGER_S,
        THIRD_NOT_VIEW,
        VIEW_TOP,
    }

    [SerializeField]
    private Transform[] _trans = default;         //チュートリアルUIを表示させる座標を管理
    [SerializeField]
    private Sprite[] _tutorialImage = default;    //チュートリアルUIの画像を管理

    private Player _player;
    private Image _uiColor;             //UIのRGBA

    private Color _displayUIColor;      //UIを表示する時のRGBA
    private Color _hiddenUIColor;       //UIを非表示する時のRGBA

    void Start()
    {
        _player = FindObjectOfType<Player>();
        _uiColor = GetComponent<Image>();

        _displayUIColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
        _hiddenUIColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0);
    }

    void Update()
    {
        //今の座標がチュートリアルを表示させるY座標の上限より下だったら
        if(_trans[(int)TUTORIAL_POSITION.VIEW_TOP].position.y > _player.transform.position.y)
        {
            //歩きのチュートリアルUIを表示
            if (_trans[(int)TUTORIAL_POSITION.SUPPLY_S].position.x > _player.transform.position.x)
                UpdateUI((int)TUTORIAL_UI.WALK);

            //給水のチュートリアルUIを表示
            if (_trans[(int)TUTORIAL_POSITION.SUPPLY_S].position.x < _player.transform.position.x &&
                _trans[(int)TUTORIAL_POSITION.DISCHARGE_S].position.x > _player.transform.position.x)
                UpdateUI((int)TUTORIAL_UI.SUPPLY);

            //放水のチュートリアルUIを表示
            if (_trans[(int)TUTORIAL_POSITION.DISCHARGE_S].position.x < _player.transform.position.x &&
                _trans[(int)TUTORIAL_POSITION.FIRST_NOT_VIEW].position.x > _player.transform.position.x)
                UpdateUI((int)TUTORIAL_UI.DISCHARGE);

            //チュートリアルUIを非表示
            if (_trans[(int)TUTORIAL_POSITION.FIRST_NOT_VIEW].position.x < _player.transform.position.x &&
                _trans[(int)TUTORIAL_POSITION.SMALLSIZE_S].position.x > _player.transform.position.x)
                _uiColor.color = _hiddenUIColor;

            //小さくなるチュートリアルUIを表示
            if (_trans[(int)TUTORIAL_POSITION.SMALLSIZE_S].position.x < _player.transform.position.x &&
                _trans[(int)TUTORIAL_POSITION.SECOND_NOT_VIEW].position.x > _player.transform.position.x)
            {
                _uiColor.color = _displayUIColor;
                UpdateUI((int)TUTORIAL_UI.SMALLSIZE);
            }

            //チュートリアルUIを非表示
            if (_trans[(int)TUTORIAL_POSITION.SECOND_NOT_VIEW].position.x < _player.transform.position.x &&
                _trans[(int)TUTORIAL_POSITION.DANGER_S].position.x > _player.transform.position.x)
                _uiColor.color = _hiddenUIColor;

            //即死ブロックのチュートリアルUIを表示
            if (_trans[(int)TUTORIAL_POSITION.DANGER_S].position.x < _player.transform.position.x &&
                _trans[(int)TUTORIAL_POSITION.THIRD_NOT_VIEW].position.x > _player.transform.position.x)
            {
                _uiColor.color = _displayUIColor;
                UpdateUI((int)TUTORIAL_UI.DANGER);
            }

            //チュートリアルUIを非表示
            if (_trans[(int)TUTORIAL_POSITION.THIRD_NOT_VIEW].position.x < _player.transform.position.x)
                _uiColor.color = _hiddenUIColor;
        }
    }

    /// <summary>
    /// チュートリアルUIの画像を差し替える
    /// </summary>
    /// <param name="tutorial"></param>
    private void UpdateUI(int tutorial)
    {
        Image image = GetComponent<Image>();
        //UIの画像を差し替え
        image.sprite = _tutorialImage[tutorial];
    }
}