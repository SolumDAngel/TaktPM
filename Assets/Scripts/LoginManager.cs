using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField username;
    public GameObject loginScene;
    public GameObject nextScene;
    public void SubmitBtn()
    {
        if (SaveManager.SaveUser(username.text))
        {
            loginScene.SetActive(false);
            nextScene.SetActive(true);
        }
    }
}
