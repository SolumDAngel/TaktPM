using System.Collections;
using System.Collections.Generic;
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
    public WorkPackageContainer selectedContainer;

    public TMP_InputField queueNameInputField;
    public TMP_InputField entryWorkPackageInputField;
    public TMP_InputField exitWorkPackageInputField;


    public void CreateQueueBtn()
    {

    }

    public void CreateQueueContainerBtn()
    {
        queueCreateOverMenu.SetActive(true);
    }

}
