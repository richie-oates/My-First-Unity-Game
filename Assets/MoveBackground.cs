using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    public GameObject background;
    public float moveSpeed;
    public float zLimit;
    float zStart;
    public float zSize;

    // Start is called before the first frame update
    void Start()
    {
        zStart = transform.position.z;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * -moveSpeed * Time.deltaTime);

        if (transform.position.z < zStart - zLimit)
        {
            transform.position = transform.position + Vector3.forward * zSize;
        }
    }
}
