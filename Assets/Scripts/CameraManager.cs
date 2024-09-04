using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerMovement playerMovement;
    public Transform PlayerTransform;
    public Transform CameraTransform;
    [Header("Camera Movement")]
    private Vector3 CameraFollowVelocity;
    public float CameraFollowSpeed;
    public float lookangle = 20;
    public float pivotangle;
    public float minpivotangle = -30;
    public float maxpivotangle = 30;
    public float lookspeed = 2f;
    public float pivotspeed = 2f;
    public Transform camerapivot;
    [Header("Camera Collision")]
    public float Cameracollisionoffset = .2f;
    public float MinCollisionoffset = .2f;
    public float CameraCollisionRadius = .2f;
    public LayerMask collisionlayer;
    public float defaultposition;
    Vector3 Cameravectorpos;
    [Header("Scope")]
    public Camera Maincamera;
    public GameObject Scopecanvas;
    
    public GameObject Playerui;
    float originalfov = 60f;
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        PlayerTransform = FindObjectOfType<PlayerManager>().transform;
        inputManager = FindObjectOfType<InputManager>();
        CameraTransform = Camera.main.transform;
        defaultposition = CameraTransform.localPosition.z;
        //playerMovement = GetComponent<PlayerMovement>();
        playerMovement=FindObjectOfType<PlayerMovement>();  
    }
    public void HandleAllCameramovement()
    {
        FollowTarget();
        CameraRotation();
        CameraCollision();
        isscopeinput();
    
    }

    public void FollowTarget()
    {
        Vector3 targetpos = Vector3.SmoothDamp(transform.position, PlayerTransform.position, ref CameraFollowVelocity, CameraFollowSpeed);
        transform.position = targetpos;
    }
    void CameraRotation()
    {
        Vector3 rotation;
        Quaternion targetrotation;
        lookangle = lookangle + (inputManager.CameraInputx * lookspeed);
        pivotangle = pivotangle + (inputManager.CameraInputY * pivotspeed);
        pivotangle = Mathf.Clamp(pivotangle, minpivotangle, maxpivotangle);
        rotation = Vector3.zero;
        rotation.y = lookangle;
        targetrotation = Quaternion.Euler(rotation);
        transform.rotation = targetrotation;

        rotation = Vector3.zero;
        rotation.x = pivotangle;
        targetrotation = Quaternion.Euler(rotation);
        camerapivot.localRotation = targetrotation;

        if (playerMovement.ismoving == false && playerMovement.issprinting == false)
        {
            PlayerTransform.rotation = Quaternion.Euler(0, lookangle, 0);
        }

    }
    public void CameraCollision()
    {
        float targetposition = defaultposition;
        RaycastHit hit;
        Vector3 dir = CameraTransform.position - camerapivot.position;
        dir.Normalize();

        if (Physics.SphereCast(camerapivot.transform.position, CameraCollisionRadius, dir, out hit, Mathf.Abs(targetposition), collisionlayer))

        {
            float distance = Vector3.Distance(camerapivot.position, hit.point);
            targetposition = -(distance - Cameracollisionoffset);
            if (Mathf.Abs(targetposition) < MinCollisionoffset)
            {
                targetposition = targetposition - MinCollisionoffset;
            }
            Cameravectorpos.z = Mathf.Lerp(CameraTransform.localPosition.z, targetposition, 0.2f);
            CameraTransform.localPosition = Cameravectorpos;
        }

    }
    public void isscopeinput()
    {
        if(inputManager.ScopeInput)
        {
            Scopecanvas.SetActive(true);
            Playerui.SetActive(false);
            Maincamera.fieldOfView = 20f;
        }
        else
        {
            Scopecanvas.SetActive(false);
            Playerui.SetActive(true);
            Maincamera.fieldOfView = originalfov;

        }
    }




}
