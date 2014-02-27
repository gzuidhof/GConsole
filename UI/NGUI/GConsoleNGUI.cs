using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GConsoleNGUI : MonoBehaviour {

    public UITextList textList;
    public UIInput input;
    public GConsoleNGUISuggestion[] suggestions;

    public bool clearOnSubmit = false;
    public bool reselectOnSubmit = true;

    public int minCharBeforeSuggestions;

	void Start () {
        //Register the "OnOutput" method as a listener for console output.
	    GConsole.OnOutput += OnOutput;

        //Fire the OnChange method whenever input changes (so we can then update the suggestions).
        EventDelegate.Add(input.onChange, OnChange);
	}

    void OnOutput(string line)
    {
        textList.Add(line);
    }

    public void OnInput()
    {
        string cmd = input.value;

        if (string.IsNullOrEmpty(cmd))
            return;


        //Send command to the console
        GConsole.Eval(cmd);

        if (clearOnSubmit)
            input.value = string.Empty;

        if (reselectOnSubmit) //Has to be done in next frame for NGUI reasons (quirk in NGUI)..
            StartCoroutine(SelectInputNextFrame());
    }

    IEnumerator SelectInputNextFrame()
    {
        yield return null;
        input.isSelected = true;

    }

    IEnumerator ClearInputNextFrame()
    {
        yield return null;
        input.value = "";

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }


    void OnEnable()
    {
        //Setting the input as selected when the console is opened.
        input.isSelected = true;
        StartCoroutine(ClearInputNextFrame()); //Necessary because otherwise the input field will contain the letter used to open the UI.
        //Another NGUI quirk!
    }


    void OnChange()
    {
        LoadSuggestions();
    }

    private void LoadSuggestions()
    {
        List<String> sugstrings;

        //Not enough characters typed yet, no suggestions to be shown!
        if (minCharBeforeSuggestions != 0 && input.value.Length < minCharBeforeSuggestions)
        {
            sugstrings = new List<String>();
        }
        else
        {
            //Ask GConsole for suggestions, true because we want to have the description too.
            sugstrings = GConsole.GetSuggestions(input.value, true);
        }

        //Display suggestions (and hide unused suggestion boxes by passing null).
        for (int i = 0; i < suggestions.Length; i++)
        {
            if (i < sugstrings.Count)
                suggestions[i].ShowSuggestion(sugstrings[i]);
            else
                suggestions[i].ShowSuggestion(null);
        }
        
    }
    
    public void OnSuggestionClicked()
    {
        //Ugly solution of setting input to the button (suggestion) that was just clicked.
        input.value =
          NGUIText.StripSymbols(
          UIButton.current.GetComponent<GConsoleNGUISuggestion>().label.text.Split(' ')[0]
          + " ");

        //Reselect the input the next frame (quirk in NGUI, can't do it the current).
      StartCoroutine(SelectInputNextFrame());
    }
}
