using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    [SerializeField] float zLimitUpper = 20;
    [SerializeField] float zLimitLower = -5;
    [SerializeField] float xLimitUpper = 12;
    [SerializeField] float xLimitLower = -12;
    public bool deactivate;


    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        if (pos.z > zLimitUpper || pos.z < zLimitLower || pos.x > xLimitUpper|| pos.x < xLimitLower)
        {
            if (deactivate)
            {
                gameObject.SetActive(false);
            } else
            {
                Destroy(gameObject);
            }
        }
    }
}
