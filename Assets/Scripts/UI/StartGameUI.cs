using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameUI : UIBase
{
    private Button _startGameBtn;

    private void Awake()
    {
        _startGameBtn = this.transform.Find("Canvas/StartGameBtn").GetComponent<Button>();
        _startGameBtn.onClick.AddListener(OnClickStartGameBtn);
    }

    private void OnClickStartGameBtn()
    {
        GameManager.Instance.StartGame();
    }
}
