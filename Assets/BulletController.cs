using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 25;
    public int damageStrength = 1;
    public Vector3 bulletDirection;
    public List<string> destroysObjectsWithTag;

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        transform.Translate(bulletDirection * speed * Time.deltaTime);
    }
    
    void OnTriggerEnter(Collider other) {
        foreach (string tag in destroysObjectsWithTag)
        {
            if (other.gameObject.tag == tag)
            {
                other.gameObject.SendMessageUpwards("ApplyDamage", damageStrength, SendMessageOptions.DontRequireReceiver);
                gameObject.SetActive(false);
            }
        }
    }
}
