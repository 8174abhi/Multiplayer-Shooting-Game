using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{


    PlayerInputActions Playerinputactions;
    AnimationScript animationscript;
    PlayerMovement pmovement;
    public Vector2 MovementInput;
    public float VerticalInput;
    public float HorizontalInput;
    public float Movementamount;
    public Vector2 CameraMovementInput;
    public float CameraInputx;
    public float CameraInputY;
    [Header("Input button flags")]
    public bool jumpInput;
    public bool bInput;
    public bool FireInput;
    public bool ReloadInput;
    public bool ScopeInput;


    private void Awake()
    {
        animationscript = GetComponent<AnimationScript>();
        pmovement = GetComponent<PlayerMovement>();
    }



    private void OnEnable()
    {
        if (Playerinputactions == null)
        {
            Playerinputactions = new PlayerInputActions();
            Playerinputactions.PlayerMovement.Movement.performed += i => MovementInput = i.ReadValue<Vector2>();
            Playerinputactions.PlayerMovement.CameraMovement.performed += i => CameraMovementInput = i.ReadValue<Vector2>();

            Playerinputactions.Playeractions.B.performed += i => bInput = true;
            Playerinputactions.Playeractions.B.canceled += i => bInput = false;
            Playerinputactions.Playeractions.jump.performed += i => jumpInput = true;
            Playerinputactions.Playeractions.jump.canceled += i => jumpInput = false;
            Playerinputactions.Playeractions.Fire.performed += i => FireInput = true;
            Playerinputactions.Playeractions.Fire.canceled += i => FireInput = false;
            Playerinputactions.Playeractions.Reload.performed += i => ReloadInput = true;
            Playerinputactions.Playeractions.Reload.canceled += i => ReloadInput = false;
            Playerinputactions.Playeractions.Scope.performed += i => ScopeInput = true;
            Playerinputactions.Playeractions.Scope.canceled += i => ScopeInput = false;
        }
        Playerinputactions.Enable();

    }
    private void OnDisable()
    {
        Playerinputactions.Disable();
    }
    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprinting();
        HandleJumpingInput();

    }
    public void HandleMovementInput()
    {
        VerticalInput = MovementInput.y;
        HorizontalInput = MovementInput.x;
        Movementamount = Mathf.Clamp01(Mathf.Abs(HorizontalInput) + Mathf.Abs(VerticalInput));
        animationscript.ChangeAnimatorValues(0, Movementamount, pmovement.issprinting);
        CameraInputx = CameraMovementInput.x;
        CameraInputY = CameraMovementInput.y;
    }
    void HandleSprinting()
    {
        if (bInput && Movementamount > 0.5)
        {
            pmovement.issprinting = true;

        }
        else
        {
            pmovement.issprinting = false;
        }
    }
    void HandleJumpingInput()
    {
        if (jumpInput)
        {
            jumpInput = false;
            pmovement.HandleJumping();
        }

    }


}
