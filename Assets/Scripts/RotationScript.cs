using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    public float rotationsPerMinute = 640f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotationsPerMinute * Time.deltaTime, Space.Self);//This will rotate the GameObject the script is attached to around the Z-axis based on the value of rotationsPerMinute.
    }
}
