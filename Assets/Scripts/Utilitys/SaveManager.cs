using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public string id;
    public string userName;
}

[System.Serializable]
public class UserList
{
    public List<UserData> users = new List<UserData>();
}

[System.Serializable]
public class AssetsData
{
    public string id;
    public string name;
    public string description;
}

[System.Serializable]
public class AssetsList
{
    public List<AssetsData> assets = new List<AssetsData>();
}

[System.Serializable]
public class WorkPackageData
{
    public string id;
    public string workPackageName;
    public List<AssetsData> assetsEntry = new List<AssetsData>();
    public List<int> assetsEntryQuantity = new List<int>();
    public List<AssetsData> assetsExit = new List<AssetsData>();
    public List<int> assetsExitQuantity = new List<int>();
    public List<UserData> persons = new List<UserData>();
}

[System.Serializable]
public class WorkPackageList
{
    public List<WorkPackageData> workPackages = new List<WorkPackageData>();
}

[System.Serializable]
public class QueuesData
{
    public string id;
    public string queueName;
    public List<WorkPackageData> entryWorkPackages = new List<WorkPackageData>();
    public List<WorkPackageData> exitWorkPackages = new List<WorkPackageData>();
}

[System.Serializable]
public class QueuesList
{
    public List<QueuesData> queues = new List<QueuesData>();
}

public static class SaveManager
{
    private static string saveUsersFilePath = Path.Combine(Application.dataPath, "userdata.json");
    private static string saveAssetsFilePath = Path.Combine(Application.dataPath, "assetsData.json");
    private static string saveWorkPackagesFilePath = Path.Combine(Application.dataPath, "workPackagesData.json");
    private static string saveQueuesFilePath = Path.Combine(Application.dataPath, "queuesData.json");

    public static UserList userList;
    public static AssetsList appAssetsList;
    public static WorkPackageList workPackageList;
    public static QueuesList queueList;

    static SaveManager()
    {
        userList = LoadUsersFromJson() ?? new UserList();
        appAssetsList = LoadAssetsFromJson() ?? new AssetsList();
        workPackageList = LoadWorkPackageFromJson() ?? new WorkPackageList();
        queueList = LoadQueuesFromJson() ?? new QueuesList();
    }

    public static bool SaveUser(string id, string userName)
    {
        if (userName == "")
            return false;

        if (!userList.users.Exists(u => u.userName == userName))
        {
            UserData newUser = new UserData { id = id, userName = userName };
            userList.users.Add(newUser);

            string json = JsonUtility.ToJson(userList, true);
            File.WriteAllText(saveUsersFilePath, json);
            Debug.Log("User Saved in: " + saveUsersFilePath);
        }
        else
        {
            Debug.LogWarning("User already exists: " + userName);
        }
        return true;
    }

    public static bool SaveAssetData(string idAsset, string nameAsset, string descriptionAsset)
    {
        if (idAsset == "")
        {
            Debug.LogWarning("No asset ID");
            return false;
        }

        if (appAssetsList.assets.Exists(u => u.id == idAsset))
        {
            int editAssetData = appAssetsList.assets.FindIndex(assets => assets.id == idAsset);
            appAssetsList.assets[editAssetData].id = idAsset;
            appAssetsList.assets[editAssetData].name = nameAsset;
            appAssetsList.assets[editAssetData].description = descriptionAsset;

            string json = JsonUtility.ToJson(appAssetsList, true);
            File.WriteAllText(saveAssetsFilePath, json);
            Debug.Log("AssetData saved in: " + saveAssetsFilePath);
        }
        else
        {
            AssetsData newAssetData = new AssetsData { id = idAsset, name = nameAsset, description = descriptionAsset };
            appAssetsList.assets.Add(newAssetData);

            string json = JsonUtility.ToJson(appAssetsList, true);
            File.WriteAllText(saveAssetsFilePath, json);
            Debug.Log("AssetData saved in: " + saveAssetsFilePath);
        }

        return true;
    }

    public static bool SaveWorkPackageData(WorkPackageData workPackageData)
    {
        if (workPackageData.id == "")
        {
            Debug.LogWarning("No WorkPackage ID");
            return false;
        }

        if (workPackageList.workPackages.Exists(u => u.id == workPackageData.id))
        {
            int editWorkPackageData = workPackageList.workPackages.FindIndex(workPackage => workPackage.id == workPackageData.id);
            workPackageList.workPackages[editWorkPackageData].id = workPackageData.id;
            workPackageList.workPackages[editWorkPackageData].workPackageName = workPackageData.workPackageName;
            workPackageList.workPackages[editWorkPackageData].assetsEntry = workPackageData.assetsEntry;
            workPackageList.workPackages[editWorkPackageData].assetsEntryQuantity = workPackageData.assetsEntryQuantity;
            workPackageList.workPackages[editWorkPackageData].assetsExit = workPackageData.assetsExit;
            workPackageList.workPackages[editWorkPackageData].assetsExitQuantity = workPackageData.assetsExitQuantity;
            workPackageList.workPackages[editWorkPackageData].persons = workPackageData.persons;

            string json = JsonUtility.ToJson(workPackageList, true);
            File.WriteAllText(saveWorkPackagesFilePath, json);
            Debug.Log("WorkPackageData saved in: " + saveWorkPackagesFilePath);
        }
        else
        {
            WorkPackageData newWorkPackageData = new WorkPackageData
            {
                id = workPackageData.id,
                workPackageName = workPackageData.workPackageName,
                assetsEntry = workPackageData.assetsEntry,
                assetsEntryQuantity = workPackageData.assetsEntryQuantity,
                assetsExit = workPackageData.assetsExit,
                assetsExitQuantity = workPackageData.assetsExitQuantity,
                persons = workPackageData.persons
            };
            workPackageList.workPackages.Add(newWorkPackageData);

            string json = JsonUtility.ToJson(workPackageList, true);
            File.WriteAllText(saveWorkPackagesFilePath, json);
            Debug.Log("AssetData saved in: " + saveWorkPackagesFilePath);
        }

        return true;
    }

    public static bool SaveQueueData(QueuesData queueData)
    {
        if (queueData.id == "")
        {
            Debug.LogWarning("No queueu ID");
            return false;
        }

        if (queueList.queues.Exists(u => u.id == queueData.id))
        {
            int editQueueData = queueList.queues.FindIndex(queue => queue.id == queueData.id);
            queueList.queues[editQueueData].id = queueData.id;
            queueList.queues[editQueueData].queueName = queueData.queueName;
            queueList.queues[editQueueData].entryWorkPackages = queueData.entryWorkPackages;
            queueList.queues[editQueueData].exitWorkPackages = queueData.exitWorkPackages;

            string json = JsonUtility.ToJson(queueList, true);
            File.WriteAllText(saveQueuesFilePath, json);
            Debug.Log("QueueData saved in: " + saveQueuesFilePath);
        }
        else
        {
            QueuesData newQueueData = new QueuesData
            {
                id = queueData.id,
                queueName = queueData.queueName,
                entryWorkPackages = queueData.entryWorkPackages,
                exitWorkPackages = queueData.exitWorkPackages
            };
            queueList.queues.Add(newQueueData);

            string json = JsonUtility.ToJson(queueList, true);
            File.WriteAllText(saveQueuesFilePath, json);
            Debug.Log("Queueusdata saved in: " + saveQueuesFilePath);
        }

        return true;
    }

    public static UserList LoadUsersFromJson()
    {
        if (File.Exists(saveUsersFilePath))
        {
            string json = File.ReadAllText(saveUsersFilePath);
            return JsonUtility.FromJson<UserList>(json);
        }
        else
        {
            Debug.LogWarning("File for user data don't found!");
            return null;
        }
    }

    public static AssetsList LoadAssetsFromJson()
    {
        if (File.Exists(saveAssetsFilePath))
        {
            string json = File.ReadAllText(saveAssetsFilePath);
            return JsonUtility.FromJson<AssetsList>(json);
        }
        else
        {
            Debug.LogWarning("File for Asset data don't found!");
            return null;
        }
    }

    public static WorkPackageList LoadWorkPackageFromJson()
    {
        if (File.Exists(saveWorkPackagesFilePath))
        {
            string json = File.ReadAllText(saveWorkPackagesFilePath);
            return JsonUtility.FromJson<WorkPackageList>(json);
        }
        else
        {
            Debug.LogWarning("File for WorkPackage data don't found!");
            return null;
        }
    }

    public static QueuesList LoadQueuesFromJson()
    {
        if (File.Exists(saveQueuesFilePath))
        {
            string json = File.ReadAllText(saveQueuesFilePath);
            return JsonUtility.FromJson<QueuesList>(json);
        }
        else
        {
            Debug.LogWarning("File for Queue data don't found!");
            return null;
        }
    }

    public static void RemoveUser(string idPerson)
    {
        if (userList.users.Exists(u => u.id == idPerson))
        {
            userList.users.RemoveAll(user => user.id == idPerson);
            string json = JsonUtility.ToJson(userList, true);
            File.WriteAllText(saveUsersFilePath, json);
            Debug.Log("User data sucessful removed: " + idPerson);
        }
    }

    public static void RemoveAsset(string idAsset)
    {
        if (appAssetsList.assets.Exists(u => u.id == idAsset))
        {
            appAssetsList.assets.RemoveAll(asset => asset.id == idAsset);
            string json = JsonUtility.ToJson(appAssetsList, true);
            File.WriteAllText(saveAssetsFilePath, json);
            Debug.Log("Asset sucessful removed: " + idAsset);
        }
    }

    public static void RemoveWorkPackage(string idWorkPackage)
    {
        if (workPackageList.workPackages.Exists(u => u.id == idWorkPackage))
        {
            workPackageList.workPackages.RemoveAll(workPackage => workPackage.id == idWorkPackage);
            string json = JsonUtility.ToJson(workPackageList, true);
            File.WriteAllText(saveWorkPackagesFilePath, json);
            Debug.Log("WorkPackage sucessful removed: " + idWorkPackage);
        }
    }

    public static void RemoveQueue(string idQueue)
    {
        if (queueList.queues.Exists(u => u.id == idQueue))
        {
            queueList.queues.RemoveAll(queue => queue.id == idQueue);
            string json = JsonUtility.ToJson(queueList, true);
            File.WriteAllText(saveQueuesFilePath, json);
            Debug.Log("Queue sucessful removed: " + idQueue);
        }
    }
}
