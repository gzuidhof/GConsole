using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

[AddComponentMenu("Scripts/GConsole")]
public class GConsole : MonoBehaviour
{
    public static GConsole instance;
    public bool outputUnityLog = true;
    public bool outputStackTrace = true;

    //NGUI color coding. Turn off if you are not using NGUI.
    public bool useColoredText = true; 

    //If a command returns nothing or you print an empty string, it will still send it to listeners (the UI), which will then have to deal with that.
    public bool allowEmptyOutput = false;

    public bool newlineAfterCommandOutput = false;

    private static Dictionary<string, GConsoleCommand> commands = new Dictionary<string, GConsoleCommand>();

    public delegate void GConsoleListener(string line);

    //Subscribe to this event for a console GUI (or anything that wants the console output)!
    public static event GConsoleListener OnOutput;

    #region Default Output

    private const string INVALID_COMMAND_STRING = Color("Invalid Command!", "FF0000"); 
    private const string COMMAND_NOT_FOUND_STRING = Color("Unrecognized command: ", "FF0000");

    private const string ERROR_STRING = Color( "Error: ", "EEAA00");
    private const string WARNING_STRING = Color("Warning: ", "CCAA00");
    private const string LOG_STRING = Color("Log: ", "AAAAAA");
    private const string EXCEPTION_STRING = Color("Exception: ","FF0000");
    private const string ASSERT_STRING = Color("Assert: ", "0000FF");

    #endregion

    #region Unity Callbacks

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        //Register the Log Callback
        if (outputUnityLog)
        { 
            Application.RegisterLogCallback(new Application.LogCallback(this.HandleUnityLog));
        }
        LoadBuiltInCommands();

    }

    private void HandleUnityLog(string logString, string trace, LogType logType)
    {
        string output = String.Empty;

        switch (logType)
        {
            case LogType.Error:
                output += ERROR_STRING;
                break;
            case LogType.Assert:
                output += ASSERT_STRING;
                break;
            case LogType.Warning:
                output += WARNING_STRING;
                break;
            case LogType.Log:
                output += LOG_STRING;
                break;
            case LogType.Exception:
                output += EXCEPTION_STRING;
                break;
            default:
                return;
        }
        output += logString + (instance.outputStackTrace ? "\n" + Color(trace, "AAAAAA"): String.Empty);
        Print(output);
    }

    #endregion

    #region Adding and Removing ConsoleCommands

    public bool RemoveCommand(string command)
    {
        if (commands.ContainsKey(command))
        {
            commands.Remove(command);
            return true;
        }
        return false;
    }

    public static bool AddCommand(string command, string description, Func<string, string> method, string helpText = null)
    {
        //Check for invalid command strings.
        if (string.IsNullOrEmpty(command))
        {
            Debug.LogError("Could not add ConsoleCommand \"" + command + "\" because it is empty!");
            return false;
        }
        if (commands.ContainsKey(command))
        {
            Debug.LogError("Could not add ConsoleCommand \"" + command + "\" because it already exists.");
            return false;
        }

        //Add the command to the dictionary.
        GConsoleCommand consoleCommand = new GConsoleCommand(description, method, helpText);
        commands.Add(command, consoleCommand);

        SortCommands();

        return true;
    }

    #endregion

    #region Command Evaluation

    /// <summary>
    /// Evaluate given string (execute console commandline given)
    /// <returns>Direct output of the method that is called</returns>
    public static string Eval(string command)
    {
        string output = String.Empty;

        //Print what was entered, in a lightblueish color
        Print("> " + Color(command, "AADDFF"));


        if (string.IsNullOrEmpty(command)) {
            output = INVALID_COMMAND_STRING;
            return Print(output);
        }

        string[] spaceSeparatedCommand = command.Split(' ');

        //Actual "command"
        string root = spaceSeparatedCommand[0];

        if (!commands.ContainsKey(root))
        {
            output = COMMAND_NOT_FOUND_STRING + root;
            return Print(output);
        }

        string parameters = ExtractParameters(command, root);
        output = commands[root].method(parameters);

        if (instance.newlineAfterCommandOutput)
            output += "\n";

        return Print(output);

    }

    #endregion

    #region Utility Methods

    //Extract the parameters from a command (removing the first word and trimming the rest).
    private static string ExtractParameters(string command, string root)
    {
        string parameters = (command.Length > root.Length) ? command.Substring(root.Length + 1, command.Length - (root.Length + 1)) : string.Empty;
        return parameters.Trim();
    }

    /// <summary>
    /// Sort the commands alphabetically in the dictionary (for an ordered "help list")
    /// </summary>
    private static void SortCommands()
    {
        commands = commands.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
    }

    /// <summary>
    /// Get suggestions for a given (incomplete) input
    /// </summary>
    /// <returns>A list of suggestions</returns>
    public static List<string> GetSuggestions(string inputSoFar, bool includeDescription = false)
    {
        return commands.Keys
            .Where(c => c.StartsWith(inputSoFar)) //Only take those which start with the input so far
            .OrderBy(c => c.Length) //Order them by length
            .Select(c => c + (includeDescription ? " \t" + Color(commands[c].description, "CCCCCC") : String.Empty)) //Append description if requested

            .Select(c => Color(c.Substring(0,inputSoFar.Length), "00CCCC") + c.Substring(inputSoFar.Length-1)) //Color part typed so far

            .ToList(); //Convert to list
    }

    #endregion

    /// <summary>
    /// Puts tags around text, if using NGUI, for coloring the text with the provided color code (Example: "FF0000")
    /// </summary>
    public static string Color(string text, string colorCode) {

        if(instance.useColoredText) {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("[");
            sb.Append(colorCode);
            sb.Append("]");
            sb.Append(text);
            sb.Append("[-]");
            return sb.ToString();
        }
        else {
            return text;
        }
    }



    #region Printing and Output Sending to Listeners
    private static string SendOutputToListeners(string output)
    {
        if (OnOutput != null)
            OnOutput(output);
        return output;
    }

    public static string Print(string text)
    {
        if (text == null) return String.Empty;

        //If option is not to allow empty output, don't send it to the listeners (bail out here).
        if (GConsole.instance.allowEmptyOutput && text == string.Empty)
            return String.Empty;

        SendOutputToListeners(text);
        return text;

    }

    //Added to hide MonoBehaviour.print
    public static string print(string text)
    {
        return Print(text);
    }

    #endregion

    #region Built In Console Commands

    private void LoadBuiltInCommands()
    {
        GConsole.AddCommand("help", "Show help on how to use the console", HelpConsoleCommand);
    }

    private static string HelpConsoleCommand(string param)
    {
        if (string.IsNullOrEmpty(param))
        {
            return "Type " + Color("help list [description]","33EE33") + " for a list of possible commands, or " + Color("help <command>","33EE33") + " for a description of a certain command.";
        }
        if (param == "list description" || param == "list [description]")
        {
            return GetHelpList(true);
        }
        else if (param == "list")
        {
            return GetHelpList(false);
        }
        else if (commands.ContainsKey(param))
        {
            GConsoleCommand command = commands[param];
            return Color(param, "33EE33") + " "
                + command.description + "\n" 
                + (command.helpText == null ? String.Empty : (Color(command.helpText,"DDDDDD")));
        }
        else
        {
            return "Help not found for [33EE33]" + param + "[-]";
        }
    }

    private static string GetHelpList(bool includeDescription)
    {
        if (!includeDescription)
            return string.Join("\n", commands.Keys.ToArray());
        else
        {
            string result = String.Empty;

            foreach (string command in commands.Keys)
            {
                result += command + " \t" + Color(commands[command].description, "AAAAAA") + "\n";
            }

            return result;
        }
    }

    #endregion
}
