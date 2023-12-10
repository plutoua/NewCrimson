using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentMover : MonoBehaviour
{
    public Transform targetObject;
    
    void Update()
    {
        transform.position = new Vector3(targetObject.position.x, targetObject.position.y, targetObject.position.z);
    }
}
