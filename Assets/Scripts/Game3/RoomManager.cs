using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Firebase;
using Firebase.Auth;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;
    public GameObject player;
    [Space]
    public Transform[] spawnerPoints;

    private bool playerSpawned = false; // Add this flag
    [Space]
    public GameObject roomCam;

    [Space]
    public GameObject nameUI;
    public GameObject connectingUI;

    private string nickname = "BinhTom";
    private FirebaseManager firebaseManager;

    [HideInInspector]
    public int kills = 0;
    [HideInInspector]
    public int deaths = 0;
    public string roomNameToJoin = "test";
    void Awake()
    {
        instance = this;
    }

    public void changeNickName(string _name)
    {
        nickname = _name;
    }

    public void JoinRoomButtonPressed()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.JoinOrCreateRoom(roomNameToJoin, null, null);

        nameUI.SetActive(false);
        connectingUI.SetActive(true);
    }
    // void Start()
    // {
    //     Debug.Log("Connecting ...");
    //     PhotonNetwork.ConnectUsingSettings();
    //     nickname = PlayerPrefs.GetString("PlayerName", nickname); // Lấy giá trị name đã lưu trữ
    //     Debug.Log("Nickname loaded: " + nickname); // Thêm dòng này để kiểm tra
    //     firebaseManager = FindObjectOfType<FirebaseManager>();
    //     if (firebaseManager != null && firebaseManager.user != null)
    //     {
    //         nickname = firebaseManager.user.DisplayName; // Lấy giá trị name từ FirebaseManager
    //         Debug.Log("Nickname loaded: " + nickname); // Thêm dòng này để kiểm tra
    //         PhotonNetwork.JoinOrCreateRoom(roomNameToJoin, null, null);
    //     }
    //     else
    //     {
    //         Debug.LogError("FirebaseManager or user is not initialized!");
    //     }
    // }

    // public override void OnConnectedToMaster()
    // {
    //     base.OnConnectedToMaster();
    //     Debug.Log("Connected to Server successfully - Binh dep trai");
    //     PhotonNetwork.JoinLobby();
    // }

    // public override void OnJoinedLobby()
    // {
    //     base.OnJoinedLobby();
    //     Debug.Log("Joined Lobby!");
    //     PhotonNetwork.JoinOrCreateRoom(roomNameToJoin, null, null);
    // }

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
        Transform spawnerPoint = spawnerPoints[UnityEngine.Random.Range(0, spawnerPoints.Length)];

        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnerPoint.position, Quaternion.identity);
        _player.GetComponent<PlayerSetup>().IsLocalPlayer();
        _player.GetComponent<Health>().IsLocalPlayer = true;
        _player.GetComponent<PhotonView>().RPC("SetNickName", RpcTarget.AllBuffered, nickname);
        PhotonNetwork.LocalPlayer.NickName = nickname;

        playerSpawned = true;

    }

    public void SetHashes()
    {
        try
        {
            Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties;
            hash["kills"] = kills;
            hash["deaths"] = deaths;

            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        }
        catch
        {

        }
    }
}