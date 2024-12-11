using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime; // Cần thiết để sử dụng RoomOptions và TypedLobby

public class RoomManager : MonoBehaviourPunCallbacks
{
    public GameObject player; // Prefab nhân vật
    [Space]
    public Transform spawnerPoint; // Vị trí spawn nhân vật

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connecting ...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to Server successfully - Binh dep trai");

        // Tham gia lobby sau khi kết nối thành công
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined Lobby!");

        // Tạo hoặc tham gia phòng
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4; // Giới hạn số người chơi trong phòng
        PhotonNetwork.JoinOrCreateRoom("test", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined Room Successfully!");

        // Spawn nhân vật khi vào phòng
        if (player != null && spawnerPoint != null)
        {
            PhotonNetwork.Instantiate(player.name, spawnerPoint.position, Quaternion.identity);
            Debug.Log("Player spawned successfully!");
        }
        else
        {
            Debug.LogError("Player prefab or spawn point is not set!");
        }
    }
}
