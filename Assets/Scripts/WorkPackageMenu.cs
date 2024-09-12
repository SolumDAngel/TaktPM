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
    public WorkPackageContainer selectedContainer;

    public TMP_InputField workPackageNameInputField;
    public TMP_InputField searchWorkPackageInputField;
    public TMP_InputField searchAssetsEntryInputField;
    public TMP_InputField searchAssetsExitInputField;
    public TMP_InputField searchPersonsInputField;


    public void Start()
    {
        foreach (WorkPackageData workPackageData in SaveManager.workPackageList.workPackages)
        {
            GameObject obj = Instantiate(workPackageContainer, workPackageContainerTransform);
            WorkPackageContainer workPackage = obj.GetComponent<WorkPackageContainer>();

            workPackage.id = workPackageData.id;
            workPackage.workPackageName = workPackageData.workPackageName;
            workPackage.UpdateContainer();
        }


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
                asset.assetData = assetEntry;
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
                asset.assetData = assetEntry;
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
                    isSelected = workPackage.assetsExit.Any(a => a.id == assetExit.id);
                }
            if (instantiatedExitAssets.ContainsKey(assetExit.id))
            {
                // Atualiza o nome e o selected
                WorkPackageContainerAssets asset = instantiatedExitAssets[assetExit.id];
                asset.assetName = assetExit.name;
                asset.selected = isSelected;
                asset.assetData = assetExit;
                asset.UpdateContainer();
            }
            else
            {
                GameObject obj = Instantiate(workPackageContainerAssets, workPackageContainerAssetsExitTransform);
                WorkPackageContainerAssets asset = obj.GetComponent<WorkPackageContainerAssets>();

                asset.id = assetExit.id;
                asset.assetName = assetExit.name;
                asset.selected = isSelected;
                asset.assetData = assetExit;
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
                    isSelected = workPackage.persons.Exists(a => a.id == person.id);
                }
            if (instantiatedPersons.ContainsKey(person.id))
            {
                // Atualiza o nome e o selected
                WorkPackageContainerPersons personContainer = instantiatedPersons[person.id];
                personContainer.personName = person.userName;
                personContainer.selected = isSelected;
                personContainer.userData = person;
                personContainer.UpdateContainer();
            }
            else
            {
                GameObject obj = Instantiate(workPackageContainerPersons, workPackageContainerPersonsTransform);
                WorkPackageContainerPersons personContainer = obj.GetComponent<WorkPackageContainerPersons>();

                personContainer.id = person.id;
                personContainer.personName = person.userName;
                personContainer.selected = isSelected;
                personContainer.userData = person;
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
        workPackage.UpdateContainer();

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

        WorkPackageContainerAssets[] assetsExit = workPackageContainerAssetsExitTransform.GetComponentsInChildren<WorkPackageContainerAssets>();
        List<AssetsData> assetsExitList = new List<AssetsData>();
        List<int> assetsExitQuantity = new List<int>();
        foreach (WorkPackageContainerAssets asset in assetsExit)
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
        worPackageCreateOverMenu.SetActive(false);
    }




    public void EditBtn()
    {
        WorkPackageContainer workPackage = selectedContainer;
        WorkPackageData workPackageData = SaveManager.workPackageList.workPackages.Find(x => x.id == workPackage.id);

        workPackage.id = selectedContainer.id;
        workPackage.workPackageName = workPackageNameInputField.text;
        workPackage.UpdateContainer();

        WorkPackageContainerAssets[] assetsEntry = workPackageContainerAssetsEntryTransform.GetComponentsInChildren<WorkPackageContainerAssets>();
        List<AssetsData> assetsEntryList = new List<AssetsData>();
        List<int> assetsEntryQuantity = new List<int>();
        foreach (WorkPackageContainerAssets asset in assetsEntry)
        {
            if (asset.selected)
            {
                if (workPackageData.assetsEntry.Exists(x => x.id == asset.assetData.id))
                {
                    int editWorkPackageAssetEntryData = workPackageData.assetsEntry.FindIndex(assets => assets.id == asset.assetData.id);
                    workPackageData.assetsEntry[editWorkPackageAssetEntryData] = asset.assetData;
                    workPackageData.assetsEntryQuantity[editWorkPackageAssetEntryData] = asset.quantity;
                }
                assetsEntryList.Add(asset.assetData);
                assetsEntryQuantity.Add(asset.quantity);
            }
        }

        WorkPackageContainerAssets[] assetsExit = workPackageContainerAssetsExitTransform.GetComponentsInChildren<WorkPackageContainerAssets>();
        List<AssetsData> assetsExitList = new List<AssetsData>();
        List<int> assetsExitQuantity = new List<int>();
        foreach (WorkPackageContainerAssets asset in assetsExit)
        {
            if (asset.selected)
            {
                if (workPackageData.assetsExit.Exists(x => x.id == asset.assetData.id))
                {
                    int editWorkPackageAssetExitData = workPackageData.assetsExit.FindIndex(assets => assets.id == asset.assetData.id);
                    workPackageData.assetsExit[editWorkPackageAssetExitData] = asset.assetData;
                    workPackageData.assetsExitQuantity[editWorkPackageAssetExitData] = asset.quantity;
                }
                assetsExitList.Add(asset.assetData);
                assetsExitQuantity.Add(asset.quantity);
            }
        }

        WorkPackageContainerPersons[] persons = workPackageContainerPersonsTransform.GetComponentsInChildren<WorkPackageContainerPersons>();
        List<UserData> personsList = new List<UserData>();
        foreach (WorkPackageContainerPersons person in persons)
        {
            if (person.selected)
            {
                if (workPackageData.persons.Exists(x => x.id == person.id))
                {
                    int editWorkPackagePersonData = workPackageData.persons.FindIndex(persons => persons.id == person.id);
                    workPackageData.persons[editWorkPackagePersonData] = person.userData;
                }
                personsList.Add(person.userData);
            }
        }

        workPackageData.id = workPackage.id;
        workPackageData.workPackageName = workPackage.workPackageName;
        workPackageData.assetsEntry = assetsEntryList;
        workPackageData.assetsEntryQuantity = assetsEntryQuantity;
        workPackageData.assetsExit = assetsExitList;
        workPackageData.assetsExitQuantity = assetsExitQuantity;
        workPackageData.persons = personsList;

        SaveToJson(workPackageData);
        worPackageCreateOverMenu.SetActive(false);
    }


    public void EditContainerBtn()
    {
        selectedContainer = null;
        WorkPackageContainer[] workPackage = workPackageContainerTransform.GetComponentsInChildren<WorkPackageContainer>();
        foreach (WorkPackageContainer workpackageData in workPackage)
        {

            if (workpackageData.selected)
            {
                selectedContainer = workpackageData;
                break;
            }
        }
        if (selectedContainer == null)
        {
            Debug.Log("No asset container selected");
            return;
        }
        worPackageCreateOverMenu.SetActive(true);
        createButton.SetActive(false);
        editButton.SetActive(true);


         Debug.Log("Selected Container ID: " + selectedContainer.id);

        //int editWorkPackageData = SaveManager.workPackageList.workPackages.FindIndex(workPackage => workPackage.id == selectedContainer.id);

        int editWorkPackageData = 0;
        for (int i = 0; i < SaveManager.workPackageList.workPackages.Count; i++)
        {
            if (SaveManager.workPackageList.workPackages[i].id == selectedContainer.id)
            {
                editWorkPackageData = i;
            }
        }

        Debug.Log("Found Index: " + editWorkPackageData);

        WorkPackageContainerAssets[] assetsEntry = workPackageContainerAssetsEntryTransform.GetComponentsInChildren<WorkPackageContainerAssets>();
        if (SaveManager.workPackageList.workPackages.Exists(u => u.id == selectedContainer.id))
        {
            foreach (WorkPackageContainerAssets assetEntryData in assetsEntry)
            {
                assetEntryData.quantity = 0;
                assetEntryData.quantityInputField.text = "0";
                foreach (AssetsData assetsData in SaveManager.workPackageList.workPackages[editWorkPackageData].assetsEntry)
                    if (assetEntryData.assetData.id == assetsData.id)
                    {
                        assetEntryData.selected = true;
                        assetEntryData.quantity =
                            SaveManager.workPackageList.workPackages[editWorkPackageData].assetsEntryQuantity
                            [SaveManager.workPackageList.workPackages[editWorkPackageData].assetsEntry.IndexOf(assetsData)];
                        assetEntryData.quantityInputField.text = assetEntryData.quantity.ToString();
                        assetEntryData.UpdateContainer();
                    }
            }
        }

        WorkPackageContainerAssets[] assetsExit = workPackageContainerAssetsExitTransform.GetComponentsInChildren<WorkPackageContainerAssets>();
        if (SaveManager.workPackageList.workPackages.Exists(u => u.id == selectedContainer.id))
        {
            foreach (WorkPackageContainerAssets assetExitData in assetsExit)
            {
                assetExitData.quantity = 0;
                assetExitData.quantityInputField.text = "0";
                foreach (AssetsData assetsData in SaveManager.workPackageList.workPackages[editWorkPackageData].assetsExit)
                    if (assetExitData.assetData.id == assetsData.id)
                    {
                        assetExitData.selected = true;
                        assetExitData.quantity =
                            SaveManager.workPackageList.workPackages[editWorkPackageData].assetsExitQuantity
                            [SaveManager.workPackageList.workPackages[editWorkPackageData].assetsExit.IndexOf(assetsData)];
                        assetExitData.quantityInputField.text = assetExitData.quantity.ToString();
                        assetExitData.UpdateContainer();
                    }
            }
        }

        WorkPackageContainerPersons[] persons = workPackageContainerPersonsTransform.GetComponentsInChildren<WorkPackageContainerPersons>();
        if (SaveManager.workPackageList.workPackages.Exists(u => u.id == selectedContainer.id))
        {
            foreach (WorkPackageContainerPersons personData in persons)
                foreach (UserData userData in SaveManager.workPackageList.workPackages[editWorkPackageData].persons)
                    if (personData.userData.id == userData.id)
                    {
                        personData.selected = true;
                        personData.UpdateContainer();
                    }
        }
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
        workPackageNameInputField.text = "";
        WorkPackageContainerAssets[] assetsEntry = workPackageContainerAssetsEntryTransform.GetComponentsInChildren<WorkPackageContainerAssets>();
        foreach (WorkPackageContainerAssets asset in assetsEntry)
        {
            asset.selected = false;
            asset.quantity = 0;
            asset.quantityInputField.text = "0";
            asset.UpdateContainer();
        }
        
        WorkPackageContainerAssets[] assetsExit = workPackageContainerAssetsExitTransform.GetComponentsInChildren<WorkPackageContainerAssets>();
        foreach (WorkPackageContainerAssets asset in assetsExit)
        {
            asset.selected = false;
            asset.quantity = 0;
            asset.quantityInputField.text = "0";
            asset.UpdateContainer();
        }
        WorkPackageContainerPersons[] persons = workPackageContainerPersonsTransform.GetComponentsInChildren<WorkPackageContainerPersons>();
        foreach (WorkPackageContainerPersons person in persons)
        {
            person.selected = false;
            person.UpdateContainer();
        }   
    }

    public void CancelBtn()
    {
        worPackageCreateOverMenu.SetActive(false);
        workPackageNameInputField.text = "";
    }

}

