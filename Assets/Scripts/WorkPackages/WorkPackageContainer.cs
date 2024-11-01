using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkPackageContainer : MonoBehaviour
{
    public string id;
    public string workPackageName;
    public TextMeshProUGUI workPackageNameText;
    public WorkPackageMenu workPackageMenu;
    public Toggle toggle;
    public bool selected;

    private void Start()
    {
        toggle = GetComponent<Toggle>();
    }

    public void UpdateContainer()
    {
        if (toggle == null)
            toggle = GetComponent<Toggle>();

        workPackageNameText.text = workPackageName;
        toggle.isOn = selected;
    }
    public void Select(bool select)
    {
        workPackageMenu.ResetSelecteds();
        selected = select;
    }
}
