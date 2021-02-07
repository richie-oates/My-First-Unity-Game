using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 25;
    public int bulletStrength = 1;
    public Vector3 bulletDirection;

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        transform.Translate(bulletDirection * speed * Time.deltaTime);
    }
    
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag != gameObject.tag)
        {
            other.gameObject.SendMessageUpwards("ApplyDamage", bulletStrength, SendMessageOptions.DontRequireReceiver);
            gameObject.SetActive(false);
        }
    }
}
