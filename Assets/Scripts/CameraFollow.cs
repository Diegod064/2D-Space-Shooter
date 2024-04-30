using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform myTarget;
    // Update is called once per frame
    void Update()
    {
        if (myTarget != null)
        {
            Vector3 targPos = transform.position;
            targPos.z = transform.position.z;
            
            // Consider using Vector3.lerp
            transform.position = targPos;

        }
    }
}