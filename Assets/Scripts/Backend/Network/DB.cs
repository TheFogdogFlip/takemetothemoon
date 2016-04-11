using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
public class DB : MonoBehaviour {
	// Use this for initialization
    public static DB databasecontroller;

     NetworkHandler requesthandler;
    void Awake()
     {
         if (databasecontroller == null)
         {
             DontDestroyOnLoad(this.gameObject);
             databasecontroller = this;
         }
         else if (databasecontroller != this)
         {
             Destroy(this.gameObject);
         }
     }

	 void Start(){
         requesthandler = new NetworkHandler();
         DontDestroyOnLoad(this.gameObject);
         ApplicationModel.Instance.Loginmanager.db = this.gameObject;
     }

     public void GET(string url,System.Action<WWW> onSuccess)
     {
       StartCoroutine(requesthandler.HandleGetRequest(url,onSuccess));
     }
 
     public void POST(string url, Dictionary<string,string> post,System.Action<WWW> onSuccess)
     {
         StartCoroutine(requesthandler.HandlePostRequest(url, post, onSuccess));
     }
 
     private IEnumerator WaitForRequest(WWW www)
     {
         yield return www;
 
         // check for errors
         if (www.error == null)
         {
             Debug.Log("WWW Ok!: " + www.text);
         } else {
             Debug.Log("WWW Error: "+ www.error);
         }    
     }
}


public class NetworkHandler
{
    public IEnumerator HandlePostRequest(string url,Dictionary<string,string> data,System.Action<WWW> onSuccess)
    {
        WWWForm form = new WWWForm();
        foreach (KeyValuePair<String, String> post_arg in data)
        {
            form.AddField(post_arg.Key, post_arg.Value);
        }

        WWW www = new WWW(url,form);
        Debug.Log("SENDING DATA");
        yield return www;
        //if (string.IsNullOrEmpty(www.error))
       // {
            //Debug.Log("WWW Ok!: " + www.text);
            onSuccess(www);
       // }
        //else
        //{
         //   Debug.Log("WWW Error: " + www.error);
            //Debug.LogWarning("\n WWW request returned an error.");
        //}
    }

    public IEnumerator HandleGetRequest(string url, System.Action<WWW> onSuccess)
    {
        WWW www = new WWW(url);
        yield return www;
        //if (string.IsNullOrEmpty(www.error))
        //{
            onSuccess(www);
        //}
        //else
        //{
        //    Debug.LogWarning("WWW request returned an error.");
        //}
    }
}
