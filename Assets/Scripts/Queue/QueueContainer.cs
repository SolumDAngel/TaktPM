using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QueueContainer : MonoBehaviour
{
    public string id;
    public string queueuName;
    public TextMeshProUGUI queueuNameText;
    public QueueMenu queueuMenu;
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

        queueuNameText.text = queueuName;
        toggle.isOn = selected;
    }
    public void Select(bool select)
    {
        selected = select;
    }
}
