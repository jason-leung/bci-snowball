using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject howtoplayPanel;
    public GameObject nextPageButton;
    public GameObject lastPageButton;
    public GameObject pageNumber;
    public List<GameObject> pages;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteAll();
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
        pageNumber.GetComponent<Text>().text = "Page 1";
        pages[0].SetActive(true);
        pages[1].SetActive(false);
        pages[2].SetActive(false);
        lastPageButton.SetActive(false);
        nextPageButton.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void HowToPlayNextPage()
    {
        if (pageNumber.GetComponent<Text>().text == "Page 1")
        {
            pageNumber.GetComponent<Text>().text = "Page 2";
            pages[0].SetActive(false);
            pages[1].SetActive(true);
            lastPageButton.SetActive(true);
        }
        else if (pageNumber.GetComponent<Text>().text == "Page 2")
        {
            pageNumber.GetComponent<Text>().text = "Page 3";
            pages[1].SetActive(false);
            pages[2].SetActive(true);
            nextPageButton.SetActive(false);
        }
    }

    public void HowToPlayLastPage()
    {
        if (pageNumber.GetComponent<Text>().text == "Page 2")
        {
            pageNumber.GetComponent<Text>().text = "Page 1";
            pages[1].SetActive(false);
            pages[0].SetActive(true);
            lastPageButton.SetActive(false);
        }
        else if (pageNumber.GetComponent<Text>().text == "Page 3")
        {
            pageNumber.GetComponent<Text>().text = "Page 2";
            pages[2].SetActive(false);
            pages[1].SetActive(true);
            nextPageButton.SetActive(true);
        }
    }
}
