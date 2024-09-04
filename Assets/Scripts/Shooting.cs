using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shooting : MonoBehaviour
{
 
    PhotonView view;
   Animator animator;
    InputManager inputManager;
    PlayerMovement playerMovement;

    [Header("Shooting Var")]
    public float FireRate = 0f;
    public float FireRange = 100f;
    public float FireDamage = 15f;
    float NextFireTime = 0f;
    public Transform Firepoint;
    [Header("Shooting Flags")]
    public bool isshooting;
    public bool iswalking;
    public bool isshootinginput;
    public bool isreloading = false;    
    [Header("Reloading")]
    public int maxammo = 30;
     int currentammo;
    public float relodingtime = 1.5f;
    public bool isScopeinput;
    [Header("Effects")]
    public GameObject Muzzleflash;
    public ParticleSystem bloodeffects;
    public Transform Muzzleflashpos;
    [Header("Sound Effects")]
    public AudioSource Soundaudio;
    public AudioClip shootingclip;
    public AudioClip reloadingclip;

    public int playerTeam;

    private void Start()
    {
       
        
        view =GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
        playerMovement = GetComponent<PlayerMovement>();
        currentammo=maxammo;
        if(view.Owner.CustomProperties.ContainsKey("Team"))
        {
            int team = (int)view.Owner.CustomProperties["Team"];
            playerTeam = team;
        }
        
    }
    private void Update()
    {
        if (!view.IsMine)
            return;
        if (isreloading || playerMovement.issprinting)
        {
            animator.SetBool("Shoot", false);
            animator.SetBool("ShootingMovement", false);
            animator.SetBool("ShootWalk", false);
            return;
        }
        iswalking = playerMovement.ismoving;
        isScopeinput = inputManager.ScopeInput;
        isshootinginput = inputManager.FireInput;
        if (isshootinginput && iswalking)
        {
            if (Time.time >= NextFireTime)
            {

                NextFireTime = Time.time + 1 / FireRate;
                Shoot();
                animator.SetBool("ShootWalk", true);
            }
            animator.SetBool("Shoot", false);
            animator.SetBool("ShootingMovement", true);
            isshooting = true;
        }
        else if (isshootinginput)
        {
            if (Time.time >= NextFireTime)
            {

                NextFireTime = Time.time + 1 / FireRate;
                Shoot();

            }
            animator.SetBool("Shoot", true);
            animator.SetBool("ShootingMovement", false);
            animator.SetBool("ShootWalk", false);
            isshooting = true;
        }
        else if (isScopeinput)
        {
            animator.SetBool("Shoot", false);
            animator.SetBool("ShootingMovement", true);
            animator.SetBool("ShootWalk", false);
            isshooting = false;

        }
        else
        {
            animator.SetBool("Shoot", false);
            animator.SetBool("ShootingMovement", false);
            animator.SetBool("ShootWalk", false);
            isshooting = false;
        }
        if (inputManager.ReloadInput && currentammo < maxammo)
        {
            Reload();
        }
    }
    void Shoot()
    {
        if (currentammo > 0)

        {
            RaycastHit hit;
            if (Physics.Raycast(Firepoint.position, Firepoint.forward, out hit, FireRange))
            {
                Debug.Log(hit.transform.name);
                Vector3 hitpoint = hit.point;
                Vector3 hitnormal = hit.normal;
                PlayerMovement playerMovementdamage = hit.collider.GetComponent<PlayerMovement>();
                if (playerMovementdamage != null && playerMovementdamage.playerTeam != playerTeam) 
                {
                    playerMovementdamage.ApplyDamage(FireDamage);
                    view.RPC("RPC_Shoot", RpcTarget.All, hitpoint, hitnormal);
                }
            }

            GameObject flash = Instantiate(Muzzleflash, Muzzleflashpos);
            Destroy(flash, .1f);
            Soundaudio.PlayOneShot(shootingclip);
            currentammo--;
        }
        else
        {
            Reload();
        }

    }
    [PunRPC]
    void RPC_Shoot(Vector3 hitpoint, Vector3 hitnormal)

    {
        ParticleSystem blood = Instantiate(bloodeffects, hitpoint, Quaternion.LookRotation(hitnormal));
        Destroy(blood.gameObject,blood.main.duration);
        Debug.Log("Blood Instantiated");
    }

    void Reload()
    {
        if (!isreloading && currentammo < maxammo)
        {
            if (isshootinginput && iswalking)
            {
                animator.SetTrigger("ShootReload");
            }
            else
            {
                animator.SetTrigger("Reload");
            }
            isreloading = true;
            Soundaudio.PlayOneShot(reloadingclip);
            Invoke("FinishReloading", relodingtime);
        }

    }
    void FinishReloading()
    {
        currentammo = maxammo;
        isreloading = false;
        if (isshootinginput && iswalking)
        {
            animator.ResetTrigger("ShootReload");
        }
        else
        {
            animator.ResetTrigger("Reload");
        }
    }
}
