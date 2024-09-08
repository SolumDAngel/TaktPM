using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AssetsMenu : MonoBehaviour
{
    public GameObject assetContainer;
    public Transform assetContainerParent;

    public GameObject assetCreateOverMenu, createButton, editButton;
    public GameObject selectedContainer;

    public TMP_InputField nameInputField;
    public TMP_InputField descriptionInputField;

    private void Start()
    {
       foreach(AssetsData assetData in SaveManager.appAssetsData.assets)
        {
            GameObject obj = Instantiate(assetContainer, assetContainerParent);
            AssetContainer asset = obj.GetComponent<AssetContainer>();

            asset.id = assetData.id;
            asset.assetName = assetData.name;
            asset.description = assetData.description;
            asset.UpdateContainer();
        }
    }

    public string GetFreeID()
    {
        HashSet<int> usedIDs = new HashSet<int>();

        // Adiciona todos os IDs já usados à lista de IDs usados
        foreach (AssetsData asset in SaveManager.appAssetsData.assets)
        {
            if (int.TryParse(asset.id, out int id))
            {
                usedIDs.Add(id);
            }
        }

        // Encontra o maior ID usado
        int maxID = 0;
        if (usedIDs.Count > 0)
        {
            maxID = usedIDs.Max();
        }

        // Gera um novo ID maior que o maior ID encontrado
        int newID = maxID + 1;
        return newID.ToString();
    }



    public void CreateAssetBtn()
    {
        if(nameInputField.text == "")
        {
            print("Please enter a name for the new asset"); 
            return;
        }

        AssetContainer[] assets = assetContainerParent.GetComponentsInChildren<AssetContainer>();

        foreach (AssetContainer assetsData in assets)
        {
            if (assetsData.assetName == nameInputField.text)
            {                
                assetsData.description = descriptionInputField.text;
                SaveToJson(assetsData.id);
                assetsData.UpdateContainer();
                assetCreateOverMenu.SetActive(false);
                return;
            }
        }

        GameObject obj = Instantiate(assetContainer, assetContainerParent);
        AssetContainer asset = obj.GetComponent<AssetContainer>();

        asset.id = GetFreeID();
        asset.assetName = nameInputField.text;
        asset.description = descriptionInputField.text;
        SaveToJson(asset.id);
        asset.UpdateContainer();
        assetCreateOverMenu.SetActive(false);
        assets = new AssetContainer[0];
    }
    public void EditAssetBtn()
    {
        GameObject obj = selectedContainer;
        AssetContainer asset = obj.GetComponent<AssetContainer>();

        asset.assetName = nameInputField.text;
        asset.description = descriptionInputField.text;
        SaveToJson(asset.id);
        asset.UpdateContainer();
        assetCreateOverMenu.SetActive(false);       
    }

    public void SaveToJson(string id)
    {
        SaveManager.SaveAssetData(id, nameInputField.text, descriptionInputField.text);
    }


    public void CreateContainerBtn()
    {
        assetCreateOverMenu.SetActive(true);
        createButton.SetActive(true);
        editButton.SetActive(false);

        nameInputField.text = "";
        descriptionInputField.text = "";
    }
    public void EditContainerBtn()
    {
        selectedContainer = null;
        AssetContainer[] assets = assetContainerParent.GetComponentsInChildren<AssetContainer>();
        foreach (AssetContainer assetsData in assets)
        {
   
            if (assetsData.selected)
            {
                selectedContainer = assetsData.gameObject;
                break;
            }
        }      
        if (selectedContainer == null)
        {
            Debug.Log("No asset container selected");
            return;
        }

        AssetContainer asset = selectedContainer.GetComponent<AssetContainer>();
        nameInputField.text = asset.assetName;
        descriptionInputField.text = asset.description;


        assetCreateOverMenu.SetActive(true);
        createButton.SetActive(false);
        editButton.SetActive(true);
    }


    public void CancelBtn()
    {
        assetCreateOverMenu.SetActive(false);
        nameInputField.text = "";
        descriptionInputField.text = "";
    }

    public void RemoveBtn()
    {
        selectedContainer = null;
        AssetContainer[] assets = assetContainerParent.GetComponentsInChildren<AssetContainer>();
        AssetContainer asset = null;
        foreach (AssetContainer assetsData in assets)
        {
            if (assetsData.selected)
            {
                asset = assetsData;
                selectedContainer = assetsData.gameObject;
                break;
            }
        }
        if (selectedContainer == null)
        {
            Debug.Log("No asset container selected");
            return;
        }
        SaveManager.RemoveAsset(asset.id);
        Destroy(selectedContainer);
        assets = new AssetContainer[0];
    }
}
