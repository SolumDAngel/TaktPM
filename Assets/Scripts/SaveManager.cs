using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public string userName;
}

[System.Serializable] // Permite a serialização de listas
public class UserList
{
    public List<UserData> users = new List<UserData>();
}

public static class SaveManager
{
    private static string saveFilePath;
    private static UserList userList;

    static SaveManager()
    {
        // Define o caminho onde o arquivo JSON será salvo
        saveFilePath = Path.Combine(Application.dataPath, "userdata.json");

        // Carrega a lista de usuários existentes (se houver)
        userList = LoadFromJson();
        if (userList == null)
        {
            userList = new UserList(); // Inicializa uma nova lista se nenhuma existir
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
            File.WriteAllText(saveFilePath, json);

            Debug.Log("Usuário salvo em: " + saveFilePath);
        }
        else
        {
            Debug.LogWarning("Usuário já existe: " + userName);
        }
        return true;
    }

    // Método para carregar a lista de usuários do JSON
    public static UserList LoadFromJson()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            return JsonUtility.FromJson<UserList>(json);
        }
        else
        {
            Debug.LogWarning("Arquivo de dados não encontrado!");
            return null;
        }
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
