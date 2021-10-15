using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    public List<GameObject> powerupList;
    public GameObject sizePU;
    public GameObject snowballPU;
    public GameObject heartPU;
    public float powerupTimer;

    // Start is called before the first frame update
    void Start()
    {
        powerupList = new List<GameObject>();
        powerupList.Add(sizePU);
        powerupList.Add(snowballPU);
        powerupList.Add(heartPU);
    }

    // Update is called once per frame
    void Update()
    {
        // move powerup object slightly
        transform.position = new Vector3(0f, Mathf.PingPong(Time.time * 0.3f, 0.2f) - 0.1f, 0f);

        // show each powerup object at specific intervals
        powerupTimer = Mathf.Repeat(Time.time, powerupList.Count * 15);
        if (powerupTimer > 5 && powerupTimer <= 15 && PlayerPrefs.GetInt("isGameOver") == 0)
        {
            sizePU.SetActive(true);
            snowballPU.SetActive(false);
            heartPU.SetActive(false);
        }
        else if (powerupTimer > 20 && powerupTimer <= 30 && PlayerPrefs.GetInt("isGameOver") == 0)
        {
            sizePU.SetActive(false);
            snowballPU.SetActive(true);
            heartPU.SetActive(false);
        }
        else if (powerupTimer > 35 && powerupTimer <= 45 && PlayerPrefs.GetInt("isGameOver") == 0)
        {
            sizePU.SetActive(false);
            snowballPU.SetActive(false);
            heartPU.SetActive(true);
        }
        else
        {
            sizePU.SetActive(false);
            snowballPU.SetActive(false);
            heartPU.SetActive(false);
        }
    }
}
