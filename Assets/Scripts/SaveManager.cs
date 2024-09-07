using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public string userName;
}

[System.Serializable] // Permite a serializa��o de listas
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
        // Define o caminho onde o arquivo JSON ser� salvo
        saveFilePath = Path.Combine(Application.dataPath, "userdata.json");

        // Carrega a lista de usu�rios existentes (se houver)
        userList = LoadFromJson();
        if (userList == null)
        {
            userList = new UserList(); // Inicializa uma nova lista se nenhuma existir
        }
    }

    // M�todo para salvar um novo usu�rio na lista e gravar no arquivo JSON
    public static bool SaveUser(string userName)
    {
        if (userName == "")
            return false;
        // Verifica se o usu�rio j� existe
        if (!userList.users.Exists(u => u.userName == userName))
        {
            UserData newUser = new UserData();
            newUser.userName = userName;

            // Adiciona o novo usu�rio � lista
            userList.users.Add(newUser);

            // Converte os dados da lista de usu�rios para JSON
            string json = JsonUtility.ToJson(userList, true);

            // Salva o JSON no arquivo
            File.WriteAllText(saveFilePath, json);

            Debug.Log("Usu�rio salvo em: " + saveFilePath);
        }
        else
        {
            Debug.LogWarning("Usu�rio j� existe: " + userName);
        }
        return true;
    }

    // M�todo para carregar a lista de usu�rios do JSON
    public static UserList LoadFromJson()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            return JsonUtility.FromJson<UserList>(json);
        }
        else
        {
            Debug.LogWarning("Arquivo de dados n�o encontrado!");
            return null;
        }
    }

    // M�todo para obter todos os nomes de usu�rios salvos
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
