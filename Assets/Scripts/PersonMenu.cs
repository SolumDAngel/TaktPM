using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PersonMenu : MonoBehaviour
{
    public GameObject personContainer;
    public Transform personContainerParent;

    public GameObject personCreateOverMenu;
    public GameObject selectedContainer;

    public TMP_InputField searchInputField;
    public TMP_InputField personNameInputField;

    private void Start()
    {
        foreach (UserData userData in SaveManager.userList.users)
        {
            GameObject obj = Instantiate(personContainer, personContainerParent);
            PersonContainer user = obj.GetComponent<PersonContainer>();

            user.id = userData.id;
            user.personName = userData.userName;
            user.UpdateContainer();
        }
    }


    public void SearchPerson(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            // Se o nome de busca está vazio ou apenas contém espaços, ativa todos os assets
            foreach (PersonContainer user in personContainerParent.GetComponentsInChildren<PersonContainer>(true))
            {
                user.gameObject.SetActive(true);
            }
            return;
        }

        // Converte o nome para minúsculas para uma comparação case insensitive
        string searchText = name.ToLower();

        // Obtém todos os componentes AssetContainer filhos do pai
        PersonContainer[] users = personContainerParent.GetComponentsInChildren<PersonContainer>(true);


        // Ativa apenas os assets que correspondem à busca e desativa os demais
        foreach (PersonContainer user in users)
        {
            user.gameObject.SetActive(user.personName.ToLower().Contains(name.ToLower()));
        }

    }

    public void CreateBtn()
    {
        if (personNameInputField.text == "")
        {
            print("Please enter a name for the new person");
            return;
        }

        PersonContainer[] users = personContainerParent.GetComponentsInChildren<PersonContainer>(true);

        foreach (PersonContainer usersData in users)
        {
            if (usersData.personName == personNameInputField.text)
            {
                SaveToJson(usersData.id);
                usersData.UpdateContainer();
                personCreateOverMenu.SetActive(false);
                return;
            }
        }

        GameObject obj = Instantiate(personContainer, personContainerParent);
        PersonContainer user = obj.GetComponent<PersonContainer>();

        user.id = GetFreeID();
        user.personName = personNameInputField.text;
        SaveToJson(user.id);
        user.UpdateContainer();
        personCreateOverMenu.SetActive(false);
        users = new PersonContainer[0];

        ResetSelected();
    }

    public void SaveToJson(string id)
    {
        SaveManager.SaveUser(id, personNameInputField.text);
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

    public void CreateContainerBtn()
    {
        personNameInputField.text = "";
        personCreateOverMenu.SetActive(true);

        ResetSelected();
    }
    public void CancelBtn()
    {
        personNameInputField.text = "";
        personCreateOverMenu.SetActive(false);

        ResetSelected();
    }
    public void RemoveBtn()
    {
        selectedContainer = null;
        PersonContainer[] users = personContainerParent.GetComponentsInChildren<PersonContainer>(true);
        PersonContainer user = null;
        foreach (PersonContainer userData in users)
        {
            if (userData.selected)
            {
                user = userData;
                selectedContainer = userData.gameObject;
                break;
            }
        }
        if (selectedContainer == null)
        {
            Debug.Log("No person container selected");
            return;
        }
        SaveManager.RemoveUser(user.id);
        Destroy(selectedContainer);
        users = new PersonContainer[0];
      
        ResetSelected();
    }

    public void ResetSelected()
    {
        selectedContainer = null;
        PersonContainer[] users = personContainerParent.GetComponentsInChildren<PersonContainer>(true);
        foreach (PersonContainer userData in users)
        {
            userData.selected = false;
        }
    }
}
