using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usernamebillboard : MonoBehaviour
{
    Camera Cam;
    private void Update()
    {
        if(Cam == null)
        {
            Cam=FindObjectOfType<Camera>();
        }
        if(Cam == null)
        
            return;
        transform.LookAt(Cam.transform);
        transform.Rotate(Vector3.up * 180);
        

    }
}
