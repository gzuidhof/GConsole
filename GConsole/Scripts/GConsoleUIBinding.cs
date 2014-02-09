using UnityEngine;
using System.Collections;

/// <summary>
/// Simple script that enables and disables the gameobject specified on key (if no input has focus).
/// </summary>
public class GConsoleUIBinding : MonoBehaviour
{

    public KeyCode consoleOpenKey = KeyCode.P;
    public GameObject consoleGameObject;

    void Update()
    {
        if (Input.GetKeyDown(consoleOpenKey) && !UICamera.inputHasFocus)
            if (consoleGameObject)
                consoleGameObject.SetActive(!consoleGameObject.activeSelf);

    }

}
