using UnityEngine;
using System.Collections;


[AddComponentMenu("Scripts/GConsoleNGUISuggestion")]
public class GConsoleNGUISuggestion : MonoBehaviour
{
    public UILabel label;

    public void ShowSuggestion(string s)
    {
        if (string.IsNullOrEmpty(s))
            gameObject.SetActive(false);
        else
        {
            gameObject.SetActive(true);
            label.text = s;
        }
    }
}
