using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public GameObject player; 
    [Space]
    public Transform spawnerPoint; 

    void Start()
    {
        Debug.Log("Connecting ...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to Server successfully - Binh dep trai");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined Lobby!");

        // // Tạo hoặc tham gia phòng
        // RoomOptions roomOptions = new RoomOptions();
        // roomOptions.MaxPlayers = 4; // Giới hạn số người chơi trong phòng
        // PhotonNetwork.JoinOrCreateRoom("test", roomOptions, TypedLobby.Default);
        PhotonNetwork.JoinOrCreateRoom("test", null, null);

    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined Room Successfully!");

        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnerPoint.position, Quaternion.identity);
        _player.GetComponent<PlayerSetup>().IsLocalPlayer();
    }
}
 