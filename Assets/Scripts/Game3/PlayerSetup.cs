using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using ExitGames.Client.Photon; // Thêm dòng này để sử dụng Hashtable
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class PlayerSetup : MonoBehaviour
{
    public Movement movement;
    public GameObject camera;
    public string nickName;
    public TextMeshPro nicknameText;

    public void IsLocalPlayer()
    {
        movement.enabled = true;
        camera.SetActive(true);
    }

    [PunRPC]
    public void SetNickName(string _name)
    {
        nickName = _name;
        nicknameText.text = nickName;
    }

    public void SetHashes()
    {
        try
        {
            if (RoomManager.instance != null)
            {
                Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties;
                hash["kills"] = RoomManager.instance.kills;
                hash["deaths"] = RoomManager.instance.deaths;

                PhotonNetwork.LocalPlayer.SetCustomProperties(hash); // Sửa lỗi cú pháp
            }
            else
            {
                Debug.LogError("RoomManager.instance is null!");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error setting custom properties: " + ex.Message);
        }
    }
}