using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualEnemy : MonoBehaviour
{
    private float xLimit;
    public GameObject enemyManagerObject;
    EnemyManager enemyManager;
    public int pointsValue;
    public int damageStrength = 1;
    [SerializeField] private GameObject gameManagerObject;


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
        }
        if (transform.position.x < -xLimit)
        {
            enemyManager.moveRight = true;
            enemyManager.moveDown = true;
        }

             
    }

    private void OnDestroy() {
        enemyManager.enemyObjectList.Remove(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.SendMessageUpwards("ApplyDamage", damageStrength, SendMessageOptions.DontRequireReceiver);

            gameManagerObject.GetComponent<GameManager>().GameOver();
        }

    }

}
