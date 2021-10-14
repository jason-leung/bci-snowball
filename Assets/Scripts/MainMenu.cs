using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject howtoplayPanel;

    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        if (!PlayerPrefs.HasKey("Player1Name")) PlayerPrefs.SetString("Player1Name", "Player1");
        if (!PlayerPrefs.HasKey("Player2Name")) PlayerPrefs.SetString("Player2Name", "Player2");
        if (!PlayerPrefs.HasKey("Player1Key")) PlayerPrefs.SetString("Player1Key", KeyCode.Alpha1.ToString());
        if (!PlayerPrefs.HasKey("Player2Key")) PlayerPrefs.SetString("Player2Key", KeyCode.Alpha2.ToString());
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void HowToPlay()
    {
        howtoplayPanel.SetActive(!howtoplayPanel.activeSelf);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
