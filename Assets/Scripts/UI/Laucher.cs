using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using System.Linq;



public class Laucher : MonoBehaviourPunCallbacks
{
   public static Laucher instance;  
    [SerializeField] TMP_InputField RoomnameInputfield;
    [SerializeField] TMP_Text ErorText;
    [SerializeField] TMP_Text RoomNameText;
    [SerializeField] Transform roomlistcontent;
    [SerializeField] GameObject roomlistitemprefab;
    [SerializeField] Transform playerlistcontent;
    [SerializeField] GameObject playerlistprefab;
    public GameObject Startbtn;
    int nextTeamNo = 1;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting To master");
        PhotonNetwork.AutomaticallySyncScene=true;
    }
    
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master");
        PhotonNetwork.JoinLobby();
       
    }
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(RoomnameInputfield.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(RoomnameInputfield.text);
        MenuManager.instance.OpenMenu("LoadingMenu");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.instance.OpenMenu("LoadingMenu");

    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.OpenMenu("LoadingMenu");

    }
    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Game");
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        MenuManager.instance.OpenMenu("TitleMenu");
        PhotonNetwork.NickName = "Player" + Random.Range(0, 1000).ToString();

    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Startbtn.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnJoinedRoom()
    {
        MenuManager.instance.OpenMenu("RoomMenu");
        RoomNameText.text = PhotonNetwork.CurrentRoom.Name;
        Player[] players = PhotonNetwork.PlayerList;
        foreach(Transform child in playerlistcontent)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < players.Count(); i++)
        {
            int teamNumber = GetNextTeamNumber();

            
            Instantiate(playerlistprefab, playerlistcontent).GetComponent<Playerlistitem>().Setup(players[i],teamNumber);
        }
        Startbtn.SetActive(PhotonNetwork.IsMasterClient);
    }
 
    public override void OnCreateRoomFailed(short returnCode, string Erormessage)
    {
        MenuManager.instance.OpenMenu("ErorMenu");
        ErorText.text = "Room Creation Failed" + Erormessage;

    }
    public override void OnLeftRoom()
    {
        MenuManager.instance.OpenMenu("TitleMenu");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform trans in roomlistcontent)
        {
            Destroy(trans.gameObject);
        }
      for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomlistitemprefab, roomlistcontent).GetComponent<RoomListItem>().Setup(roomList[i]);
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        int teamNuber = GetNextTeamNumber();
        GameObject playeritem = Instantiate(playerlistprefab, playerlistcontent);
        playeritem.GetComponent<Playerlistitem>().Setup(newPlayer,teamNuber);
    }
    private int GetNextTeamNumber()
    {
        int teamNumber = nextTeamNo;
        nextTeamNo = 3 - nextTeamNo;
        return teamNumber;
    }
}
