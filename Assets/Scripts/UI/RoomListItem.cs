using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text roomnametext;
    public RoomInfo roominfo;
    public void Setup(RoomInfo _roominfo)
    {
        roominfo = _roominfo;
        roomnametext.text = _roominfo.Name;
    }
    public void OnClick()
    {
        Laucher.instance.JoinRoom(roominfo);
    }

}
