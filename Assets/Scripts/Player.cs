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
    public GameObject largeSnowballPrefab;

    public List<GameObject> snowballObjects;
    public List<Rigidbody2D> snowballRigidBodies;
    public List<Collider2D> snowballColliders;
    public Vector3 snowballStartingPosition; 
    public float forceAngle;
    public float forceMagnitude;
    public float snowballForceMultiplier;
    public float snowballForceOffset;
    public float snowballForceAngleOffset = 0.2f;
    public int numActiveSnowballs = 0;

    // UI
    public GameObject gameOverText;
    public GameObject player1WinsText;
    public GameObject player2WinsText;

    // powerups
    public bool hasSizePowerup;
    public int numSnowballs;
    public GameObject sizePowerupIcon;
    public GameObject snowballCount;

    // audio
    public AudioSource chooseSound;
    public AudioSource shootSound;
    public AudioSource sizePowerupSound;
    public AudioSource snowballPowerupSound;
    public AudioSource snowballHitSound;
    public AudioSource heartSound;
    public AudioSource winSound;

    // Start is called before the first frame update
    void Start()
    {
        // game settings
        numHearts = 5;
        PlayerPrefs.SetInt("isGameOver", 0);
        playerKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(gameObject.name + "Key"));

        // powerups
        hasSizePowerup = false;
        numSnowballs = 1;

        // UI
        playerName.GetComponent<Text>().text = PlayerPrefs.GetString(gameObject.name + "Name");
        if (gameObject.name == "Player1") player1WinsText.GetComponent<Text>().text = PlayerPrefs.GetString(gameObject.name + "Name") + " Wins!";
        if (gameObject.name == "Player2") player2WinsText.GetComponent<Text>().text = PlayerPrefs.GetString(gameObject.name + "Name") + " Wins!";

        // animation
        animator.SetBool("isThrowing", false);
        animator.SetBool("isHurt", false);
        animator.SetBool("playerLost", false);

        // Create snowball
        snowballObjects = new List<GameObject>();
        snowballRigidBodies = new List<Rigidbody2D>();;
        snowballColliders = new List<Collider2D>();
        CreateSnowball();
    }

    void UpdateArrow()
    {
        if (arrowState == "rotate")
        {
            arrow.transform.localEulerAngles = new Vector3(0, 0, Mathf.PingPong((Time.time + arrowTimeOffset) * arrowRotateSpeed, 90) + arrowAngleOffset);
        }
        else if (arrowState == "scale")
        {
            arrow.transform.localScale = new Vector3(0.1f + Mathf.PingPong(Time.time * arrowScaleSpeed, 0.1f), arrow.transform.localScale.y, arrow.transform.localScale.z);
        }
    }

    void UpdateSnowball()
    {
        // Fix snowball at player's hand when not in shooting mode
        if (arrowState != "shoot" && PlayerPrefs.GetInt("isGameOver") == 0)
        {
            for (int i = 0; i < snowballRigidBodies.Count; i++)
            {
                if (snowballRigidBodies[i])snowballRigidBodies[i].transform.position = snowballStartingPosition;
            }
        }
    }

    void GetUserInput()
    {
        if (Input.GetKeyDown(playerKey) && PlayerPrefs.GetInt("isGameOver") == 0)
        {
            // Choose angle
            if (arrowState == "rotate")
            {
                forceAngle = (Mathf.PingPong((Time.time + arrowTimeOffset) * arrowRotateSpeed, 90) + arrowAngleOffset) / 180f * Mathf.PI;
                arrowState = "scale";
                chooseSound.Play();
            }
            // Choose scale
            else if (arrowState == "scale")
            {
                forceMagnitude = arrow.transform.localScale.x * snowballForceMultiplier - snowballForceOffset;
                arrowState = "shoot";
                Shoot();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update arrow
        UpdateArrow();

        // Update Snowball
        UpdateSnowball();

        // Get user input
        GetUserInput();

        // Game over
        if (PlayerPrefs.GetInt("isGameOver") == 1)
        {
            animator.SetBool("isThrowing", true);
            for (int i = 0; i < snowballRigidBodies.Count; i++)
            {
                if (snowballRigidBodies[i]) snowballRigidBodies[i].isKinematic = false;
            }
        }
    }

    public void CreateSnowball()
    {
        // Destroy all existing snowballs
        foreach(GameObject snowballObj in GameObject.FindGameObjectsWithTag("Snowball"))
        {
            if (snowballObj.name.Contains(snowballPrefab.name) || snowballObj.name.Contains(largeSnowballPrefab.name))
            {
                Destroy(snowballObj);
            }
        }
        snowballObjects.Clear();
        snowballRigidBodies.Clear();
        snowballColliders.Clear();

        // Instantiate snowballs based on powerups
        for (int i = 0; i < numSnowballs; i++)
        {
            // enable game object
            snowballObjects.Add(Instantiate(hasSizePowerup ? largeSnowballPrefab : snowballPrefab));
            snowballObjects[i].SetActive(true);
            snowballObjects[i].GetComponent<Snowball>().snowballID = i;

            // disable rigid body
            snowballRigidBodies.Add(snowballObjects[i].GetComponent<Rigidbody2D>());
            snowballRigidBodies[i].isKinematic = true;

            // disable collider
            snowballColliders.Add(snowballRigidBodies[i].GetComponent<Collider2D>());
            snowballColliders[i].enabled = false;
        }
    }

    public void Shoot()
    {
        animator.SetBool("isThrowing", true);
        shootSound.Play();
        for (int i = 0; i < snowballRigidBodies.Count; i++)
        {
            // keep track of number of active snowballs
            numActiveSnowballs = numActiveSnowballs + 1;

            // enable collision physics
            snowballRigidBodies[i].isKinematic = false; 
            if (snowballColliders[i]) snowballColliders[i].enabled = true; 
            
            // Set snowball velocity
            if (i == 0)
            {
                snowballRigidBodies[i].velocity = new Vector3(forceMagnitude * Mathf.Cos(forceAngle), forceMagnitude * Mathf.Sin(forceAngle), 0f);
            }
            else if (i == 1)
            {
                snowballRigidBodies[i].velocity = new Vector3(forceMagnitude * Mathf.Cos(forceAngle + snowballForceAngleOffset), forceMagnitude * Mathf.Sin(forceAngle + snowballForceAngleOffset), 0f);
            }
            else if (i == 2)
            {
                snowballRigidBodies[i].velocity = new Vector3(forceMagnitude * Mathf.Cos(forceAngle - snowballForceAngleOffset), forceMagnitude * Mathf.Sin(forceAngle - snowballForceAngleOffset), 0f);
            }
        }
    }

    public void DeductHeart()
    {
        // Reduce hearts
        numHearts = Math.Max(0, numHearts - 1);
        for (int i = 1; i <= 5; i++) hearts[i - 1].SetActive(i <= numHearts);

        // Player hurt
        if (numHearts > 0)
        {
            animator.SetBool("isHurt", true);
            ResetHurtAnimationCoroutine();
        }
        // Game over
        else if (numHearts == 0)
        {
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
            winSound.Play();
        }
    }

    public void AddHeart()
    {
        numHearts = Math.Min(5, numHearts + 1);
        for (int i = 1; i <= 5; i++) hearts[i - 1].SetActive(i <= numHearts);
        heartSound.Play();
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
        SceneManager.LoadScene("GameScene");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GetSizePowerUp()
    {
        hasSizePowerup = true;
        sizePowerupIcon.SetActive(true);
        sizePowerupSound.Play();
    }

    public void GetSnowballPowerUp()
    {
        numSnowballs = Math.Min(3, numSnowballs + 1);
        snowballCount.GetComponent<Text>().text = "x " + numSnowballs.ToString() + "/3";
        snowballPowerupSound.Play();
    }
}
