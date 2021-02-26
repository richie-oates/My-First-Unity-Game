using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] int maxHealth = 5;
    // In case we don't want the explosion particles to start at the object position 
    // we can apply an offset
    [SerializeField] Vector3 explosionOffset = new Vector3(0, 1, 0.5f);
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] AudioClip explosionSound;
    // The amount of points added to the player score if theis object is destroyed
    [SerializeField] int pointsValue;

    private AudioSource audioSource;
    private GameManager gameManager;

    public int currentHealth;
    public int currentShieldLevel;
    public int maxShieldLevel;

    public TextMeshProUGUI shieldIndicator;
    public TextMeshProUGUI healthIndicator;

    // Awake is called when the object is instantiated
    void Awake()
    {
        
        currentHealth = maxHealth;
        if (gameObject.tag == "Player")
        {
        shieldIndicator.text = "Shield: " + currentShieldLevel;
        healthIndicator.text = "Health: " + currentHealth;
        }   
        
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
    }

    // This can be called from other scripts on objects which can cause this one damage
   public void ApplyDamage(int amountOfDamage)
   {
        if (currentShieldLevel > 0)
        {
            currentShieldLevel -= amountOfDamage;
            if (currentShieldLevel < 0)
            {
                currentHealth += currentShieldLevel;
                currentShieldLevel = 0;
            }
        }
        else
        {
            currentHealth -= amountOfDamage;
        }

        if (gameObject.tag == "Player")
        {
            shieldIndicator.text = "Shield: " + currentShieldLevel;
            healthIndicator.text = "Health: " + currentHealth;
        }

        if (currentHealth <= 0)
            Death();

   }

   public void ShieldPowerup(int powerupLevel)
   {
       currentShieldLevel += powerupLevel;
       if (currentShieldLevel > maxShieldLevel)
        {currentShieldLevel = maxShieldLevel;}

        if (gameObject.tag == "Player")
        {
            shieldIndicator.text = "Shield: " + currentShieldLevel;
            // healthIndicator.text = "Health: " + currentHealth;
        }
   }

    public void HealthPowerup(int powerupLevel)
    {
        currentHealth += powerupLevel;
        if (currentHealth > maxHealth)
        { currentHealth = maxHealth; }

        if (gameObject.tag == "Player")
        {
            healthIndicator.text = "Health: " + currentHealth;
        }
    }


    void Death()
   {
       // Explosion particles and sound
        Instantiate(explosionParticles, transform.position + explosionOffset ,
        explosionParticles.transform.rotation);
        audioSource.PlayOneShot(explosionSound);

        // Increase score in gameManger
        if (pointsValue != 0)
            {
            gameManager.ScoreChange(pointsValue);
                Destroy(gameObject);
            } 
        if (gameObject.name == "Player")
        {
            Destroy(gameObject);
            gameManager.GameOver();

        }
        
   }
}