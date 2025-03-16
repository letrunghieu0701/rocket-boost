using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    public string UIName => this.gameObject.name;

    public virtual void Show() => this.gameObject.SetActive(true);
    public virtual void Hide() => this.gameObject.SetActive(false);
}
