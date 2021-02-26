using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject powerup;
    public Material healthMaterial;
    public Material shieldMaterial;

    private void Start() 
    {
        EventBroker.EnemyWaveDefeated += SpawnPowerup;
    }
   private void SpawnPowerup()
    {
        Vector3 pos = new Vector3(Random.Range(-10, 10), 0, 21);
        // Instantiate(powerup, pos, Quaternion.identity);

        GameObject pooledPowerup = ObjectPooler.SharedInstance.GetPooledObject("Powerup");
        if (pooledPowerup != null)
        {
            pooledPowerup.GetComponent<DestroyOutOfBounds>().deactivate = true;
            pooledPowerup.transform.position = pos; // position it at random pos at top of screen
            // Randomise poweruptype
            int randomType = Random.Range(1, 3);
            pooledPowerup.GetComponent<PowerupController>().powerupType = randomType;
            if (randomType == 1)
            {
                pooledPowerup.tag = "Health Powerup";
                pooledPowerup.GetComponent<MeshRenderer>().material = healthMaterial;
            }
            else
        if (randomType == 2)
            {
                pooledPowerup.GetComponent<MeshRenderer>().material = shieldMaterial;
                pooledPowerup.tag = "Shield Powerup";
            }
            Debug.Log("Powerup: " + randomType);
            pooledPowerup.SetActive(true); // activate it
            
        }
    }
}
