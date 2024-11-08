using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;

public class FirebaseManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseUser user;

    // Initialize Firebase
    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            if (task.Result == DependencyStatus.Available)
            {
                // Firebase is ready to be used
                auth = FirebaseAuth.DefaultInstance;
                Debug.Log("Firebase initialized with config from google-services.json or GoogleService-Info.plist");
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });
    }

    // Login Method
    public void Login(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCompleted && !task.IsFaulted)
            {
                AuthResult authResult = task.Result;
                user = authResult.User;
                Debug.Log("User logged in successfully: " + user.Email);
            }
            else
            {
                Debug.LogError("Login failed: " + task.Exception);
            }
        });
    }

    // Register Method with Username
    public void Register(string email, string password, string username)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCompleted && !task.IsFaulted)
            {
                AuthResult authResult = task.Result;
                user = authResult.User;
                Debug.Log("User registered successfully: " + user.Email);

                // Update the profile with the username
                UpdateProfile(username);
            }
            else
            {
                Debug.LogError("Registration failed: " + task.Exception);
            }
        });
    }

    // Update Profile Method to Set Display Name (Username)
    public void UpdateProfile(string displayName)
    {
        if (user != null)
        {
            UserProfile profile = new UserProfile { DisplayName = displayName };
            user.UpdateUserProfileAsync(profile).ContinueWith(task => {
                if (task.IsCompleted && !task.IsFaulted)
                {
                    Debug.Log("Profile updated with username: " + user.DisplayName);
                }
                else
                {
                    Debug.LogError("Failed to update profile: " + task.Exception);
                }
            });
        }
    }

    // Logout Method
    public void Logout()
    {
        auth.SignOut();
        Debug.Log("User logged out successfully.");
        user = null;
    }
}
