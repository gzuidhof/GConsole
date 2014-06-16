using UnityEngine;
using System.Collections;
/// <summary>
/// Adds a ton of commands to the console to demonstrate the suggestion feature of gconsole.
/// </summary>
public class GConsoleSuggestionDemonstration : MonoBehaviour {

    private string[] phoneyCommands = { "dm_ping", 
                                          "dm_changelevel", 
                                          "dm_say",
                                          "dm_ip",
                                          "dm_ipserver",
                                          "dm_debugpeep",
                                      "dm_graphics",
                                      "dm_vsync"};                    

    private string[] phoneyDescriptions = { "Ping the server you are currently in.", 
                                              "Change the current level to level specified.", 
                                              "Say message in chat.", "Prints your local and online IP address.", 
                                              "Prints the IP address of the server you are connected to.", 
                                              "Plays a beep sound, for debug purposes",
                                          "Changes the graphics settings",
                                          "Turns VSync on and off"};

	void Start () {
        for (int i = 0; i < phoneyCommands.Length; i++) //Add phoney commands to GConsole.
        {
            GConsole.AddCommand(phoneyCommands[i], phoneyDescriptions[i], PhoneyCommand);
        }
	}

    string PhoneyCommand(string param)
    {
        return "This command doesn't actually do anything, it's just to demonstrate how the suggestion feature works!";
    }
}
