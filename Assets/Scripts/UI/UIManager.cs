using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private static Dictionary<string, UIBase> allUIs = new Dictionary<string, UIBase>();

    private UIManager() { }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            Init();
        }
        else
        {
            Destroy(this);
        }
    }

    private void Init()
    {
        RegisterAllUI();
        HideAllUI();
    }

    public void RegisterAllUI()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform child = this.transform.GetChild(i);
            UIBase ui = child.GetComponent<UIBase>();
            if (ui == null)
            {
                continue;
            }
            allUIs[ui.UIName] = ui;
        }
    }

    public void HideAllUI()
    {
        foreach (UIBase ui in allUIs.Values)
        {
            ui.Hide();
        }
    }

    public void ShowUI(string uiName)
    {
        if (allUIs.TryGetValue(uiName, out UIBase ui))
        {
            ui.Show();
        }
    }
}
