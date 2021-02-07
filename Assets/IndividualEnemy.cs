using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualEnemy : MonoBehaviour
{
    public float xLimit;
    public GameObject enemyManagerObject;
    EnemyManager enemyManager;
    public int pointsValue;
    public float zLimit = -8.0f;


    // Awake is called when the gameobject is created
    void Awake()
    {
        GameObject enemyManagerObject = GameObject.Find("EnemyManager");
        enemyManager = enemyManagerObject.GetComponent<EnemyManager>();
        xLimit = enemyManager.horizontalLimit;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // if any enemy moves to the horizontal limit all enemies change direction
        // and move down the screen a set amount
        if (transform.position.x > xLimit)
        {
            enemyManager.moveRight = false;
            enemyManager.moveDown = true;            
        } else if (transform.position.x < -xLimit)
        {
            enemyManager.moveRight = true;
            enemyManager.moveDown = true;
        }

             
    }

    // void OnCollisionEnter(Collision other)
    // {
    //     if (other.gameObject.CompareTag("Bullet"))
    //     {
    //         EnemyManager.ScoreChange(pointsValue);
    //         // No need to destroy bullet now
    //         // Destroy(other.gameObject);
    //         // Just deactivate it
    //         other.gameObject.SetActive(false);
    //         Destroy(gameObject);
    //         Instantiate(explosionParticles, transform.position + new Vector3(0, 1, 0.5f), 
    //         explosionParticles.transform.rotation);
    //     }
    // }
}
