using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField username;
    public GameObject loginScene;
    public GameObject nextScene;
    public void SubmitBtn()
    {

        foreach (UserData users in SaveManager.userList.users)
        {
            if (users.userName == username.text)
            {
                loginScene.SetActive(false);
                nextScene.SetActive(true);
                return;
            }
        }


        if (SaveManager.SaveUser(GetFreeID(), username.text))
        {
            loginScene.SetActive(false);
            nextScene.SetActive(true);
        }
    }
    public string GetFreeID()
    {
        HashSet<int> usedIDs = new HashSet<int>();

        // Adiciona todos os IDs já usados à lista de IDs usados
        foreach (UserData users in SaveManager.userList.users)
        {
            if (int.TryParse(users.id, out int id))
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
}
