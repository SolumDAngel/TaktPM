using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PersonContainer : MonoBehaviour
{
    public string id;
    public string personName;
    public TextMeshProUGUI personNameText;
    public bool selected;
    public void UpdateContainer()
    {
        personNameText.text = personName;
    }
    public void Select(bool select)
    {
        selected = select;
    }
}
