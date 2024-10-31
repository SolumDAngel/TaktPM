using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class QueueMenu : MonoBehaviour
{
    public GameObject queueContainer;
    public GameObject queueWorkPackageContainer;

    public Transform queueContainerTransform;
    public Transform queueWorkPackageContainerEntryTransform;
    public Transform queueWorkPackageContainerExitTransform;

    public GameObject queueCreateOverMenu, createButton, editButton;
    public QueueContainer selectedContainer;

    public TMP_InputField queueNameInputField;
    public TMP_InputField entryWorkPackageInputField;
    public TMP_InputField exitWorkPackageInputField;

    private void Start()
    {
        foreach (QueuesData queueData in SaveManager.queueList.queues)
        {
            GameObject obj = Instantiate(queueContainer, queueContainerTransform);
            QueueContainer queue = obj.GetComponent<QueueContainer>();

            queue.id = queueData.id;
            queue.queueuName = queueData.queueName;
            queue.queueuMenu = this;
            queue.UpdateContainer();
        }


        LoadWorkPackages();
    }
    public void SearchQueueContainer(string name)
    {
        string searchText = name.ToLower();

        foreach (QueueContainer queue in queueContainerTransform.GetComponentsInChildren<QueueContainer>(true))
        {
            if (string.IsNullOrWhiteSpace(name))
                queue.gameObject.SetActive(true);
            else
                queue.gameObject.SetActive(queue.queueuName.ToLower().Contains(name.ToLower()));
        }
    }
    public void SearchWorkPackageEntryContainer(string name)
    {
        string searchText = name.ToLower();

        foreach (QueueWorkPackageContainer workPackageEntry in queueWorkPackageContainerEntryTransform.GetComponentsInChildren<QueueWorkPackageContainer>(true))
        {
            if (string.IsNullOrWhiteSpace(name))
                workPackageEntry.gameObject.SetActive(true);
            else
                workPackageEntry.gameObject.SetActive(workPackageEntry.workPackageName.ToLower().Contains(name.ToLower()));
        }
    }

    public void SearchWorkPackageExitContainer(string name)
    {
        string searchText = name.ToLower();

        foreach (QueueWorkPackageContainer workPackageExit in queueWorkPackageContainerExitTransform.GetComponentsInChildren<QueueWorkPackageContainer>(true))
        {
            if (string.IsNullOrWhiteSpace(name))
                workPackageExit.gameObject.SetActive(true);
            else
                workPackageExit.gameObject.SetActive(workPackageExit.workPackageName.ToLower().Contains(name.ToLower()));
        }
    }





    public void LoadWorkPackages()
    {
        Dictionary<string, QueueWorkPackageContainer> instantiatedWorkPackages = new Dictionary<string, QueueWorkPackageContainer>();


        foreach (WorkPackageData workPackages in SaveManager.workPackageList.workPackages)
        {
            if (instantiatedWorkPackages.ContainsKey(workPackages.id))
            {         
                QueueWorkPackageContainer workPackage = instantiatedWorkPackages[workPackages.id];
                workPackage.workPackageName = workPackages.workPackageName;
                workPackage.selected = false; 
            }
            else
            {
                // Instancia apenas se o ID não existir
                GameObject obj = Instantiate(queueWorkPackageContainer, queueWorkPackageContainerEntryTransform);
                QueueWorkPackageContainer queueWorkPackage = obj.GetComponent<QueueWorkPackageContainer>();

                queueWorkPackage.id = workPackages.id;
                queueWorkPackage.workPackageName = workPackages.workPackageName;
                queueWorkPackage.workPackageData = workPackages;
                queueWorkPackage.queueMenu = this;
                queueWorkPackage.selected = false;
                queueWorkPackage.UpdateContainer();

                obj = Instantiate(queueWorkPackageContainer, queueWorkPackageContainerExitTransform);
                queueWorkPackage = obj.GetComponent<QueueWorkPackageContainer>();

                queueWorkPackage.id = workPackages.id;
                queueWorkPackage.workPackageName = workPackages.workPackageName;
                queueWorkPackage.workPackageData = workPackages;
                queueWorkPackage.queueMenu = this;
                queueWorkPackage.selected = false;
                queueWorkPackage.UpdateContainer();
            }
        }
    }

    public void CreateQueueContainerBtn()
    {
        queueCreateOverMenu.SetActive(true);
        createButton.SetActive(true);
        editButton.SetActive(false);
        QueueWorkPackageContainer[] workPackageContainerEntry = queueWorkPackageContainerEntryTransform.GetComponentsInChildren<QueueWorkPackageContainer>();
        foreach (QueueWorkPackageContainer workPackageEntry in workPackageContainerEntry)
        {
            workPackageEntry.selected = false;
            workPackageEntry.UpdateContainer();
        }


        QueueWorkPackageContainer[] workPackageContainerExit = queueWorkPackageContainerExitTransform.GetComponentsInChildren<QueueWorkPackageContainer>();
        foreach (QueueWorkPackageContainer workPackageExit in workPackageContainerExit)
        {
            workPackageExit.selected = false;
            workPackageExit.UpdateContainer();
        }

    }

    public void CreateQueueBtn()
    {
        GameObject newContainer = Instantiate(queueContainer, queueContainerTransform);
        QueueContainer queue = newContainer.GetComponent<QueueContainer>();

        queue.id = GetFreeID();
        queue.queueuName = queueNameInputField.text;
        queue.queueuMenu = this;
        queue.UpdateContainer();

        QueueWorkPackageContainer[] workPackageEntry = queueWorkPackageContainerEntryTransform.GetComponentsInChildren<QueueWorkPackageContainer>(true);
        List<WorkPackageData> workPackageEntryList = new List<WorkPackageData>();       
        foreach (QueueWorkPackageContainer workPackage in workPackageEntry)
        {
            if (workPackage.selected)
            {
                workPackageEntryList.Add(workPackage.workPackageData);
            }
        }

        QueueWorkPackageContainer[] workPackageExit = queueWorkPackageContainerExitTransform.GetComponentsInChildren<QueueWorkPackageContainer>(true);
        List<WorkPackageData> workPackageExitList = new List<WorkPackageData>();
        foreach (QueueWorkPackageContainer workPackage in workPackageExit)
        {
            if (workPackage.selected)
            {
                workPackageExitList.Add(workPackage.workPackageData);
            }
        }

        QueuesData queueData = new QueuesData()
        {
            id = queue.id,
            queueName = queue.queueuName,
            entryWorkPackages = workPackageEntryList,
            exitWorkPackages = workPackageExitList,
        };


        SaveToJson(queueData);
        queueCreateOverMenu.SetActive(false);
        ResetSelecteds();
        entryWorkPackageInputField.text = "";
        exitWorkPackageInputField.text = "";
    }

    public void EditBtn()
    {
        QueueContainer queue = selectedContainer;
        QueuesData queueData = SaveManager.queueList.queues.Find(x => x.id == queue.id);

        queue.id = selectedContainer.id;
        queue.queueuName = queueNameInputField.text;
        queue.queueuMenu = this;
        queue.UpdateContainer();

        QueueWorkPackageContainer[] workPackagesEntry = queueWorkPackageContainerEntryTransform.GetComponentsInChildren<QueueWorkPackageContainer>(true);
        List<WorkPackageData> workPackageEntryList = new List<WorkPackageData>();
        foreach (QueueWorkPackageContainer workPackage in workPackagesEntry)
        {
            if (workPackage.selected)
            {
                if (queueData.entryWorkPackages.Exists(x => x.id == workPackage.workPackageData.id))
                {
                    int editWorkPackageEntryData = queueData.entryWorkPackages.FindIndex(x => x.id == workPackage.workPackageData.id);
                    queueData.entryWorkPackages[editWorkPackageEntryData] = workPackage.workPackageData;
                }
                workPackageEntryList.Add(workPackage.workPackageData);
            }
        }

        QueueWorkPackageContainer[] workPackagesExit = queueWorkPackageContainerExitTransform.GetComponentsInChildren<QueueWorkPackageContainer>(true);
        List<WorkPackageData> workPackageExitList = new List<WorkPackageData>();
        foreach (QueueWorkPackageContainer workPackage in workPackagesExit)
        {
            if (workPackage.selected)
            {
                if (queueData.exitWorkPackages.Exists(x => x.id == workPackage.workPackageData.id))
                {
                    int editWorkPackageExitData = queueData.exitWorkPackages.FindIndex(x => x.id == workPackage.workPackageData.id);
                    queueData.exitWorkPackages[editWorkPackageExitData] = workPackage.workPackageData;
                }
                workPackageExitList.Add(workPackage.workPackageData);
            }
        }



        queueData.id = queue.id;
        queueData.queueName = queue.queueuName;
        queueData.entryWorkPackages = workPackageEntryList;
        queueData.exitWorkPackages = workPackageExitList;

        SaveToJson(queueData);
        queueCreateOverMenu.SetActive(false);

        ResetSelecteds();

      //queueNameInputField.text = "";
        entryWorkPackageInputField.text = "";
        exitWorkPackageInputField.text = "";
    }

    public void EditContainerBtn()
    {
        selectedContainer = null;
        QueueContainer[] queue = queueContainerTransform.GetComponentsInChildren<QueueContainer>(true);
        foreach (QueueContainer queueData in queue)
        {
            if (queueData.selected)
            {
                selectedContainer = queueData;
                break;
            }
        }
        if (selectedContainer == null)
        {
            Debug.Log("No Queue container selected");
            return;
        }
        selectedContainer.queueuMenu = this;
        queueCreateOverMenu.SetActive(true);
        createButton.SetActive(false);
        editButton.SetActive(true);
        queueNameInputField.text = selectedContainer.queueuName;

        Debug.Log("Selected Container ID: " + selectedContainer.id);

      
        int editQueueData = 0;
        for (int i = 0; i < SaveManager.queueList.queues.Count; i++)
        {
            print("test: " + SaveManager.queueList.queues[i].id + " test: " + selectedContainer.id);
            if (SaveManager.queueList.queues[i].id == selectedContainer.id)
            {
                editQueueData = i;
            }
        }

        Debug.Log("Found Index: " + editQueueData);

        QueueWorkPackageContainer[] workPackageEntry = queueWorkPackageContainerEntryTransform.GetComponentsInChildren<QueueWorkPackageContainer>(true);
        if (SaveManager.queueList.queues.Exists(u => u.id == selectedContainer.id))
        {
            foreach (QueueWorkPackageContainer workPackageEntryData in workPackageEntry)
            {
                workPackageEntryData.selected = false;
                workPackageEntryData.UpdateContainer();
                foreach (WorkPackageData workPackageData in SaveManager.queueList.queues[editQueueData].entryWorkPackages)
                    if (workPackageEntryData.id == workPackageData.id)
                    {
                        print("test entry worKPackage: " + workPackageEntryData.id);
                        workPackageEntryData.selected = true;
                        workPackageEntryData.UpdateContainer();
                    }

            }
        }
        QueueWorkPackageContainer[] workPackageExit= queueWorkPackageContainerExitTransform.GetComponentsInChildren<QueueWorkPackageContainer>(true);
        if (SaveManager.queueList.queues.Exists(u => u.id == selectedContainer.id))
        {
            foreach (QueueWorkPackageContainer workPackageExitData in workPackageExit)
            {
                workPackageExitData.selected = false;
                workPackageExitData.UpdateContainer();
                foreach (WorkPackageData workPackageData in SaveManager.queueList.queues[editQueueData].exitWorkPackages)
                    if (workPackageExitData.id == workPackageData.id)
                    {
                        print("test exit worKPackage: " + workPackageExitData.id);
                        workPackageExitData.selected = true;
                        workPackageExitData.UpdateContainer();
                    }

            }
        }

        ResetSelecteds();


    }
    public void CancelBtn()
    {
        queueCreateOverMenu.SetActive(false);
        queueNameInputField.text = "";
        selectedContainer = null;
        ResetSelecteds();

        entryWorkPackageInputField.text = "";
        exitWorkPackageInputField.text = "";
    }

    public void RemoveContainerBtn()
    {
        selectedContainer = null;
        QueueContainer[] queues = queueContainerTransform.GetComponentsInChildren<QueueContainer>(true);
        foreach (QueueContainer queueData in queues)
        {
            if (queueData.selected)
            {
                selectedContainer = queueData;
                SaveManager.RemoveQueueContainer(selectedContainer.id);
                Destroy(selectedContainer.gameObject);
                queueCreateOverMenu.SetActive(false);
                ResetSelecteds();

                break;
            }
        }
    }


    public void ResetSelecteds()
    {

        QueueContainer[] queue = queueContainerTransform.GetComponentsInChildren<QueueContainer>(true);
        foreach (QueueContainer queueData in queue)
        {
            queueData.selected = false;
            queueData.toggle.isOn = false;
        }
    }

    public void SaveToJson(QueuesData queueData)
    {
        SaveManager.SaveQueueData(queueData);
    }

    public string GetFreeID()
    {
        HashSet<int> usedIDs = new HashSet<int>();

        if (SaveManager.workPackageList != null)
        {
            foreach (QueuesData workPackage in SaveManager.queueList.queues)
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
}
