using UnityEngine;
using System.Collections;

/// <summary>
/// Class for demonstrating the unity logging features of GConsole, throws exceptions/logs stuff on keyinput.
/// </summary>
public class GConsoleUnityLoggingDemonstration : MonoBehaviour
{

    protected GameObject s;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) //Will log something to the console
            Debug.Log("Something you have logged");

        if (Input.GetKeyDown(KeyCode.F2)) //Will throw a null error
            s.tag = "Null Please!";

        if (Input.GetKeyDown(KeyCode.F3)) //Will log some error to the console.
            Debug.LogError("Here's an error!");

        if (Input.GetKeyDown(KeyCode.F4)) //Will log some warning to the console.
            Debug.LogWarning("And a warning!");
    }

}
