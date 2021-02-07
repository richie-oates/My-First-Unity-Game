using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 25;
    public Vector3 direction;

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
    
}
