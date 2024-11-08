using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class Login : MonoBehaviour
{
    public FirebaseManager firebaseManager;

    public GameObject LogInHolder;
    public GameObject SignUpHolder;
    public GameObject ForgotPasswordHolder;

    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TMP_InputField usernameInputField;

    public void GotoMenu(){
        string email = emailLoginField.text;
        string password = passwordLoginField.text;
        firebaseManager.Login(email, password);
         Debug.Log("Login");
        SceneManager.LoadScene("Menu");
    }

    public void GotoSignUp(){
        LogInHolder.SetActive(false);
        SignUpHolder.SetActive(true);
        ForgotPasswordHolder.SetActive(false);
    }

    public void GotoForgotPassword(){
        LogInHolder.SetActive(false);
        SignUpHolder.SetActive(false);
        ForgotPasswordHolder.SetActive(true);
    }

    public void ClickBack(){
        LogInHolder.SetActive(true);
        SignUpHolder.SetActive(false);
        ForgotPasswordHolder.SetActive(false);
    }


    public void ClickSignUp(){
        string email = emailInputField.text;
        string password = passwordInputField.text;
        string username = usernameInputField.text;
        firebaseManager.Register(email, password,username);
        Debug.Log("Register");
        LogInHolder.SetActive(true);
        SignUpHolder.SetActive(false);
        ForgotPasswordHolder.SetActive(false);
    }
    public void OnLogoutButtonClicked()
    {
        firebaseManager.Logout();
    }
}
