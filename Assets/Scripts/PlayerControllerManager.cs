using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;
using UnityEngine.UI;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class PlayerControllerManager : MonoBehaviourPunCallbacks
{
    GameObject controller;
    PhotonView view;
    private Dictionary<int, int> PlayerTeams = new Dictionary<int, int>();
    public int PlayerTeam;
    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }
    private void Start()
    {
        if(view.IsMine)
        {
            CreateController();
        }
    }
    void CreateController()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
        {
            PlayerTeam = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
            Debug.Log("Player's Team : " + PlayerTeams);
            AssignPlayerToSpawnArea(PlayerTeam);
        }
        //controller = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0, new object[] { view.ViewID });
    }
    public void AssignPlayerToSpawnArea(int team)
    {
        GameObject spawnpoint1 = GameObject.Find("Spawnpoint1");
        GameObject spawnpoint2 = GameObject.Find("Spawnpoint2");
        if(spawnpoint1==null|| spawnpoint2==null)
        {
            Debug.Log("spawn area not found");
            return;
        }
        Transform spawnpoint = null;
        if(team==1)
        {
            spawnpoint = spawnpoint1.transform.GetChild(Random.Range(0, spawnpoint1.transform.childCount));

        }
        if(team==2)
        {

            spawnpoint=spawnpoint2.transform.GetChild(Random.Range(0,spawnpoint2.transform.childCount));
        }
        if (spawnpoint!=null)
        {
        controller = PhotonNetwork.Instantiate("Player", spawnpoint.position, spawnpoint.rotation, 0, new object[] { view.ViewID });
            
        }
        else
        {
            Debug.Log("No available spownpoint" + team);
        }
    }
    public void Die()
    {
        PhotonNetwork.Destroy(controller);

        CreateController();
    }
    void AssignTeamToAllPlayers()
    {
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.ContainsKey("Team"))
            {
                int Team = (int)player.CustomProperties["Team"];
                PlayerTeams[player.ActorNumber] = Team;
                Debug.Log(player.NickName + "'s:" + Team);
                AssignPlayerToSpawnArea(Team);

            }
        }
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        AssignTeamToAllPlayers();
    }


}
