using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RSG;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

public class http : MonoBehaviour {

    public static http sharedInstance = null;

    void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else if (sharedInstance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    public IPromise<string> Get(string url)
    {
        var promise = new Promise<string>();
        
        DatabaseManager.user.TokenAsync(true).ContinueWith(token =>
        {
            string Token = token.Result;
            StartCoroutine(_Get(promise, url, Token));
        });
        return promise;
    }
    public IPromise<string> Post(string url, string data)
    {
        var promise = new Promise<string>();

        DatabaseManager.user.TokenAsync(true).ContinueWith(token =>
        {
            string Token = token.Result;
            StartCoroutine(_Post(promise, url, data, Token));
        });
        return promise;
    }
    private IEnumerator _Get(Promise<string> promise, string url, string Token)
    {
        var request = UnityWebRequest.Get(url);
        
        request.SetRequestHeader("Authorization", "Bearer " + Token);

        yield return request.SendWebRequest();

        if (request.isNetworkError) // something went wrong
        {
            promise.Reject(new Exception(request.error));
        }
        else if (request.responseCode != 200) // or the response is not OK
        {
            promise.Reject(new Exception(request.downloadHandler.text));
        }
        else
        {
            // Format output and resolve promise
            Debug.Log("String from HTTP Get: " + request.downloadHandler.text);
            var tasks = request.downloadHandler.text;

            promise.Resolve(tasks);
        }
    }
    private IEnumerator _Post(Promise<string> promise, string url, string data, string Token)
    {
        var request = UnityWebRequest.Post(url, data);

        Debug.Log(data);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        request.SetRequestHeader("Authorization", "Bearer " + Token);

        yield return request.SendWebRequest();

        if (request.isNetworkError) // something went wrong
        {
            promise.Reject(new Exception(request.error));
        }
        else if (request.responseCode != 200) // or the response is not OK
        {
            promise.Reject(new Exception(request.downloadHandler.text));
        }
        else
        {
            // Format output and resolve promise
            Debug.Log("String from HTTP Post: " + request.downloadHandler.text);
            var tasks = request.downloadHandler.text;

            promise.Resolve(tasks);
        }
    }}

