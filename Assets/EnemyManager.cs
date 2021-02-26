using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/*  - Spawns waves of enemies
    - Controls enemy movement
    - Displays text between enemy waves
    - Controls enemy shooting
*/

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
    public List<GameObject> enemyPrefabs;

    // GameObject array of the instances of enemy objects on the screen
    // public GameObject[] enemyObjectsArray;
    public List<GameObject> enemyObjectList;
    
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
        // enemyObjectsArray = GameObject.FindGameObjectsWithTag("Enemy");
        if (GameManager.gameActive && enemyObjectList.Count == 0 && enemyWaveSpawning == false)
        {
            StartCoroutine(SpawnEnemies());
            enemyWaveSpawning = true;
            // Check to make sure it's not the very first wave and level
            if (enemyWave > 1 || enemyLevel > 1)
            {
                EventBroker.callEnemyWaveDefeated();
            }
        }

        if (!enemyWaveSpawning && !shooting && enemyObjectList.Count > 0)
        {
            StartCoroutine(RandomShooting());
            shooting = true;
        }
    }

    IEnumerator RandomShooting()
    {
        // Random time between shots
        float timeBetweenShots = Random.Range(timeBetweenShotsMin, timeBetweenShotsMax) / enemyLevel;
        // Wait before shooting
        yield return new WaitForSeconds(timeBetweenShots);
        // Check if therew are still enemies on screen
        if (enemyObjectList.Count > 0) {
            // Get random enemy
            GameObject enemyShooter = RandomEnemyGenerator(enemyObjectList);
            if (enemyShooter != null) {
            // Shoot bullet instantiate at enemy postion
            ShootBullet(enemyShooter.transform.position);
        }
        }
        shooting = false;
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
        transform.Translate(Vector3.right * horizontalDirection * speed * enemyLevel * 2/3 * Time.deltaTime);
    }

    // Move down a set amount
    public void MoveVertically(float distance)
    {
        transform.Translate(Vector3.back * distance);
        moveDown = false;
    }


    // Waits a certain amount of time then spawns a new wave of enemies
    // The number of rows depends on the enemyWave number
    IEnumerator SpawnEnemies()
    {
        if (enemyWave > 5)
        {
            enemyWave = 1;
            enemyLevel++;
        }

        newWaveText.SetText("Level " + enemyLevel + "\nWave " + enemyWave);      
        newWaveText.gameObject.SetActive(true);

        int timer = timeBetweenWaves;
        
        yield return new WaitForSeconds(1);
        spawnCountdownText.gameObject.SetActive(true);

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
                enemyObjectList.Add(newEnemy);
                }
            }
            
        }
        MoveVertically(enemyRows*2);
        enemyWave++;
        enemyWaveSpawning = false;
    }

    void ShootBullet(Vector3 position)
    {
        GameObject pooledProjectile = ObjectPooler.SharedInstance.GetPooledObject(bulletTag);
        if (pooledProjectile != null)
        {
            pooledProjectile.GetComponent<BulletController>().bulletDirection = shootingDirection;
            // Set the tag to Enemy so it doesn't interact with other enemies
            pooledProjectile.tag = bulletTag;
            // Set deactivate to true so it doesn't get destroyed out of bounds, just deactivated
            pooledProjectile.GetComponent<DestroyOutOfBounds>().deactivate = true;
            pooledProjectile.transform.position = position; // position it at the passed parameter position
            pooledProjectile.SetActive(true); // activate it
        }
    }

    GameObject RandomEnemyGenerator(List<GameObject> enemyList)
    {
        int enemyNumber = Random.Range(0, enemyList.Count);
        GameObject enemy = enemyList[enemyNumber];
       
        return enemy;
        
    }
    
}
