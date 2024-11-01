using TMPro;
using UnityEngine;

public class AssetContainer : MonoBehaviour
{
    public bool selected;
    public string assetName;
    public string description;
    public TextMeshProUGUI assetNameText;
    public TextMeshProUGUI descriptionText;
    public string id;
    public void UpdateContainer()
    {
        assetNameText.text = assetName;
        descriptionText.text = description;
    }

    public void Select(bool select)
    {
        selected = select;
    }
}
