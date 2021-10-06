using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    // user input
    public KeyCode userKey;
    public int numHearts;
    public List<GameObject> hearts;

    // arrow
    public GameObject arrow;
    public string arrowState = "rotate";
    public float arrowRotateSpeed = 60f;
    public float arrowScaleSpeed = 0.1f;
    public float arrowAngleOffset;
    public float arrowTimeOffset;
    

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

    public GameObject gameOverText;


    // Start is called before the first frame update
    void Start()
    {
        CreateSnowball();
        numHearts = 5;
        PlayerPrefs.SetInt("isGameOver", 0);
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
        if (arrowState != "shoot")
        {
            if (snowball) snowball.transform.position = snowballStartingPosition;
        }

        // Get user input
        if (Input.GetKeyDown(userKey) && PlayerPrefs.GetInt("isGameOver") == 0)
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
            Debug.Log("Game over!");
            gameOverText.SetActive(true);
            PlayerPrefs.SetInt("isGameOver", 1);
        }
    }
}
