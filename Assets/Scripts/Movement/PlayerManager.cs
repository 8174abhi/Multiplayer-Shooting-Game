using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerMovement playermovement;
    CameraManager cameraManager;
    public bool isIntracting;
    Animator animator;
    PhotonView view;

  
    private void Awake()
    {
        view=GetComponent<PhotonView>();
        inputManager = GetComponent<InputManager>();
        playermovement = GetComponent<PlayerMovement>();
        cameraManager = FindObjectOfType<CameraManager>();
        animator = GetComponent<Animator>();

    }
    private void Start()
    {
        if (!view.IsMine)
        {
            Destroy(GetComponentInChildren<CameraManager>().gameObject);
        }
    }
    private void Update()
    {
        if (!view.IsMine)

            return;

        inputManager.HandleAllInputs();
    }
    private void FixedUpdate()
    {
        if (!view.IsMine)

            return;
        playermovement.HandleAllMovement();
    }
    private void LateUpdate()
    {
        if (!view.IsMine)

            return;
        cameraManager.HandleAllCameramovement();
        isIntracting = animator.GetBool("isIntracting");
        playermovement.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", playermovement.isGrounded);

    }
   
}
