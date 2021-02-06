using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script will destroy any object that comes into contact with another object
// If the object destroyed is an enemy then points are added to the player score in GameManager
// If the object destroyed is the player then it triggers GameOver
// If the object goes out of bounds it is deactivated or triggers game over

public class DestroyObject : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] int pointsValue;
    [SerializeField] AudioClip explosionSound;
    AudioSource audioSource;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        GameObject gameManagerObject = GameObject.Find("Game Manager");
        gameManager = gameManagerObject.GetComponent<GameManager>();
    }

    void OnCollisionEnter(Collision other)
    {
        // If gameObject is a bullet SetActive to false once I've got the pool manager working
        // for now just destroy it
        
        if (gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
            // gameObject.SetActive(false);
        } else
        // else destroy the game object and update score (if it has one) 
        // or trigger game over (if player)
        {
            // Explosion particles
            Instantiate(explosionParticles, transform.position + new Vector3(0, 1, 0.5f),
             explosionParticles.transform.rotation);
             audioSource.PlayOneShot(explosionSound);

            if (pointsValue != 0)
            {
                gameManager.ScoreChange(pointsValue);
            } 
            else if (gameObject.CompareTag("Player"))
            {
                gameManager.GameOver();
            }
            Destroy(gameObject);
        }
    }

}