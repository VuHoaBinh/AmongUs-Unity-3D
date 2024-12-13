using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WebSocketSharp;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class GameChat : MonoBehaviour
{
    public TextMeshProUGUI chatText;
    public TMP_InputField inputField;

    private bool isInputFieldToggle;

    void Update()
    {
        // Toggle input field bằng phím Y
        if (Input.GetKeyDown(KeyCode.Y) && !isInputFieldToggle)
        {

            isInputFieldToggle = true;
            inputField.Select();
            inputField.ActivateInputField();
            Debug.Log("Input field on");

        }

        if (Input.GetKeyDown(KeyCode.Escape) && isInputFieldToggle)
        {
            isInputFieldToggle = false;
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            Debug.Log("Input field off");
        }

        // Gửi tin nhắn
        if (Input.GetKeyDown(KeyCode.Return)
            && isInputFieldToggle
            && !inputField.text.IsNullOrEmpty())
        {
            string message = $"{PhotonNetwork.LocalPlayer.NickName}: {inputField.text}";
            GetComponent<PhotonView>().RPC("SendMessage", RpcTarget.All, message);

            inputField.text = "";
            isInputFieldToggle = false;
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            Debug.Log("Sent message");
        }

        if(Input.GetKeyDown(KeyCode.Q)){
            SceneManager.LoadScene("Option");
        }
    }



    [PunRPC]
    public void SendMessage(string _message)
    {
        chatText.text = chatText.text + "\n" + _message;
    }
}
