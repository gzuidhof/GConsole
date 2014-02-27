using UnityEngine;
using System.Collections;

/// <summary>
/// Adds the quit command to the console, which closes the application (if in non-web non-editor build).
/// </summary>
public class GConsoleQuit : MonoBehaviour {

	void Start () {
	    GConsole.AddCommand("quit", "Quits the application.", QuitApplication);
	}

    string QuitApplication(string param)
    {
        if (Application.isWebPlayer || Application.isEditor)
        {
            return "You can't quit in a webplayer build, just close this window!"; //Or in the editor, but let's not print that.
        }
        else
        {
            Application.Quit();
            return null; //No point in returning a message if the application has already shut down.
        }
    }
}
