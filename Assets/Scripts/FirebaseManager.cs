using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase;
using Firebase.Auth;

public class FirebaseManager : MonoBehaviour
{
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;

    [Header("Registration")]
    public TMP_InputField nameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField confirmPasswordRegisterField;


    [Header("Layers")]
    public GameObject LogInHolder;
    public GameObject SignUpHolder;
    public GameObject ForgotPasswordHolder;

    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            user = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }

    public void Login()
    {
        StartCoroutine(LoginAsync(emailLoginField.text, passwordLoginField.text));
    }

    private IEnumerator LoginAsync(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            if (loginTask.Exception.GetBaseException() is FirebaseException firebaseException)
            {
                string failedMessage = "Lỗi nè: Login Failed! Because ";
                AuthError authError = (AuthError)firebaseException.ErrorCode;
                switch (authError)
                {
                    case AuthError.InvalidEmail: failedMessage += "Lỗi nè: Email is invalid"; break;
                    case AuthError.WrongPassword: failedMessage += "Lỗi nè: Wrong Password"; break;
                    case AuthError.MissingEmail: failedMessage += "Lỗi nè: Email is missing"; break;
                    case AuthError.MissingPassword: failedMessage += "Lỗi nè: Password is missing"; break;
                    default: failedMessage = "Lỗi nè: Login Failed"; break;
                }
                Debug.Log(failedMessage);
            }
        }
        else
        {
            user = loginTask.Result.User;
            Debug.LogFormat("{0} You Are Successfully Logged In", user.DisplayName);
            SceneManager.LoadScene("Menu");
        }
    }

    public void Register()
    {
        StartCoroutine(RegisterAsync(nameRegisterField.text, emailRegisterField.text, passwordRegisterField.text, confirmPasswordRegisterField.text));
    }

    private IEnumerator RegisterAsync(string name, string email, string password, string confirmPassword)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || password != confirmPassword)
        {
            Debug.LogError("Lỗi nè: Invalid input fields");
            yield break;
        }

        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.Exception != null)
        {
            if (registerTask.Exception.GetBaseException() is FirebaseException firebaseException)
            {
                string failedMessage = "Lỗi nè: Registration Failed! Because ";
                AuthError authError = (AuthError)firebaseException.ErrorCode;
                switch (authError)
                {
                    case AuthError.InvalidEmail: failedMessage += "Lỗi nè: Email is invalid"; break;
                    case AuthError.WrongPassword: failedMessage += "Lỗi nè: Wrong Password"; break;
                    case AuthError.MissingEmail: failedMessage += "Lỗi nè: Email is missing"; break;
                    case AuthError.MissingPassword: failedMessage += "Lỗi nè: Password is missing"; break;
                    default: failedMessage = "Lỗi nè: Registration Failed"; break;
                }
                Debug.Log(failedMessage);
            }
        }
        else
        {
            user = registerTask.Result.User;
            UserProfile userProfile = new UserProfile { DisplayName = name };
            var updateProfileTask = user.UpdateUserProfileAsync(userProfile);
            yield return new WaitUntil(() => updateProfileTask.IsCompleted);

            if (updateProfileTask.Exception != null)
            {
                user.DeleteAsync();
                Debug.LogError(updateProfileTask.Exception);
            }
            else
            {
                Debug.Log("Registration Successful! Welcome " + user.DisplayName);
                LogInHolder.SetActive(true);
                SignUpHolder.SetActive(false);
                ForgotPasswordHolder.SetActive(false);
            }
        }
    }
}
