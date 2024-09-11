using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorkPackageContainerPersons : MonoBehaviour
{
    public string id;
    public string personName;
    
    public TextMeshProUGUI personNameText; 
    public bool selected;
    public UserData userData;


    public void UpdateContainer()
    {
        personNameText.text = personName;
    }

    public void Select(bool select)
    {
        selected = select;
    }    
}
