using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorkPackageContainerAssets : MonoBehaviour
{
    public string id;
    public string assetName;

    public TextMeshProUGUI assetNameText;
    public TMP_InputField quantityInputField;

    public bool selected;
    public int quantity;

    public AssetsData assetData;


    public void UpdateContainer()
    {
        assetNameText.text = assetName;
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
