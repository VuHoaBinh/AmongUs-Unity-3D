using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
public class PlayerSetup : MonoBehaviour
{
    public Movement movement;

    public GameObject camera;

    public string nickName;

    public TextMeshPro nicknameText;

    public void IsLocalPlayer(){
        movement.enabled =true;
        camera.SetActive(true);
    }

    [PunRPC]

    public void SetNickName(string _name){
        nickName = _name;
        nicknameText.text = nickName;
    }
}
