// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Photon.Pun;
// using Photon.Realtime;

// public class RoomManager : MonoBehaviourPunCallbacks
// {
//     public static RoomManager instance;
//     public GameObject player; 
//     [Space]
//     public Transform spawnerPoint; 

//     // [Space]
//     // public GameObject roomCam;

//     void Awake(){
//         instance = this;
//     }
//     void Start()
//     {
//         Debug.Log("Connecting ...");
//         PhotonNetwork.ConnectUsingSettings();
//     }

//     public override void OnConnectedToMaster()
//     {
//         base.OnConnectedToMaster();
//         Debug.Log("Connected to Server successfully - Binh dep trai");
//         PhotonNetwork.JoinLobby();
//     }

//     public override void OnJoinedLobby()
//     {
//         base.OnJoinedLobby();
//         Debug.Log("Joined Lobby!");

//         // // Tạo hoặc tham gia phòng
//         // RoomOptions roomOptions = new RoomOptions();
//         // roomOptions.MaxPlayers = 4; // Giới hạn số người chơi trong phòng
//         // PhotonNetwork.JoinOrCreateRoom("test", roomOptions, TypedLobby.Default);
//         PhotonNetwork.JoinOrCreateRoom("test", null, null);

//     }

//     public override void OnJoinedRoom()
//     {
//         base.OnJoinedRoom();
//         Debug.Log("Joined Room Successfully!");

//         // roomCam.SetActive(false);
//         // RespawnPlayer();
//         GameObject _player = PhotonNetwork.Instantiate(player.name, spawnerPoint.position, Quaternion.identity);
//         _player.GetComponent<PlayerSetup>().IsLocalPlayer();
//         _player.GetComponent<Health>().IsLocalPlayer = true;

//     }

//     public void RespawnPlayer(){
//         GameObject _player = PhotonNetwork.Instantiate(player.name, spawnerPoint.position, Quaternion.identity);
//         _player.GetComponent<PlayerSetup>().IsLocalPlayer();
//         _player.GetComponent<Health>().IsLocalPlayer = true;
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;
    public GameObject player;
    [Space]
    public Transform spawnerPoint;

    private bool playerSpawned = false; // Add this flag

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Ensure the RoomManager persists across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
        PhotonNetwork.JoinOrCreateRoom("test", null, null);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined Room Successfully!");
        RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        if (!playerSpawned) 
        {
            Debug.Log("Spawning player...");
            GameObject _player = PhotonNetwork.Instantiate(player.name, spawnerPoint.position, Quaternion.identity);
            _player.GetComponent<PlayerSetup>().IsLocalPlayer();
            _player.GetComponent<Health>().IsLocalPlayer = true;
            playerSpawned = true; 
        }
        else
        {
            Debug.Log("Player already spawned, not spawning again.");
        }
    }
}