GConsole
========

GConsole is an ingame developer console for the Unity3D Engine. 

Here are some screenshots:

* [Some example usage from a discontinued project of mine](http://i.imgur.com/E5ivVNx.png)
* [Unity Log Callbacks](http://i.imgur.com/g4DqIAV.png)
* [Suggestions feature (GIF)](https://dl.dropboxusercontent.com/u/43693599/sug.gif)

##Why?
A proper console solution didn't exist (in my eyes) on the asset store, and I can't live without one. Having a developer console ingame is great for debug purposes, and for your final product too. 

See, for instance, Valve's game developer console, which prints information that is otherwise not visible to the user and allows the user to send "advanced" commands, for instance changing a setting.

##Features
* Separated internals from externals (UI)
* Easy to use
* Drag 'n' drop "installation"
* Suggestions feature
* Hooks into Unity logger (if you want it to), including stacktraces.
* Customizable
* Free and open source! 

##UI
GConsole has been developed with no particular GUI system in mind, in fact, it was designed with Unity's upcoming GUI (which will be released soon™.

###*Available UI Front-ends:*

* [uGUI](https://github.com/MuNgLo/GConsole-uGUI/) by MuNgLo (.unitypackage available [here](https://github.com/MuNgLo/GConsole-uGUI/releases/tag/0.1.0)
* [NGUI](https://github.com/Rahazan/GConsoleNGUI) (.unitypackage available [here](https://github.com/Rahazan/GConsoleNGUI/releases))
* [OpenGUI](https://github.com/MuNgLo/GConsoleOGUI) by MuNgLo (.unitypackage available [here](https://github.com/MuNgLo/GConsoleOGUI/releases))

##Usage
###Setup

* Download this repository, put the GConsole folder somewhere in your project.
* Pop a GConsole script on any (enabled) GameObject in your scene (or drop in the provided prefab).

* Download a frontend.
* See frontend page for further steps.

###Printing to the console.
```csharp
GConsole.Print("Hello world");
```
###Adding a command
```csharp
GConsole.AddCommand("quit", "Quits the application.", QuitApplication);
```
* The first argument is the command name
* The second argument is a description
* **The third argument is a method which returns a string and takes one parameter: a string. It doesn't matter whether it's private, internal, protected or public. What this method returns is what is printed to the console.**
* (OPTIONAL) Fourth argument: string, additional help text (shown when the user types "help < commandname > ".


**Full Example**
```csharp
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
``` 
###Dynamically calling a command
If for some reason you want to call a command dynamically (from code, perhaps from some sort of script file), you can do that.
```csharp
GUIConsole.Eval(cmd);
```
Where cmd is a string, just like the user would have typed it into the console. 

This returns the output of this command (useful if you want people to be able to use console commands from say, a chat window, where if they prepend a "/" it's evaluated by the console, and then you print what this returns in that window).

##Contribution Guidelines
**Any contributions are welcome, including feature requests.**

Two notes

* Especially welcome are front-ends for different UI frameworks (such as EZ GUI).
* The project could use a logo. 

##Contributors
[Rahazan](https://github.com/Rahazan) (Guido Zuidhof)
[TarasOsiris](https://github.com/TarasOsiris) (Taras Leskiv)
[MuNgLo](https://github.com/MuNgLo) 

##License

MIT license
