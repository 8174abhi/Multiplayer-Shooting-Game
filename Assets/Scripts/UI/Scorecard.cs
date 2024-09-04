using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Scorecard : MonoBehaviour
{
    public static Scorecard Instance;   
    public TMP_Text blueteamtext;
    public TMP_Text redteamtext;
    public int blueteamscore = 0;
    public int redteamscore = 0;
    PhotonView pv;
    private void Awake()
    {
        Instance = this;
        pv = GetComponent<PhotonView>();
    }
    public void Playerdied(int playerteam)
    {
        if(playerteam==2)
        {
            blueteamscore++;

        }
        if(playerteam==1) 
        {
        redteamscore++;
        }
        pv.RPC("UpdateScore",RpcTarget.All,blueteamscore,redteamscore);
    }
    [PunRPC]
    void UpdateScore(int bluescore, int redscore)
    {
        blueteamscore = bluescore;
        redteamscore = redscore;
        blueteamtext.text=blueteamscore.ToString();
        redteamtext.text = redteamscore.ToString();
    }

}
