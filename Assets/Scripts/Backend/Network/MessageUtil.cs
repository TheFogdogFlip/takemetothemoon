using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

public delegate void MessagePromptDelegate(GameObject messagebutton);

public sealed class MessageBoxUtil
{
    public static void ShowMessageBox(string text,GameObject canvas, MessagePromptDelegate callback)
    {
        GameObject newObject = Resources.Load("Prefabs/MessagePrompt") as GameObject;
        newObject = (GameObject)GameObject.Instantiate(newObject);
        newObject.GetComponent<MessagePrompt>().Text.text = text;
        newObject.GetComponent<MessagePrompt>().transform.SetParent(canvas.transform);
        newObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 2, Screen.height / 2);
        newObject.transform.position = canvas.transform.position;
        newObject.transform.localScale = new Vector3(1, 1, 1);
        newObject.GetComponent<MessagePrompt>().SetCallback(callback);
    }
}

public sealed class MD5Hash
{
    public static string MD5ComputeHash(string value)
    {
        MD5 keyhasher = MD5.Create();
        byte[] hash = keyhasher.ComputeHash(System.Text.Encoding.UTF8.GetBytes(value));
        StringBuilder sBuilder = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sBuilder.Append(hash[i].ToString("x2"));
        }
        return sBuilder.ToString();
    }
}

public class NetworkData
{
    public double traveleddistance;
    public bool complete;
}