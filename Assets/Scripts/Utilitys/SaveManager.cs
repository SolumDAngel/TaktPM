using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;



[System.Serializable]
public class UserData
{
    public string userName;
}
[System.Serializable]
public class UserList
{
    public List<UserData> users = new List<UserData>();
}
[System.Serializable]
public class AssetsList
{
    public List<AssetsData> assets = new List<AssetsData>();
}

[System.Serializable]
public class AssetsData
{
    public string id;
    public string name;
    public string description;
}




public static class SaveManager
{
    private static string saveUsersFilePath = Path.Combine(Application.dataPath, "userdata.json");
    private static string saveAssetsFilePath = Path.Combine(Application.dataPath, "assetsData.json");
    public static UserList userList;
    public static AssetsList appAssetsData;

    static SaveManager()
    {

        // Carrega a lista de usuários existentes (se houver)
        userList = LoadUsersFromJson();
        if (userList == null)
        {
            userList = new UserList(); // Inicializa uma nova lista se nenhuma existir
        }

        appAssetsData = LoadAssetsFromJson();
        if (appAssetsData == null)
        {
            appAssetsData = new AssetsList();
        }

    }





    // Método para salvar um novo usuário na lista e gravar no arquivo JSON
    public static bool SaveUser(string userName)
    {
        if (userName == "")
            return false;
        // Verifica se o usuário já existe
        if (!userList.users.Exists(u => u.userName == userName))
        {
            UserData newUser = new UserData();
            newUser.userName = userName;

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

        if (appAssetsData.assets.Exists(u => u.id == idAsset))
        {
            Debug.LogWarning("AssetData já existe modificando: " + idAsset + " : " + nameAsset + " : " + descriptionAsset);

            int editAssetData = appAssetsData.assets.FindIndex(assets => assets.id == idAsset);
            appAssetsData.assets[editAssetData].id = idAsset;
            appAssetsData.assets[editAssetData].name = nameAsset;
            appAssetsData.assets[editAssetData].description = descriptionAsset;           

            string json = JsonUtility.ToJson(appAssetsData, true);
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

            appAssetsData.assets.Add(newAssetData);

            string json = JsonUtility.ToJson(appAssetsData, true);
            File.WriteAllText(saveAssetsFilePath, json);
            Debug.Log("AssetData salvo em: " + saveAssetsFilePath);
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




    public static void RemoveAsset(string idAsset)
    {
        // Verifica se existe um asset com o ID especificado
        AssetsData assetToRemove = appAssetsData.assets.Find(asset => asset.id == idAsset);

        if (assetToRemove == null)
        {
            Debug.LogWarning("Nenhum Asset encontrado para remover com ID: " + idAsset);
            return;
        }

        // Remove o asset da lista
        appAssetsData.assets.Remove(assetToRemove);

        // Salva a lista atualizada no arquivo
        string json = JsonUtility.ToJson(appAssetsData, true);
        File.WriteAllText(saveAssetsFilePath, json);
        Debug.Log("AssetData removido e salvo em: " + saveAssetsFilePath);
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
