using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] int maxHealth = 1;
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

    // Awake is called when the object is instantiated
    void Awake()
    {
        currentHealth = maxHealth;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
    }

    // This can called from other scripts on objects which can cause this one damage
   public void ApplyDamage(int amountOfDamage)
   {
       currentHealth -= amountOfDamage;
       if (currentHealth <= 0) {
           Death();
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
