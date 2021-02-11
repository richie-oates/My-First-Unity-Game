using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float playerSpeed = 30;
    [SerializeField] float horizontalInput;
    [SerializeField] float horizontalLimit = 20;
    [SerializeField] float maxRotation = 60;
    [SerializeField] float smooth = 2.5f;
    [SerializeField] float stopSpeed = 0.1f;
    [SerializeField] float startSpeed = 0.4f;

    [SerializeField] string bulletTag;
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] GameObject gameManagerObject;
    [SerializeField] Vector3 shootingDirection;
    private GameManager gameManager;
    Collider playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = gameManagerObject.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
        ConstrainPlayerPosition();     
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootBullet();
        }
    }

    // Moves player based on horizontal input
    void MovePlayer()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // Makes for a quicker stop but also a slower start when moving
        if (Mathf.Abs(horizontalInput) < stopSpeed)
        {
            horizontalInput = 0;
        }

        // Makes for a quicker start
        if (horizontalInput > startSpeed)
        {
            horizontalInput = 1;
        }

        // Makes for a quicker start
        if (horizontalInput < -startSpeed)
        {
            horizontalInput = -1;
        }

        transform.Translate(Vector3.right * playerSpeed * horizontalInput * Time.deltaTime, Space.World);

        // Smoothly tilts a transform towards a target rotation.
        float tiltAroundZ = -horizontalInput * maxRotation;

        // Rotate the player by converting the angles into a quaternion.
        Quaternion target = Quaternion.Euler(0, 0, tiltAroundZ);

        // Dampen towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
        
    }

    // Stop the player from moving outside the horizontalbounds of the screen
    void ConstrainPlayerPosition()
    {
        // Constrain horizontal movement
        if (Mathf.Abs(transform.position.x) > horizontalLimit)
        {
            transform.position = new Vector3(horizontalLimit * Mathf.Sign(transform.position.x),
            transform.position.y, transform.position.z);
        }
    }

    void ShootBullet() 
    {
        GameObject pooledProjectile = ObjectPooler.SharedInstance.GetPooledObject(bulletTag);
        if (pooledProjectile != null)
        {
            pooledProjectile.GetComponent<BulletController>().bulletDirection = shootingDirection;
            pooledProjectile.tag = bulletTag;
            pooledProjectile.GetComponent<DestroyOutOfBounds>().deactivate = true;
            Debug.Log("projectileTag: " + pooledProjectile.tag + " shooterTag: " + gameObject.tag);
            pooledProjectile.transform.position = transform.position + new Vector3(0, 0, 1.5f); // position it at player
            pooledProjectile.SetActive(true); // activate it
            pooledProjectile.transform.position = transform.position + new Vector3(0, 0, 1.5f); // position it at player
        }
    }
}
