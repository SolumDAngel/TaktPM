using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkPackageContainerAssets : MonoBehaviour
{
    public string id;
    public string assetName;

    public TextMeshProUGUI assetNameText;
    public TMP_InputField quantityInputField;

    public bool selected;
    public int quantity;

    public AssetsData assetData;
    public GameObject checkMark;
    public Toggle toggle;

    public void UpdateContainer()
    {
        assetNameText.text = assetName;
        checkMark.SetActive(selected);
        toggle.isOn = selected;
    }
    public void Select(bool select)
    {
        selected = select;
    }
    public void QuantityInputField(string quantityInputField)
    {
        quantity = int.Parse(quantityInputField);
    }
    public void AddQuantity()
    {
        quantity++;
        quantityInputField.text = quantity.ToString();
    }
    public void RemoveQuantity()
    {
        quantity--;
        quantityInputField.text = quantity.ToString();
    }
}
