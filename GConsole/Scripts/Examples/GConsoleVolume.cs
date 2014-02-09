using UnityEngine;
using System.Collections;

/// <summary>
/// Adds the volume command to the console, which allows for setting the global AudioListener volume.
/// </summary>
public class GConsoleVolume : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GConsole.AddCommand("volume", "Changes the volume, value between 0 and 1", 
            ChangeVolume, 
            "Usage: volume [value between 0 and 1]\nExample \"volume 0.5\" puts volume at 50%");
	}

    string ChangeVolume(string param)
    {
        if (string.IsNullOrEmpty(param))
        {
            return "Current volume: " + AudioListener.volume;
        }

        float newVolume;
        if (float.TryParse(param, out newVolume))
        {
            AudioListener.volume = newVolume;
            return "The volume is now " + newVolume;
        }

        return "Didn't understand your input, remember to enter a value between 0 and 1 after volume!";
    }
}
