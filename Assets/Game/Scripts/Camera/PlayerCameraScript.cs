using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraScript : MonoBehaviour
{
    public Transform targetObject;
    // NOT WORKING!!!
    static int camera_height = 10;
    
    void Update()
    {

        transform.position = new Vector3(targetObject.position.x, targetObject.position.y, targetObject.position.z - camera_height);
    }
}
