using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEditor.VersionControl;
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

    // public List<QueuesData> queuesDatasEntry = new List<QueuesData>();
    // public List<QueuesData> queuesDatasExist = new List<QueuesData>();
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

        // Carrega a lista de usuários existentes (se houver)
        userList = LoadUsersFromJson();
        if (userList == null)
        {
            userList = new UserList(); // Inicializa uma nova lista se nenhuma existir
        }

        appAssetsList = LoadAssetsFromJson();
        if (appAssetsList == null)
        {
            appAssetsList = new AssetsList();
        }

        workPackageList = LoadWorkPackageFromJson();
        if(workPackageList == null)
        {
            workPackageList = new WorkPackageList();
        }
        queueList = LoadQueuesFromJson();
        if(queueList == null)
        {
            queueList = new QueuesList();
        }


    }





    // Método para salvar um novo usuário na lista e gravar no arquivo JSON
    public static bool SaveUser(string id, string userName)
    {
        if (userName == "")
            return false;
        // Verifica se o usuário já existe
        if (!userList.users.Exists(u => u.userName == userName))
        {
            UserData newUser = new UserData();
            newUser.userName = userName;
            newUser.id = id;

            // Adiciona o novo usuário à lista
            userList.users.Add(newUser);

            // Converte os dados da lista de usuários para JSON
            string json = JsonUtility.ToJson(userList, true);

            // Salva o JSON no arquivo
            File.WriteAllText(saveUsersFilePath, json);

            Debug.Log("Usuário salvo em: " + saveUsersFilePath);
        }
        else
        {
            Debug.LogWarning("Usuário já existe: " + userName);
        }
        return true;
    }

    // Método para salvar um novo usuário na lista e gravar no arquivo JSON
    public static bool SaveAssetData(string idAsset, string nameAsset, string descriptionAsset)
    {
        if (idAsset == "")
        {
            Debug.LogWarning("No asset ID");
            return false;
        }

        if (appAssetsList.assets.Exists(u => u.id == idAsset))
        {
            Debug.LogWarning("AssetData já existe modificando: " + idAsset + " : " + nameAsset + " : " + descriptionAsset);

            int editAssetData = appAssetsList.assets.FindIndex(assets => assets.id == idAsset);
            appAssetsList.assets[editAssetData].id = idAsset;
            appAssetsList.assets[editAssetData].name = nameAsset;
            appAssetsList.assets[editAssetData].description = descriptionAsset;           

            string json = JsonUtility.ToJson(appAssetsList, true);
            File.WriteAllText(saveAssetsFilePath, json);
            Debug.Log("AssetData salvo em: " + saveAssetsFilePath);
        }
        else
        {
            Debug.LogWarning("AssetData ainda não existe: ");

            AssetsData newAssetData = new AssetsData();
            newAssetData.id = idAsset;
            newAssetData.name = nameAsset;
            newAssetData.description = descriptionAsset;

            appAssetsList.assets.Add(newAssetData);

            string json = JsonUtility.ToJson(appAssetsList, true);
            File.WriteAllText(saveAssetsFilePath, json);
            Debug.Log("AssetData salvo em: " + saveAssetsFilePath);
        } 

        return true;
    }

    public static bool SaveWorkPackageData(WorkPackageData workPackageData)
    {
        if (workPackageData.id == "")
        {
            Debug.LogWarning("No workPackage ID");
            return false;
        }

        if (workPackageList.workPackages.Exists(u => u.id == workPackageData.id))
        {
            Debug.LogWarning("workPackage já existe modificando: " + workPackageData.id + " : " + workPackageData.workPackageName);

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
            Debug.Log("WorkPackageData salvo em: " + saveWorkPackagesFilePath);
        }
        else
        {
            Debug.LogWarning("WorkPackageData ainda não existe: ");

            WorkPackageData newWorkPackageData = new WorkPackageData();
            newWorkPackageData.id = workPackageData.id;
            newWorkPackageData.workPackageName = workPackageData.workPackageName;
            newWorkPackageData.assetsEntry = workPackageData.assetsEntry;
            newWorkPackageData.assetsEntryQuantity = workPackageData.assetsEntryQuantity;
            newWorkPackageData.assetsExit = workPackageData.assetsExit;
            newWorkPackageData.assetsExitQuantity = workPackageData.assetsExitQuantity;
            newWorkPackageData.persons = workPackageData.persons;


            workPackageList.workPackages.Add(newWorkPackageData);

            string json = JsonUtility.ToJson(workPackageList, true);
            File.WriteAllText(saveWorkPackagesFilePath, json);
            Debug.Log("AssetData salvo em: " + saveWorkPackagesFilePath);
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
            Debug.LogWarning("Queue já existe modificando: " + queueData.id + " : " + queueData.queueName);

            int editQueueData = queueList.queues.FindIndex(queue => queue.id == queueData.id);
            queueList.queues[editQueueData].id = queueData.id;
            queueList.queues[editQueueData].queueName = queueData.queueName;
            queueList.queues[editQueueData].entryWorkPackages = queueData.entryWorkPackages;
            queueList.queues[editQueueData].exitWorkPackages = queueData.exitWorkPackages;


            string json = JsonUtility.ToJson(queueList, true);
            File.WriteAllText(saveQueuesFilePath, json);
            Debug.Log("QueueData salvo em: " + saveQueuesFilePath);
        }
        else
        {
            Debug.LogWarning("QueueData ainda não existe: ");

            QueuesData newQueueData = new QueuesData();
            newQueueData.id = queueData.id;
            newQueueData.queueName = queueData.queueName;
            newQueueData.entryWorkPackages = queueData.entryWorkPackages;
            newQueueData.exitWorkPackages = queueData.exitWorkPackages;            
            queueList.queues.Add(newQueueData);

            string json = JsonUtility.ToJson(queueList, true);
            File.WriteAllText(saveQueuesFilePath, json);
            Debug.Log("Salvado Queueusdata salvo em: " + saveQueuesFilePath);
        }

        return true;
    }



    // Método para carregar a lista de usuários do JSON
    public static UserList LoadUsersFromJson()
    {
        if (File.Exists(saveUsersFilePath))
        {
            string json = File.ReadAllText(saveUsersFilePath);
            return JsonUtility.FromJson<UserList>(json);
        }
        else
        {
            Debug.LogWarning("Arquivo de dados de Usuarios não encontrado!");
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
            Debug.LogWarning("Arquivo de dados de Assets não encontrado");
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
            Debug.LogWarning("Arquivo de dados de WorkPackage não encontrado");
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
            Debug.LogWarning("Arquivo de dados de Queue não encontrado");
            return null;
        }
    }

    public static void RemoveUser(string idPerson)
    {       
        UserData assetToRemove = userList.users.Find(users => users.id == idPerson);

        if (assetToRemove == null)
        {
            Debug.LogWarning("Nenhum Person encontrado para remover com ID: " + idPerson);
            return;
        }
       
        userList.users.Remove(assetToRemove);

        // Salva a lista atualizada no arquivo
        string json = JsonUtility.ToJson(userList, true);
        File.WriteAllText(saveUsersFilePath, json);
        Debug.Log("UserData removido em: " + saveUsersFilePath);
    }


    public static void RemoveAsset(string idAsset)
    {
        // Verifica se existe um asset com o ID especificado
        AssetsData assetToRemove = appAssetsList.assets.Find(asset => asset.id == idAsset);

        if (assetToRemove == null)
        {
            Debug.LogWarning("Nenhum Asset encontrado para remover com ID: " + idAsset);
            return;
        }

        // Remove o asset da lista
        appAssetsList.assets.Remove(assetToRemove);

        // Salva a lista atualizada no arquivo
        string json = JsonUtility.ToJson(appAssetsList, true);
        File.WriteAllText(saveAssetsFilePath, json);
        Debug.Log("AssetData removido e salvo em: " + saveAssetsFilePath);
    }

    public static void RemoveWorkPackageContainer(string idWorkPackageContainer)
    {
        WorkPackageData workPackageToRemove = workPackageList.workPackages.Find(workpackage => workpackage.id == idWorkPackageContainer);

        if(workPackageToRemove == null)
        {
            Debug.LogWarning("Nenhum WorkPackage encontrado para remover com ID: " + idWorkPackageContainer);
            return;
        }

        workPackageList.workPackages.Remove(workPackageToRemove);

        string json = JsonUtility.ToJson(workPackageList, true);
        File.WriteAllText (saveWorkPackagesFilePath, json);
        Debug.Log("WorkPackage removido e salvo em: " + saveWorkPackagesFilePath);
    }

    public static void RemoveQueueContainer(string idQueueContainer)
    {
        QueuesData queueToRemove = queueList.queues.Find(queue => queue.id == idQueueContainer);

        if (queueToRemove == null)
        {
            Debug.LogWarning("Nenhum Queue encontrado para remover com ID: " + idQueueContainer);
            return;
        }

        queueList.queues.Remove(queueToRemove);

        string json = JsonUtility.ToJson(queueList, true);
        File.WriteAllText(saveQueuesFilePath, json);
        Debug.Log("Queue removido e salvo em: " + saveQueuesFilePath);
    }


    // Método para obter todos os nomes de usuários salvos
    public static List<string> GetUserNames()
    {
        List<string> userNames = new List<string>();
        foreach (var user in userList.users)
        {
            userNames.Add(user.userName);
        }
        return userNames;
    }
}
