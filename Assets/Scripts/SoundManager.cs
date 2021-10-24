using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public GameObject sound;

    void Start()
    {
        if (!PlayerPrefs.HasKey("SoundOn")) PlayerPrefs.SetInt("SoundOn", 1);
        sound.SetActive(PlayerPrefs.GetInt("SoundOn") == 1);
    }
}
