using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelGroup : MonoBehaviour
{
    public List<GameObject> panel;

    public void Show(int idPanel)
    {
        for (int i = 0; i < panel.Count; i++)
        {
            panel[i].SetActive(i == idPanel);
        }
    }
}
