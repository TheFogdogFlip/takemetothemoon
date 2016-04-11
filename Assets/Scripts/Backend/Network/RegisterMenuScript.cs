using UnityEngine;
using System.Collections;

public class RegisterMenuScript : MonoBehaviour {

    public UnityEngine.UI.InputField Email;
    public UnityEngine.UI.InputField Username;
    public UnityEngine.UI.InputField Password;
	// Use this for initialization
	void Start () {
	}


    public void RegisterUser()
    {
        //Check all inputs for correct formats
        ApplicationModel.Instance.RegisterUser(Email.text,Username.text,Password.text);
    }
}
