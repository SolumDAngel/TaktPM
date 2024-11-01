using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QueueWorkPackageContainer : MonoBehaviour
{
    public string id;
    public string workPackageName;

    public TextMeshProUGUI workPackageNameText;

    public bool selected;

    public WorkPackageData workPackageData;
    public QueueMenu queueMenu;
    public GameObject checkMark;
    public Toggle toggle;

    public void UpdateContainer()
    {
        workPackageNameText.text = workPackageName;
        checkMark.SetActive(selected);
        toggle.isOn = selected;
    }

    public void Select(bool select)
    {
        selected = select;
    }
}
