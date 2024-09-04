using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;


public class PlayerMovement : MonoBehaviour
{
    PhotonView view;
    public int playerTeam;
   

    [Header("Player Health")]
    const float MaxHealth = 150f;
    public float CurrentHealth;
    public Slider Healthbar;
    public GameObject playerUI;
    InputManager inputManager;
    PlayerManager playerManager;
    PlayerControllerManager playercontroller;
    AnimationScript animationscript;


    Vector3 Movedir;
    Transform Cameragameobject;
    Rigidbody PlayerrRb;
    [Header("Movement Flags")]
    public bool ismoving;
    public bool issprinting;


    [Header("Movement Values")]

    public float rspeed;
    public float mspeed;

    public float sprintspeed;
    [Header("Faling And Landing")]

    public float inairtimer;
    public float leapingvelocity;
    public float fallingvelocity;
    public float raycastheightoffset;
    public bool isGrounded;
    public LayerMask groundlayer;
    [Header("Jump var")]
    public float jumpheight;
    public float gravityintensity;
    public bool isJumping;


    
    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerManager = GetComponent<PlayerManager>();
        animationscript = GetComponent<AnimationScript>();
        PlayerrRb = GetComponent<Rigidbody>();
        Cameragameobject = Camera.main.transform;
        CurrentHealth = MaxHealth;
        view=GetComponent<PhotonView>();    
    
        Healthbar.minValue = 0f;
        Healthbar.maxValue = MaxHealth;
        Healthbar.value = CurrentHealth;

        playercontroller = PhotonView.Find((int)view.InstantiationData[0]).GetComponent<PlayerControllerManager>();
    }
    private void Start()
    {
        if (!view.IsMine)
        {
            Destroy(playerUI);
            Destroy(PlayerrRb);
        }
        if (view.Owner.CustomProperties.ContainsKey("Team"))
        {
            int team = (int)view.Owner.CustomProperties["Team"];
            playerTeam = team;
        }
    }
    public void HandleMovement()
    {
        if (isJumping)
            return;

        Movedir = new Vector3(Cameragameobject.forward.x, 0f, Cameragameobject.forward.z) * inputManager.VerticalInput;
        Movedir = Movedir + Cameragameobject.right * inputManager.HorizontalInput;
        Movedir.Normalize();
        Movedir.y = 0;
        if (issprinting)
        {
            Movedir = Movedir * sprintspeed;
        }
        else
        {
            if (inputManager.Movementamount >= 0.5f)
            {
                Movedir = Movedir * mspeed;
                ismoving = true;

            }
            if (inputManager.Movementamount <= 0f)
            {
                ismoving = false;
            }
        }
        Vector3 Movementvelocity = Movedir;
        PlayerrRb.velocity = Movementvelocity;

    }
    public void HandleAllMovement()
    {
        FallingandLanding();
        if (playerManager.isIntracting)
            return;
        HandleRotation();
        HandleMovement();

    }
    public void HandleRotation()
    {
        if (isJumping)
            return;

        Vector3 Targetdir = Vector3.zero;
        Targetdir = Cameragameobject.forward * inputManager.VerticalInput;
        Targetdir = Targetdir + Cameragameobject.right * inputManager.HorizontalInput;
        Targetdir.Normalize();
        Targetdir.y = 0;
        if (Targetdir == Vector3.zero)
        {
            Targetdir = transform.forward;
        }
        Quaternion targetrotation = Quaternion.LookRotation(Targetdir);
        Quaternion Playerrotation = Quaternion.Slerp(transform.rotation, targetrotation, rspeed * Time.deltaTime);
        transform.rotation = Playerrotation;

    }
    public void FallingandLanding()
    {
        RaycastHit hit;
        Vector3 raycastorigin = transform.position;
        Vector3 targetposition;
        raycastorigin.y = raycastorigin.y + raycastheightoffset;
        targetposition = transform.position;
        if (!isGrounded && !isJumping)
        {
            if (!playerManager.isIntracting)
            {
                animationscript.PlayTargetAnim("Falling", true);
            }
            inairtimer = inairtimer + Time.deltaTime;
            PlayerrRb.AddForce(transform.forward * leapingvelocity);
            PlayerrRb.AddForce(-Vector3.up * fallingvelocity * inairtimer);
        }
        if (Physics.SphereCast(raycastorigin, 0.2f, -Vector3.up, out hit, groundlayer))
        {
            if (!isGrounded && !playerManager.isIntracting)
            {
                animationscript.PlayTargetAnim("Landing", true);
            }
            Vector3 raycasthitpoint = hit.point;
            targetposition.y = raycasthitpoint.y;
            inairtimer = 0;
            isGrounded = true;



        }
        else
        {
            isGrounded = false;
        }
        if (isGrounded && !isJumping)
        {
            if (playerManager.isIntracting || inputManager.Movementamount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetposition, Time.deltaTime / 0.1f);
            }
            else
            {
                transform.position = targetposition;
            }
        }

    }
    public void HandleJumping()
    {
        if (isGrounded)
        {
            animationscript.animator.SetBool("isJumping", true);
            animationscript.PlayTargetAnim("Jump", false);
            float jumpingvelocity = Mathf.Sqrt(-2 * gravityintensity * jumpheight);
            Vector3 playervelocity = Movedir;
            playervelocity.y = jumpingvelocity;
            PlayerrRb.velocity = playervelocity;

            isJumping = false;
        }
    }
    public void SetisJumping(bool isJumping)
    {
        this.isJumping = isJumping;
    }
    public void ApplyDamage(float Damagevalue)
    {
        view.RPC("RPC_TakeDamage", RpcTarget.All, Damagevalue);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (!view.IsMine)
            return;
        CurrentHealth -= damage;
        Healthbar.value = CurrentHealth;
        if (CurrentHealth <= 0)
        {
            Die();
        }
        Debug.Log("Damage Taken" + damage);
        Debug.Log("CurrentHealth" + CurrentHealth);
    }
    void Die()
    {
        playercontroller.Die();
        Scorecard.Instance.Playerdied(playerTeam);
    }
}
