using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Firebase;
using Firebase.Auth;
public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;
    public GameObject player;
    [Space]
    public Transform spawnerPoint;

    private bool playerSpawned = false; // Add this flag
    [Space]
    public GameObject roomCam;

    [Space]
    public GameObject nameUI;
    public GameObject connectingUI;

    private string nickname = "BinhTom";
    private FirebaseManager firebaseManager;
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
        nickname = PlayerPrefs.GetString("PlayerName", nickname); // Lấy giá trị name đã lưu trữ
        Debug.Log("Nickname loaded: " + nickname); // Thêm dòng này để kiểm tra
        firebaseManager = FindObjectOfType<FirebaseManager>();
        if (firebaseManager != null && firebaseManager.user != null)
        {
            nickname = firebaseManager.user.DisplayName; // Lấy giá trị name từ FirebaseManager
            Debug.Log("Nickname loaded: " + nickname); // Thêm dòng này để kiểm tra
        }
        else
        {
            Debug.LogError("FirebaseManager or user is not initialized!");
        }
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

        if (roomCam != null)
        {
            Debug.Log("Deactivating roomCam");
            roomCam.SetActive(false); // Corrected method call
        }
        else
        {
            Debug.LogError("roomCam is not assigned!");
        }

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
            _player.GetComponent<PhotonView>().RPC("SetNickName", RpcTarget.All, nickname);

            playerSpawned = true;
        }
        else
        {
            Debug.Log("Player already spawned, not spawning again.");
        }
    }
}