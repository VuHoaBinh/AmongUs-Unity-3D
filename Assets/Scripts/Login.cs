using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Login : MonoBehaviour
{
    public static Login Instance;

    [SerializeField]
    public GameObject LogInHolder;
    [SerializeField]
    public GameObject SignUpHolder;
    [SerializeField]
    public GameObject ForgotPasswordHolder;

     private void Awake()
    {
        CreateInstance();
    }

    private void CreateInstance()
    {
        if(Instance == null)
        {
            Instance = this;
        }
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
        LogInHolder.SetActive(true);
        SignUpHolder.SetActive(false);
        ForgotPasswordHolder.SetActive(false);
    }

}
