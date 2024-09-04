using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;

public class Usenamdisplay : MonoBehaviour
{

    
    public TMP_Text usernametext;
    public PhotonView view;
    public TMP_Text Teamtext;
    private void Start()
    {
        if(view.IsMine)
        {
            gameObject.SetActive(false);
        }
        usernametext.text = view.Owner.NickName;
        if(view.Owner.CustomProperties.ContainsKey("Team"))
        {
            int Team = (int)view.Owner.CustomProperties["Team"];
            Teamtext.text = "Team" + Team;
        }
    }
}
