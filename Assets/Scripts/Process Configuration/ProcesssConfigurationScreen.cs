using System.Collections.Generic;
using UnityEngine;

public class ProcesssConfigurationScreen : MonoBehaviour
{
    public List<GameObject> menus = new List<GameObject>();

    public void Start()
    {
        for (int i = 0; i < menus.Count; i++)
        {
            menus[i].SetActive(false);
        }
    }

    public void Select(int menu)
    {
        for (int i = 0; i < menus.Count; i++)
        {
            if (i == menu)
            {
                menus[i].SetActive(true);
            }
            else
            {
                menus[i].SetActive(false);
            }
        }
    }
}
