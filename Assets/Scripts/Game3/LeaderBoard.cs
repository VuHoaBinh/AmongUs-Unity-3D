using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;

public class LeaderBoard : MonoBehaviour
{
    public GameObject playersHolder;
    [Header("Options")]

    public float refreshRate = 1f;
    [Header("UI")]

    public GameObject[] slots;
    [Space]

    public TextMeshProUGUI[] scoresText;
    public TextMeshProUGUI[] nameText;
    public TextMeshProUGUI[] kdText;


    private void Start()
    {
        InvokeRepeating(nameof(Refresh), 1f, refreshRate);
    }

    public void Refresh()
    {
        foreach (var slot in slots)
        {
            slot.SetActive(false); // Sửa lỗi phương thức
        }

        var sortedPlayerList = (from player in PhotonNetwork.PlayerList orderby player.GetScore() descending select player).ToList();
        // var sortedPlayerList = "0";

        int i = 0;
        foreach (var player in sortedPlayerList)
        {
            if (i >= slots.Length || i >= nameText.Length || i >= scoresText.Length)
            {
                break; // Tránh lỗi vượt quá giới hạn mảng
            }

            slots[i].SetActive(true); // Sửa lỗi phương thức
            if (string.IsNullOrEmpty(player.NickName))
            {
                player.NickName = "BinhTom";
            }

            nameText[i].text = player.NickName; // Sửa lỗi chính tả
            scoresText[i].text = player.GetScore().ToString(); // Sửa lỗi chính tả

            if(player.CustomProperties["kills"] != null){
                kdText[i].text = player.CustomProperties["Kills"] + "/" + player.CustomProperties["deaths"];
            }else{
                kdText[i].text = "0/0";

            }
            i++;
        }
    }

    private void Update()
    {
        playersHolder.SetActive(Input.GetKey(KeyCode.Tab));
    }
}