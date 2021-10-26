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
    public GameObject useP300ControlsToggle;

    public int recordKey = 0;

    public void Update()
    {
        if (recordKey == 1)
        {
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(vKey))
                {
                    if (vKey != KeyCode.Escape && vKey != KeyCode.Mouse0 && vKey != KeyCode.P)
                    {
                        // Set player prefs
                        PlayerPrefs.SetString("Player1Key", vKey.ToString());
                    }
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
                    if (vKey != KeyCode.Escape && vKey != KeyCode.Mouse0)
                    {
                        // Set player prefs
                        PlayerPrefs.SetString("Player2Key", vKey.ToString());
                    }
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
        print(PlayerPrefs.GetString("Player1Name"));
        print(PlayerPrefs.GetString("Player2Name"));
        p1NameInputField.GetComponent<InputField>().text = PlayerPrefs.GetString("Player1Name");
        p2NameInputField.GetComponent<InputField>().text = PlayerPrefs.GetString("Player2Name");
        p1KeyButtonText.GetComponent<Text>().text = PlayerPrefs.GetString("Player1Key");
        p2KeyButtonText.GetComponent<Text>().text = PlayerPrefs.GetString("Player2Key");
        useP300ControlsToggle.GetComponent<Toggle>().isOn = PlayerPrefs.GetInt("UseP300Controls") == 1;
    }

    public void ExitControlsPanel()
    {
        // check names
        p1Name = p1NameInputField.GetComponent<InputField>().text;
        p2Name = p2NameInputField.GetComponent<InputField>().text;
        print("p1Name: " + p1Name);
        print("p2Name: " + p2Name);
        if (p1Name == "") p1Name = "Player 1";
        if (p2Name == "") p2Name = "Player 2";
        print("p1Name: " + p1Name);
        print("p2Name: " + p2Name);

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
            PlayerPrefs.SetString("Player1Name", p1Name);
            PlayerPrefs.SetString("Player2Name", p2Name);
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

    public void ToggleP300Controls()
    {
        PlayerPrefs.SetInt("UseP300Controls", useP300ControlsToggle.GetComponent<Toggle>().isOn ? 1 : 0);
    }
}
