using UnityEngine;
using System.Collections;

public class LoginMenuScript : MonoBehaviour {

    public UnityEngine.UI.InputField Email;
    public UnityEngine.UI.InputField Password;
    public UnityEngine.UI.Toggle auto_login;

	// Use this for initialization
	void Start () {
	}
    public void LoginUser()
    {
        //Check all inputs for correct formats
        ApplicationModel.Instance.LoginUser(Email.text,MD5Hash.MD5ComputeHash(Password.text),auto_login.isOn);
    }
}