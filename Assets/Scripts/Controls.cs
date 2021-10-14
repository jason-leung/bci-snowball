using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{
    public GameObject controlsPanel;

    public string p1Name;
    public string p2Name;
    public GameObject p1NameInputField;
    public GameObject p2NameInputField;
    public KeyCode p1Key;
    public KeyCode p2Key;
    public GameObject p1KeyButtonText;
    public GameObject p2KeyButtonText;
    public GameObject warningText;

    public int recordKey = 0;

    public void Update()
    {
        if (recordKey == 1)
        {
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(vKey))
                {
                    // Set player prefs
                    PlayerPrefs.SetString("Player1Key", vKey.ToString());
                    // Set button text
                    p1KeyButtonText.GetComponent<Text>().text = PlayerPrefs.GetString("Player1Key");
                    // reset state
                    recordKey = 0;
                }
            }
        }else if (recordKey == 2)
        {
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(vKey))
                {
                    // Set player prefs
                    PlayerPrefs.SetString("Player2Key", vKey.ToString());
                    // Set button text
                    p2KeyButtonText.GetComponent<Text>().text = PlayerPrefs.GetString("Player2Key");
                    // reset state
                    recordKey = 0;
                }
            }
        }
    }

    public void OpenControlsPanel()
    {
        controlsPanel.SetActive(true);
        warningText.SetActive(false);

        // Set player name and key
        p1NameInputField.GetComponent<Text>().text = PlayerPrefs.GetString("Player1Name");
        p2NameInputField.GetComponent<Text>().text = PlayerPrefs.GetString("Player2Name");
        p1KeyButtonText.GetComponent<Text>().text = PlayerPrefs.GetString("Player1Key");
        p2KeyButtonText.GetComponent<Text>().text = PlayerPrefs.GetString("Player2Key");

    }

    public void ExitControlsPanel()
    {
        // check names
        p1Name = p1NameInputField.GetComponent<Text>().text;
        p2Name = p2NameInputField.GetComponent<Text>().text;
        if (p1Name == "") p1Name = "Player 1";
        if (p2Name == "") p2Name = "Player 2";

        // check keys
        p1Key = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Player1Key"));
        p2Key = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Player2Key"));

        // make sure names and keys are different
        if ((p1Name == p2Name) || (p1Key == p2Key))
        {
            warningText.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetString("Player1Name", p1NameInputField.GetComponent<Text>().text);
            PlayerPrefs.SetString("Player2Name", p2NameInputField.GetComponent<Text>().text);
            warningText.SetActive(false);
            controlsPanel.SetActive(false);
        }
    }

    public void RecordKeyPress(int inputKey)
    {
        recordKey = inputKey;

        if (inputKey == 1)
        {
            p1KeyButtonText.GetComponent<Text>().text = "Recording...";
            p2KeyButtonText.GetComponent<Text>().text = PlayerPrefs.GetString("Player2Key");
        }
        else if (inputKey == 2)
        {
            p1KeyButtonText.GetComponent<Text>().text = PlayerPrefs.GetString("Player1Key"); 
            p2KeyButtonText.GetComponent<Text>().text = "Recording...";
        }
    }
}
