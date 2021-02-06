using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float speed = 7;
    public float horizontalDirection = 1;
    public float verticalMovementAmount = 0.1f;
    public bool moveRight = true;
    public bool moveDown = false;
    public float horizontalLimit = 11;
    public int enemiesInRow = 6;
    public int enemyWave = 1;
    
    // GameObject array of enemy prefabs which are inserted with unity
    public GameObject[] enemyPrefabs;
    // GameObject array of the instances of enemy objects on the screen
    public GameObject[] enemyObjectsArray;
    // EnemyCow prefab from unity
    public GameObject enemyCowPrefab;
    // Array to count the number of enemy cows on the screen
    public GameObject[] enemyCowsArray;
    
    private bool cowSpawning = false;
    private bool enemyWaveSpawning = false;
    
    public int timeBetweenWaves = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate() {
         MoveHorizontally(moveRight);
         if (moveDown == true)
         {
            MoveVertically(verticalMovementAmount);
         }
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if there are any enemies on the screen,
        // if not, spawn a new wave, increase the wave number
        enemyObjectsArray = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemyObjectsArray.Length == 0 && enemyWaveSpawning == false)
        {
            StartCoroutine(SpawnEnemies());
            enemyWaveSpawning = true;
        }
        
        // Check if there are any instances of the enemyCow still alive
        // If not, start a timer then spawn a new one
        enemyCowsArray = GameObject.FindGameObjectsWithTag("SpaceCow");
        if (enemyCowsArray.Length == 0 && cowSpawning == false)
        {
            StartCoroutine(SpawnCowRoutine());
            cowSpawning = true;
        }
    }

    IEnumerator SpawnCowRoutine()
    {
        yield return new WaitForSeconds(5);
        SpawnCow();
        cowSpawning = false;
    }

    // Change the direction of movement of the enemies and bring them closer to the player
    // This method is triggered in the IndividualEnemy script when any enemy reaches a horizontal limit
    public void MoveHorizontally(bool moveRight)
    {
        if (moveRight == true) 
        {
            horizontalDirection = 1;
        } 
        else
        {
            horizontalDirection = -1;
        }
        // Move all enemies horizontally
        transform.Translate(Vector3.right * horizontalDirection * speed * Time.deltaTime);
    }

    // Move down a set amount
    public void MoveVertically(float distance)
    {
        transform.Translate(Vector3.back * distance);
        moveDown = false;
    }


    // Waits a certain amount of time then spawns a new wave of enemies
    // The number of rows depends on the enemyWave number up to 
    // a maximum of the number of enemy prefabs in the array
    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < timeBetweenWaves; i++)
        {
            // need to setup canvas to display countdown timer
            int timer = timeBetweenWaves;
            yield return new WaitForSeconds(1);
            timer = timer - 1;
        }
        int enemyRows = enemyWave;
        if (enemyWave > enemyPrefabs.Length)
        {
            enemyRows = enemyPrefabs.Length;
        }
        for (int j = 0; j < enemyRows; j++)
        {
            for (int i = 0; i < enemiesInRow; i++)
            {
                Vector3 newPos = new Vector3(-7.5f + i * 2.5f, 0.8f, 11 + j * 2);
                GameObject newEnemy = Instantiate(enemyPrefabs[j], newPos, Quaternion.identity);
                newEnemy.transform.parent = transform;
            }
            
        }
        MoveVertically(enemyRows*2);
        enemyWave++;
        enemyWaveSpawning = false;
    }

    // Spawns an enemyCow moving across the top of the screen
    public void SpawnCow()
    {
        Instantiate(enemyCowPrefab);
    }
}
