using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 25;

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    // private void Update() 
    // {
    //     if (transform.position.z > 18)
    //     {
    //         // Destroy(gameObject);

    //         // Just deactivate it
    //         gameObject.SetActive(false);
    //     }
    // }
    
}
