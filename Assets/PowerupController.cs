using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] AudioClip collectPowerupSound;
    AudioSource audioSource;
    public int powerupLevel;
    public int powerupType;

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * -moveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (powerupType == 1)
            {
            other.gameObject.SendMessageUpwards("HealthPowerup", powerupLevel, SendMessageOptions.DontRequireReceiver);
            } else
            if (powerupType == 2)
            {
            other.gameObject.SendMessageUpwards("ShieldPowerup", powerupLevel, SendMessageOptions.DontRequireReceiver);    
            }
            gameObject.SetActive(false);
        }
    }
}
