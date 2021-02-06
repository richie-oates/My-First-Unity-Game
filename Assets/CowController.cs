using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowController : MonoBehaviour
{
    public float speed = 10;
    public float xLimit = 25;
    public float horizontalDirection = 1;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.right * horizontalDirection * speed * Time.deltaTime);
 
    }
    private void Update()
    {
       ChangeDirection(); 
    }

    void ChangeDirection()
    {
        if (transform.position.x > xLimit)
        {
            horizontalDirection = -1;
        }
        if (transform.position.x < -xLimit)
        {
            horizontalDirection = 1;
        }
    }   
}    
    
