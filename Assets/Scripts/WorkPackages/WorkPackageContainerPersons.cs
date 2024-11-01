using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkPackageContainerPersons : MonoBehaviour
{
    public string id;
    public string personName;
    
    public TextMeshProUGUI personNameText; 
    public bool selected;
    public UserData userData;
    public GameObject checkBox;
    public Toggle toogle;

    public void UpdateContainer()
    {
        personNameText.text = personName;
        checkBox.SetActive(selected);
        toogle.isOn = selected;
    }

    public void Select(bool select)
    {
        selected = select;
    }    
}
