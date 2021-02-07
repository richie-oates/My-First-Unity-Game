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

    [SerializeField] GameObject bullet;
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
        // creates an instance of the bullet
        // Instantiate(bullet, transform.position + new Vector3(0, 0, 1.5f), bullet.transform.rotation);
        // BulletController bulletController = bullet.GetComponent<BulletController>();
        // bulletController.direction = Vector3.up;
        // bullet.layer = 9;

        // Todo: This now links the rotation of bullet to the player so it turns when the player does,
        // that's not what we want!!
        // Get an object from the pool
        GameObject pooledProjectile = ObjectPooler.SharedInstance.GetPooledObject();
        if (pooledProjectile != null)
        {
            pooledProjectile.GetComponent<BulletController>().bulletDirection = shootingDirection;
            pooledProjectile.tag = gameObject.tag;
            pooledProjectile.GetComponent<DestroyOutOfBounds>().deactivate = true;
            Debug.Log("projectileTag: " + pooledProjectile.tag + " shooterTag: " + gameObject.tag);
            pooledProjectile.transform.position = transform.position + new Vector3(0, 0, 1.5f); // position it at player
            pooledProjectile.SetActive(true); // activate it
            // Tried this to correct rotation but didn't work
            // pooledProjectile.transform.rotation = Quaternion.Euler(0, 0, 0);
            pooledProjectile.transform.position = transform.position + new Vector3(0, 0, 1.5f); // position it at player
        }
    }
}
