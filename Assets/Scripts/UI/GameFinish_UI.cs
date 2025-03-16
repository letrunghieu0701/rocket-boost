using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFinish_UI : UIBase
{
    private Button _backToMenuBtn;

    private void Awake()
    {
        _backToMenuBtn = this.transform.Find("Canvas/BackToMenuBtn").GetComponent<Button>();
        _backToMenuBtn.onClick.AddListener(OnClickBackToMenuBtn);
    }

    private void OnClickBackToMenuBtn()
    {
        LevelManager.Instance.BackToStartScene();
    }
}
