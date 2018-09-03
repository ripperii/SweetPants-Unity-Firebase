using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;
using System;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour {

    public static Login login = null;
    public FirebaseAuth auth;
    FirebaseUser user;
    Player player;

    public delegate IEnumerator AuthCallback(Task<FirebaseUser> task, string operation);
    public event AuthCallback authCallback;

    // Use this for initialization
    public void LoginFunction ()
    {

        Debug.Log("Login method called!");
        
        Debug.Log("IsEditor: " + Application.isEditor);
        Debug.Log(FirebaseAuth.DefaultInstance);

        auth = /*Application.isEditor ?*/ FirebaseAuth.GetAuth(DatabaseManager.app) /* : FirebaseAuth.DefaultInstance*/;

        auth.StateChanged += AuthStateEvent;
        AuthStateEvent(this, null);

        Debug.Log(Application.persistentDataPath);

        Debug.Log("App name: " + auth.App.Name);

        if (File.Exists(Application.persistentDataPath + "/SaveData.dat"))
        {
            string password, error;
            if (SaveFile.LoadFromFile(out password, out error))
            {
                Debug.Log("Email: " + SystemInfo.deviceUniqueIdentifier + "@" + SystemInfo.deviceType + ".com" + " Password: " + password);

                auth.SignInWithEmailAndPasswordAsync(SystemInfo.deviceUniqueIdentifier + "@" + SystemInfo.deviceType + ".com", password).ContinueWith(task => {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                        return;
                    }

                    // Firebase user has been created.
                    user = task.Result;
                    
                    Debug.LogFormat("User logged in successfully: {0} ({1})", user.DisplayName, user.UserId);

                    StartCoroutine(authCallback(task, "Log_In"));

                });
            }
            else
            {
                Debug.Log(error);
            }
        }
        else
        {
            Guid newPass = Guid.NewGuid();
            Debug.Log("Does it reach here?");
            
            auth.CreateUserWithEmailAndPasswordAsync(SystemInfo.deviceUniqueIdentifier + "@" + SystemInfo.deviceType + ".com", newPass.ToString()).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }
                
                user = task.Result;
                string error;
                SaveFile.SaveToFile(newPass.ToString(), out error);

                Debug.LogFormat("Firebase user created successfully: {0} ({1})", user.DisplayName, user.UserId);

                StartCoroutine(authCallback(task, "Sign_Up"));
            });
        }
    }
    private void CheckUserName()
    {
        Debug.Log("UserId: " + user.UserId);

        if (null == player)
        {
            player = new Player("", DateTime.Now, 0, 0, 0);

            DatabaseManager.sharedInstance.CreateNewPlayer(player, user.UserId);
        }
        Debug.Log("Username: " + player.Username);

        if (player.Username == "" && SceneManager.GetActiveScene().buildIndex == 0)
        {
            GameObject.Find("NamePopUp").GetComponent<EnterNameScript>().OpenUserNameInputMenu();
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }
    
    void Awake()
    {
        Application.runInBackground = true;
        authCallback += HandleAuthCallback;
        login = this;
        gameObject.AddComponent<http>();
    }
    private void Start()
    {
        Debug.Log("Login Started!");
        LoginFunction();
    }

    IEnumerator HandleAuthCallback (Task<FirebaseUser> task, string operation)
    {
        yield return new WaitForSeconds(1.5f);

        DatabaseManager.user = auth.CurrentUser;


        Debug.Log("HandleAuthCallback Event Called!");

        DatabaseManager.sharedInstance.GetAllItems(items =>
        {
            Items.items = items;
            DatabaseManager.sharedInstance.GetPlayer(result => 
            { 
                Player.currentPlayer = player = result;
                
                CheckUserName();
            });
        });
    }

    private void OnDestroy()
    {
        authCallback -= HandleAuthCallback;
    }
    private void AuthStateEvent(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }

}
