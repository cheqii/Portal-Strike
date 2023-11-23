using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public void OpenUI(GameObject go)
    {
        go.SetActive(true);
    }

    public void CloseUI(GameObject go)
    {
        go.SetActive(false);
    }
}
