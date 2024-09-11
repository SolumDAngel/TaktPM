using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Unity.VisualScripting;
public class WorkPackageMenu : MonoBehaviour
{
    public GameObject workPackageContainer;
    public GameObject workPackageContainerAssets;
    public GameObject workPackageContainerPersons;

    public Transform workPackageContainerTransform;
    public Transform workPackageContainerAssetsEntryTransform;
    public Transform workPackageContainerAssetsExitTransform;
    public Transform workPackageContainerPersonsTransform;

    public GameObject worPackageCreateOverMenu, createButton, editButton;
    public GameObject selectedContainer;

    public TMP_InputField workPackageNameInputField;
    public TMP_InputField searchWorkPackageInputField;
    public TMP_InputField searchAssetsEntryInputField;
    public TMP_InputField searchAssetsExitInputField;
    public TMP_InputField searchPersonsInputField;


    public void Start()
    {
        //if (SaveManager.workPackageList != null)
            LoadContainers();
    }

    public void LoadContainers()
    {
        Dictionary<string, WorkPackageContainerAssets> instantiatedEntryAssets = new Dictionary<string, WorkPackageContainerAssets>();
        Dictionary<string, WorkPackageContainerAssets> instantiatedExitAssets = new Dictionary<string, WorkPackageContainerAssets>();
        Dictionary<string, WorkPackageContainerPersons> instantiatedPersons = new Dictionary<string, WorkPackageContainerPersons>();


        // Atualizando assetsEntry
        foreach (AssetsData assetEntry in SaveManager.appAssetsList.assets)
        {
            bool isSelected = false;
            if (SaveManager.workPackageList != null)
                foreach (WorkPackageData workPackage in SaveManager.workPackageList.workPackages)
                {
                    isSelected = workPackage.assetsEntry.Any(a => a.id == assetEntry.id);
                }

            if (instantiatedEntryAssets.ContainsKey(assetEntry.id))
            {
                // Se já existir, atualiza o nome e o selected
                WorkPackageContainerAssets asset = instantiatedEntryAssets[assetEntry.id];
                asset.assetName = assetEntry.name;
                asset.selected = isSelected;
                asset.UpdateContainer();
            }
            else
            {
                // Instancia apenas se o ID não existir
                GameObject obj = Instantiate(workPackageContainerAssets, workPackageContainerAssetsEntryTransform);
                WorkPackageContainerAssets asset = obj.GetComponent<WorkPackageContainerAssets>();

                asset.id = assetEntry.id;
                asset.assetName = assetEntry.name;
                asset.selected = isSelected;
                asset.UpdateContainer();

                // Adiciona ao dicionário
                instantiatedEntryAssets.Add(assetEntry.id, asset);
            }
        }

        // Atualizando assetsExit
        foreach (AssetsData assetExit in SaveManager.appAssetsList.assets)
        {
            bool isSelected = false;
            if (SaveManager.workPackageList != null)
                foreach (WorkPackageData workPackage in SaveManager.workPackageList.workPackages)
                {
                    isSelected = workPackage.assetsEntry.Any(a => a.id == assetExit.id);
                }
            if (instantiatedExitAssets.ContainsKey(assetExit.id))
            {
                // Atualiza o nome e o selected
                WorkPackageContainerAssets asset = instantiatedExitAssets[assetExit.id];
                asset.assetName = assetExit.name;
                asset.selected = isSelected;
                asset.UpdateContainer();
            }
            else
            {
                GameObject obj = Instantiate(workPackageContainerAssets, workPackageContainerAssetsExitTransform);
                WorkPackageContainerAssets asset = obj.GetComponent<WorkPackageContainerAssets>();

                asset.id = assetExit.id;
                asset.assetName = assetExit.name;
                asset.selected = isSelected;
                asset.UpdateContainer();

                // Adiciona ao dicionário
                instantiatedExitAssets.Add(assetExit.id, asset);
            }
        }

        // Atualizando persons
        foreach (UserData person in SaveManager.userList.users)
        {
            bool isSelected = false;
            if (SaveManager.workPackageList != null)
                foreach (WorkPackageData workPackage in SaveManager.workPackageList.workPackages)
                {
                    isSelected = workPackage.assetsEntry.Any(a => a.id == person.id);
                }
            if (instantiatedPersons.ContainsKey(person.id))
            {
                // Atualiza o nome e o selected
                WorkPackageContainerPersons personContainer = instantiatedPersons[person.id];
                personContainer.personName = person.userName;
                personContainer.selected = isSelected;
                personContainer.UpdateContainer();
            }
            else
            {
                GameObject obj = Instantiate(workPackageContainerPersons, workPackageContainerPersonsTransform);
                WorkPackageContainerPersons personContainer = obj.GetComponent<WorkPackageContainerPersons>();

                personContainer.id = person.id;
                personContainer.personName = person.userName;
                personContainer.selected = isSelected;
                personContainer.UpdateContainer();

                // Adiciona ao dicionário
                instantiatedPersons.Add(person.id, personContainer);
            }
        }

    }



    public void CreateBtn()
    {
        GameObject newContainer = Instantiate(workPackageContainer, workPackageContainerTransform);
        WorkPackageContainer workPackage = newContainer.GetComponent<WorkPackageContainer>();

        workPackage.id = GetFreeID();
        workPackage.workPackageName = workPackageNameInputField.text;


        WorkPackageContainerAssets[] assetsEntry = workPackageContainerAssetsEntryTransform.GetComponentsInChildren<WorkPackageContainerAssets>();
        List<AssetsData> assetsEntryList = new List<AssetsData>();
        List<int> assetsEntryQuantity = new List<int>();
        foreach (WorkPackageContainerAssets asset in assetsEntry)
        {
            if (asset.selected)
            {
                assetsEntryList.Add(asset.assetData);
                assetsEntryQuantity.Add(asset.quantity);
            }
        }

        WorkPackageContainerAssets[] assetsExist = workPackageContainerAssetsExitTransform.GetComponentsInChildren<WorkPackageContainerAssets>();
        List<AssetsData> assetsExitList = new List<AssetsData>();
        List<int> assetsExitQuantity = new List<int>();
        foreach (WorkPackageContainerAssets asset in assetsEntry)
        {
            if (asset.selected)
            {
                assetsExitList.Add(asset.assetData);
                assetsExitQuantity.Add(asset.quantity);
            }
        }

        WorkPackageContainerPersons[] persons = workPackageContainerPersonsTransform.GetComponentsInChildren<WorkPackageContainerPersons>();
        List<UserData> personsList = new List<UserData>();
        foreach (WorkPackageContainerPersons person in persons)
        {
            if (person.selected)
                personsList.Add(person.userData);
        }



        WorkPackageData workPackageData = new WorkPackageData()
        {
            id = workPackage.id,
            workPackageName = workPackage.workPackageName,
            assetsEntry = assetsEntryList,
            assetsEntryQuantity = assetsEntryQuantity,
            assetsExit = assetsExitList,
            assetsExitQuantity = assetsExitQuantity,
            persons = personsList
        };
        SaveToJson(workPackageData);
    }


    public void SaveToJson(WorkPackageData workPackageData)
    {
        SaveManager.SaveWorkPackageData(workPackageData);
    }

    public string GetFreeID()
    {
        HashSet<int> usedIDs = new HashSet<int>();

        if (SaveManager.workPackageList != null)
        {
            foreach (WorkPackageData workPackage in SaveManager.workPackageList.workPackages)
            {
                if (int.TryParse(workPackage.id, out int id))
                {
                    usedIDs.Add(id);
                }
            }
        }
        else
        {
            usedIDs.Add(0); 
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
    public void CreateContainerBtn()
    {
        worPackageCreateOverMenu.SetActive(true);
        createButton.SetActive(true);
        editButton.SetActive(false);
    }

    public void CancelBtn()
    {

    }

}

