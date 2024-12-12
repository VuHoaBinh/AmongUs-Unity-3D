using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomList : MonoBehaviourPunCallbacks
{

    public static RoomList Instance;
    public GameObject roomManagerGameObject;
    public RoomManager roomManager;


    [Header("UI")]
    public Transform roomListParent;
    public GameObject roomListItemPrefab;

    private List<RoomInfo> cachedRoomList = new List<RoomInfo>();

    public void ChangeRoomToCreateName(string _nameRoom){
        
        roomManager.roomNameToJoin = _nameRoom;
        Debug.Log("1" + _nameRoom);
        Debug.Log("1" + roomManager.roomNameToJoin);

    }
    private void Awake(){
        Instance = this;
    }
    IEnumerator Start()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }
        yield return new WaitUntil(() => !PhotonNetwork.IsConnected);

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var room in roomList)
        {
            int index = cachedRoomList.FindIndex(r => r.Name == room.Name);
            if (room.RemovedFromList)
            {
                if (index != -1)
                    cachedRoomList.RemoveAt(index);
            }
            else
            {
                if (index == -1)
                    cachedRoomList.Add(room); // Thêm phòng mới
                else
                    cachedRoomList[index] = room; // Cập nhật phòng hiện tại
            }
        }

        UpdateUI(); // Cập nhật giao diện sau mỗi thay đổi
    }

    void UpdateUI()
    {
        foreach (Transform roomItem in roomListParent)
        {
            Destroy(roomItem.gameObject);
        }

        foreach (var room in cachedRoomList)
        {
            GameObject roomItem = Instantiate(roomListItemPrefab, roomListParent);
            roomItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = room.Name;
            roomItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{room.PlayerCount} / 10";


            roomItem.GetComponent<RoomItem>().RoomName = room.Name;
        }
    }

    public void JoinRoomByName(string _name){
        // roomManager.roomNameToJoin = _name;
        roomManagerGameObject.SetActive(true);
        gameObject.SetActive(false);

        Debug.Log("2: " + roomManager.roomNameToJoin);
    }
}
