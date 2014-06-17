GConsole
========

GConsole is an ingame developer console for the Unity3D Engine. The UI frontend for NGUI is available [here](https://github.com/Rahazan/GConsoleNGUI)

Here are some screenshots:

* [Some example usage from a discontinued project of mine](http://i.imgur.com/E5ivVNx.png)
* [Unity Log Callbacks](http://i.imgur.com/g4DqIAV.png)
* [Suggestions feature (GIF)](https://dl.dropboxusercontent.com/u/43693599/sug.gif)

##Why?
A proper console solution didn't exist (in my eyes) on the asset store, and I can't live without one. Having a developer console ingame is great for debug purposes, and for your final product too. 
See, for instance, Valve's games' developer console, which prints information that is otherwise not visible to the user and allows the user to send "advanced" commands, for instance changing a setting.

##Features
* Separated internals from externals (UI)
* Easy to use
* Drag 'n' drop "installation"
* Suggestions feature
* Hooks into Unity logger (if you want it to), including stacktraces.
* Customizable
* Free and open source! 

##NGUI
GConsole has been developed with no particular GUI system in mind, in fact, it was designed with Unity's upcoming GUI (which will be released soon(tm)) in mind. 

I have kept the GUI code separated from the inner workings, and creating your own GUI should be a piece of cake looking at current implementations.

The NGUI frontend can be found here 

##Usage
###Setup

* Download this repository , put the GConsole folder somewhere in your project.
* Download a frontend, such as the [NGUI front-end](https://github.com/Rahazan/GConsoleNGUI).
* Pop a GConsole script on any (enabled) GameObject in your scene (or drop in the provided prefab). 

* **(If using NGUI frontend)** Put the GConsolePanel prefab in your NGUI hierarchy (somewhere under the camera). There is a script attached that enables/disables (shows and hides) the UI on button down (P by default).


###Printing to the console.
	GConsole.Print("Hello world");
###Adding a command
	GConsole.AddCommand("quit", "Quits the application.", QuitApplication);

* The first argument is the command name
* The second argument is a description
* **The third argument is a method which returns a string and takes one parameter: a string. It doesn't matter whether it's private, internal, protected or public. What this method returns is what is printed to the console.**
* (OPTIONAL) Fourth argument: string, additional help text (shown when the user types "help < commandname > ".


**Full Example**

	public class GConsoleQuit : MonoBehaviour {

	void Start () {
	    GConsole.AddCommand("quit", "Quits the application.", QuitApplication);
	}

    string QuitApplication(string param)
    {
        if (Application.isWebPlayer || Application.isEditor)
        {
            return "You can't quit in a webplayer build, just close this window!"; //Or in the editor, but lets not print that.
        }
        else
        {
            Application.Quit();
            return null; //No point in returning a message if the application has already shut down.
        }
    }
	}
    
###Dynamically calling a command
If for some reason you want to call a command dynamically (from code, perhaps from some sort of script file), you can do that.

	GUIConsole.Eval(cmd);
Where cmd is a string, just like the user would have typed it into the console. 

This returns the output of this command (useful if you want people to be able to use console commands from say, a chat window, where if they prepend a "/" it's evaluated by the console, and then you print what this returns in that window).

##Contribution Guidelines
**Any contributions are welcome, including feature requests.**

Two notes

* Especially welcome are UIs for different UI frameworks (such as OpenGUI and EZ GUI).
* The project could use a logo. 

##Contributors
[Rahazan](https://github.com/Rahazan) (Guido Zuidhof)
[TarasOsiris](https://github.com/TarasOsiris) (Taras Leskiv)

##License

MIT license
