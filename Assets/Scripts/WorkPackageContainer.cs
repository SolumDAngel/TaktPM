using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorkPackageContainer : MonoBehaviour
{
    public string id;
    public string workPackageName;
    public TextMeshProUGUI workPackageNameText;
    public bool selected;

    public void UpdateContainer()
    {
        workPackageNameText.text = workPackageName;
    }
    public void Select(bool select)
    {
        selected = select;
    }
}
