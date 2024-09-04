using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class Playerlistitem : MonoBehaviourPunCallbacks
{
    Player player;
    public TMP_Text playername;
    public TMP_Text TeamText;
    int team;
    public void Setup(Player _player ,int _team)

    {
        player = _player;
        team = _team;
        playername.text = _player.NickName;
        TeamText.text = "Team" + _team;
        ExitGames.Client.Photon.Hashtable customprops=new ExitGames.Client.Photon.Hashtable();
        customprops["Team"] = _team;
        _player.SetCustomProperties(customprops);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
      if(player==otherPlayer)
        {
            Destroy(gameObject);
        }
    }
    public override void OnLeftRoom()
    {
        Destroy(gameObject) ;
    }
}
