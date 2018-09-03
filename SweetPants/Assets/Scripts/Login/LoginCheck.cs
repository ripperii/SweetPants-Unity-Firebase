using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginCheck : MonoBehaviour {

    void Awake()
    {

        if(Login.login == null)
        {
            SceneManager.LoadScene(0);
        }
    }
}
