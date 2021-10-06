using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    public Player currentPlayer;
    public Player otherPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collisionObject)
    {
        if (collisionObject.gameObject.tag == "Snowball")
        {
            Physics2D.IgnoreCollision(collisionObject.collider, currentPlayer.snowballCollider);
        }
        else
        {
            // Manage hearts
            if (collisionObject.gameObject.tag == "Player" || ((collisionObject.gameObject.tag == "Snowball") && (otherPlayer.arrowState != "shoot")))
            {
                otherPlayer.DeductHeart();
            }

            // Destroy snowball
            gameObject.SetActive(false);
            Destroy(gameObject, 0.1f);
            currentPlayer.snowball = null;
            currentPlayer.snowballCollider = null;

            // Reset state
            currentPlayer.arrowState = "rotate";
            currentPlayer.CreateSnowball();
        }
    }
}
