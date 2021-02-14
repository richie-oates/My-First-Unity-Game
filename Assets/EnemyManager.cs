using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    
    [SerializeField] string bulletTag;
    
    // private bool cowSpawning = false;
    private bool enemyWaveSpawning = false;
    
    public int timeBetweenWaves = 3;
    [SerializeField] float timeBetweenShotsMin = 0.3f;
    [SerializeField] float timeBetweenShotsMax = 1.0f;
    [SerializeField] Vector3 shootingDirection;
    private bool shooting = false;
    int enemyLevel;

    public TextMeshProUGUI newWaveText;
    public TextMeshProUGUI spawnCountdownText;
    [SerializeField] GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
       enemyLevel = 1; 
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

        if (!enemyWaveSpawning && !shooting && enemyObjectsArray.Length > 0)
        {
            StartCoroutine(RandomShooting());
            shooting = true;
        }
        
        // Check if there are any instances of the enemyCow still alive
        // If not, start a timer then spawn a new one
        // enemyCowsArray = GameObject.FindGameObjectsWithTag("SpaceCow");
        // if (enemyCowsArray.Length == 0 && cowSpawning == false)
        // {
        //     StartCoroutine(SpawnCowRoutine());
        //     cowSpawning = true;
        // }
    }

    IEnumerator RandomShooting()
    {
        // Random time between shots
        float timeBetweenShots = Random.Range(timeBetweenShotsMin, timeBetweenShotsMax) / enemyLevel;
        // Wait before shooting
        yield return new WaitForSeconds(timeBetweenShots);
        // Check if therew are still enemies on screen
        if (enemyObjectsArray.Length > 0) {
            // Get random enemy
            GameObject enemyShooter = RandomEnemyGenerator(enemyObjectsArray);
            if (enemyShooter != null) {
            // Shoot bullet instantiate at enemy postion
            ShootBullet(enemyShooter.transform.position);
        }
        }
        shooting = false;
    }
    // IEnumerator SpawnCowRoutine()
    // {
    //     yield return new WaitForSeconds(5);
    //     SpawnCow();
    //     cowSpawning = false;
    // }

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
        transform.Translate(Vector3.right * horizontalDirection * speed * enemyLevel * 2/3 * Time.deltaTime);
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
        bool gameActive = GameManager.gameActive;
        if (enemyWave > 5)
        {
            enemyWave = 1;
            enemyLevel++;
        }

        newWaveText.SetText("Level " + enemyLevel + "\nWave " + enemyWave);
        if (gameActive)
        {
        newWaveText.gameObject.SetActive(true);
        }
        int timer = timeBetweenWaves;
        
        yield return new WaitForSeconds(1);

        if (gameActive)
        {
            spawnCountdownText.gameObject.SetActive(true);
        }
        for (int i = 0; i < timeBetweenWaves; i++)
        {

            spawnCountdownText.SetText("Enemies inbound ... " + timer);       
            yield return new WaitForSeconds(1);
            
            timer = timer - 1;
        }

        spawnCountdownText.gameObject.SetActive(false);
        newWaveText.gameObject.SetActive(false);
        int enemyRows = enemyWave;

        for (int j = 0; j < enemyRows; j++)
        {
            for (int i = 0; i < enemiesInRow; i++)
            {
                Vector3 newPos = new Vector3(-7.5f + i * 2.5f, 0.8f, 11 + j *4);
                GameObject newEnemy = RandomEnemyGenerator(enemyPrefabs);
                if (newEnemy != null) 
                {
                newEnemy = Instantiate(newEnemy, newPos, Quaternion.identity);
                newEnemy.transform.parent = transform;
                }
            }
            
        }
        MoveVertically(enemyRows*2);
        enemyWave++;
        enemyWaveSpawning = false;
    }

    // // Spawns an enemyCow moving across the top of the screen
    // public void SpawnCow()
    // {
    //     Instantiate(enemyCowPrefab);
    // }

    void ShootBullet(Vector3 position)
    {
        // creates an instance of the bullet
        
        // GameObject newBullet = Instantiate(bullet, position, bullet.transform.rotation);
        // bullet.tag = gameObject.tag;
        // bullet.GetComponent<BulletController>().bulletDirection = Vector3.down;

        GameObject pooledProjectile = ObjectPooler.SharedInstance.GetPooledObject(bulletTag);
        if (pooledProjectile != null)
        {
            pooledProjectile.GetComponent<BulletController>().bulletDirection = shootingDirection;
            // Set the tag to Enemy so it doesn't interact with other enemies
            pooledProjectile.tag = bulletTag;
            // Set deactivate to true so it doesn't get destroyed out of bounds, just deactivated
            pooledProjectile.GetComponent<DestroyOutOfBounds>().deactivate = true;
            Debug.Log("projectileTag: " + pooledProjectile.tag + " shooterTag: " + gameObject.tag);
            pooledProjectile.transform.position = position; // position it at the passed parameter position
            pooledProjectile.SetActive(true); // activate it
        }
    }

    GameObject RandomEnemyGenerator(GameObject[] enemiesArray)
    {
        
        // while (enemy = null)
        // {
            int enemyNumber = Random.Range(0, enemiesArray.Length);
            GameObject enemy = enemiesArray[enemyNumber];
        // }
        Debug.Log("EnemyPrefabnumber: " + enemyNumber);
        return enemy;
        
    }
    
}
