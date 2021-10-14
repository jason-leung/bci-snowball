using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // player
    public GameObject playerName;
    public GameObject playerWinText;
    public KeyCode playerKey;
    public int numHearts;
    public List<GameObject> hearts;
    public Animator animator;

    // arrow
    public GameObject arrow;
    public string arrowState = "rotate";
    public float arrowRotateSpeed = 60f;
    public float arrowScaleSpeed = 0.1f;
    public float arrowAngleOffset;
    public float arrowTimeOffset;
    
    // snowball
    public GameObject snowballPrefab;
    public GameObject snowballObject;
    public Rigidbody2D snowball;
    public Collider2D snowballCollider;
    public float snowballForceMultiplier;
    public float snowballForceOffset;
    public Vector3 snowballStartingPosition;
    public float forceAngle;
    public float forceMagnitude;
    public float forceDirection;

    // UI
    public GameObject gameOverText;
    public GameObject player1WinsText;
    public GameObject player2WinsText;


    // Start is called before the first frame update
    void Start()
    {
        CreateSnowball();
        numHearts = 5;
        PlayerPrefs.SetInt("isGameOver", 0);
        playerName.GetComponent<Text>().text = PlayerPrefs.GetString(gameObject.name + "Name");
        if (gameObject.name == "Player1")
        {
            player1WinsText.GetComponent<Text>().text = PlayerPrefs.GetString(gameObject.name + "Name") + " Wins!";
        }
        else
        {
            player2WinsText.GetComponent<Text>().text = PlayerPrefs.GetString(gameObject.name + "Name") + " Wins!";
        }
        playerKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(gameObject.name + "Key"));
        animator.SetBool("isThrowing", false);
        animator.SetBool("isHurt", false);
        animator.SetBool("playerLost", false);
    }

    // Update is called once per frame
    void Update()
    {
        // Update arrow
        if (arrowState == "rotate")
        {
            arrow.transform.localEulerAngles = new Vector3(0, 0, Mathf.PingPong((Time.time + arrowTimeOffset) * arrowRotateSpeed, 90) + arrowAngleOffset);
        }
        else if (arrowState == "scale")
        {
            arrow.transform.localScale = new Vector3(0.1f + Mathf.PingPong(Time.time * arrowScaleSpeed, 0.1f), arrow.transform.localScale.y, arrow.transform.localScale.z);
        }

        // Update Snowball
        if (arrowState != "shoot" && PlayerPrefs.GetInt("isGameOver") == 0)
        {
            if (snowball) snowball.transform.position = snowballStartingPosition;
        }

        // Get user input
        if (Input.GetKeyDown(playerKey) && PlayerPrefs.GetInt("isGameOver") == 0)
        {
            // Choose angle
            if (arrowState == "rotate")
            {
                forceAngle = (Mathf.PingPong((Time.time + arrowTimeOffset) * arrowRotateSpeed, 90) + arrowAngleOffset) / 180f * Mathf.PI;
                arrowState = "scale";
            }
            // Choose scale
            else if (arrowState == "scale")
            {
                forceMagnitude = arrow.transform.localScale.x * snowballForceMultiplier - snowballForceOffset;
                arrowState = "shoot";
                Shoot();
            }
        }

        // Game over
        if (PlayerPrefs.GetInt("isGameOver") == 1)
        {
            animator.SetBool("isThrowing", true);
            snowball.isKinematic = false;
        }
    }

    public void CreateSnowball()
    {
        // Destroy all existing snowballs
        foreach(GameObject snowballObj in GameObject.FindGameObjectsWithTag("Snowball"))
        {
            if(snowballObj.name.Contains(snowballPrefab.name))
            {
                Destroy(snowballObj);
            }
        }
        snowballObject = Instantiate(snowballPrefab);
        snowballObject.SetActive(true);
        snowball = snowballObject.GetComponent<Rigidbody2D>();
        snowballCollider = snowball.GetComponent<Collider2D>();
        snowballCollider.enabled = false;

        snowball.isKinematic = true;
    }

    public void Shoot()
    {
        animator.SetBool("isThrowing", true);
        snowball.isKinematic = false;
        if (snowballCollider) snowballCollider.enabled = true;
        Vector3 snowballForce = new Vector3(forceMagnitude * Mathf.Cos(forceAngle), forceMagnitude * Mathf.Sin(forceAngle), 0f);
        snowball.velocity = snowballForce;
    }

    public void DeductHeart()
    {
        numHearts = Math.Max(0, numHearts - 1);
        for (int i = 1; i <= 5; i++)
        {
            hearts[i-1].SetActive(i <= numHearts);
        }

        if (numHearts == 0)
        {
            // Game over
            gameOverText.SetActive(true);
            if (gameObject.name == "Player1")
            {
                player1WinsText.SetActive(false);
                player2WinsText.SetActive(true);
            }
            else
            {
                player1WinsText.SetActive(true);
                player2WinsText.SetActive(false);
            }
            animator.SetBool("playerLost", true);
            PlayerPrefs.SetInt("isGameOver", 1);
        }
    }

    public void ResetHurtAnimationCoroutine()
    {
        StartCoroutine(ResetHurtAnimation());
    }

    public IEnumerator ResetHurtAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isHurt", false);
    }

    public void PlayAgain()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
