using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class Spawner : MonoBehaviourPunCallbacks
{
  public static Spawner instance;
    //public GameObject Playercontroller;
    
    private void Awake()

    {
       
       if( instance)
        {
            Destroy( gameObject );
            return;
        }
       DontDestroyOnLoad( gameObject );
        instance = this; 
    }
    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;


    }
    void OnSceneLoaded(Scene scene ,LoadSceneMode mode)
    {
        if( scene .buildIndex==1)
        {
            PhotonNetwork.Instantiate("PlayerController", Vector3.zero, Quaternion.identity,0);
        }
    }


}
