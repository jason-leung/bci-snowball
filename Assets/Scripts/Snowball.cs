using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    public Player currentPlayer;
    public Player otherPlayer;
    public int snowballID = -1;

    void Update()
    {
        // Ignore some collisions
        for (int i = 0; i < currentPlayer.snowballColliders.Count; i++)
        {
            // Ignore collisions between snowballs for current player
            for (int j = 0; j < currentPlayer.snowballColliders.Count; j++)
            {
                if (currentPlayer.snowballColliders[i] && currentPlayer.snowballColliders[j])
                    Physics2D.IgnoreCollision(currentPlayer.snowballColliders[i], currentPlayer.snowballColliders[j]);
            }

            // ignore collisions between snowballs for other player
            for (int j = 0; j < otherPlayer.snowballColliders.Count; j++)
            {
                if (currentPlayer.snowballColliders[i] && otherPlayer.snowballColliders[j])
                    Physics2D.IgnoreCollision(currentPlayer.snowballColliders[i], otherPlayer.snowballColliders[j]);
            }

            // ignore collisions between snowball and current player
            if (currentPlayer.snowballColliders[i])
                Physics2D.IgnoreCollision(currentPlayer.snowballColliders[i], currentPlayer.GetComponent<CapsuleCollider2D>());
        }
    }

    void OnCollisionEnter2D(Collision2D collisionObject)
    {
        // Deduct hearts when colliding with player
        if (collisionObject.gameObject.tag == "Player")
        {
            otherPlayer.DeductHeart();
        }

        // Destroy snowball
        currentPlayer.snowballObjects[snowballID].SetActive(false);
        Destroy(currentPlayer.snowballObjects[snowballID], 0.1f);
        currentPlayer.snowballRigidBodies[snowballID] = null;
        currentPlayer.snowballColliders[snowballID] = null;
        currentPlayer.numActiveSnowballs -= 1;

        // Reset state
        if (currentPlayer.numActiveSnowballs <= 0)
        {
            currentPlayer.arrowState = "rotate";
            currentPlayer.animator.SetBool("isThrowing", false);
            currentPlayer.CreateSnowball();
        }
    }

    void OnTriggerEnter2D(Collider2D collisionObject)
    {
        if (collisionObject.gameObject.name == "SizePowerUp")
        {
            currentPlayer.GetSizePowerUp();
        }
        else if (collisionObject.gameObject.name == "SnowballPowerUp")
        {
            currentPlayer.GetSnowballPowerUp();
        }
        else if (collisionObject.gameObject.name == "HeartPowerUp")
        {
            currentPlayer.AddHeart();
        }
    }
}
