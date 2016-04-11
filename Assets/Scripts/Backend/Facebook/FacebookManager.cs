using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class FacebookManager : MonoBehaviour {

    public GameObject UIFBIsLoggedIn;
    public GameObject UIFBIsNotLoggedIn;
    public GameObject UIFBAvatar;
    public GameObject UIFBUserName;
    public GameObject UIFBScoreEntryPanel;
    public GameObject UIFBScoreList;

    private List<object> scoresList = null;

    private Dictionary<string, string> profile = null;
    void Awake()
    {
       //FB.Init(SetInit, OnHideUnity);
       FB.Init(SetInit);
    }

    private void SetInit()
    {
        Debug.Log("FB Init Done");
        //if (FB.IsLoggedIn)
        //{
        //    Debug.Log("FB Logged In");
        //    HandleFBMenus(true);
        //}
        //else
        //{
        //    HandleFBMenus(false);
        //}
    }
    private void OnHideUnity(bool isGameShown)
    {
        if (isGameShown)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }
    public void FBlogin()
    {
        FB.Login("email", AuthCallback);
        GameObject.Find("Login_Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
        GameObject.Find("FBLogin_Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
    }
    void AuthCallback(FBResult result)
    {
        if (FB.IsLoggedIn)
        {
           Debug.Log("FB Login Worked");
        }
        else
        {
            Debug.Log("FB Login Fail");
        }
    }
    void HandleFBMenus(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            UIFBIsLoggedIn.SetActive(true);
            UIFBIsNotLoggedIn.SetActive(false);
            // Get Profile picture
            FB.API(FacebookUtil.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, HandleProfilePicture);
            // Get User Name
            FB.API("/me?fields=id,first_name,middle_name,last_name", Facebook.HttpMethod.GET, HandleUserName);
        }
        else
        {
            UIFBIsLoggedIn.SetActive(false);
            UIFBIsNotLoggedIn.SetActive(true);
        }
    }

    public void SetProfilePicture()
    {
        FB.API(FacebookUtil.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, HandleProfilePicture);
        // Get User Name
        FB.API("/me?fields=id,first_name,middle_name,last_name", Facebook.HttpMethod.GET, HandleUserName);
    }

    void HandleProfilePicture(FBResult result)
    {
        // Check for errors
        if (result.Error != null)
        {
            // If problem occur
            Debug.Log("Problem with getting the profile picture");
            FB.API(FacebookUtil.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, HandleProfilePicture);
            return;
        }
        Image UserAvatar = UIFBAvatar.GetComponent<Image>();
        // Facebook will return a texture
        UserAvatar.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2(0, 0));
    }
    void HandleUserName(FBResult result)
    {
        // Check for errors
        if (result.Error != null)
        {
            // If problem occur
            Debug.Log("Problem with getting the user name");
            FB.API("/me?fields=id,first_name,last_name", Facebook.HttpMethod.GET, HandleUserName);
            return;
        }
        profile = FacebookUtil.DeserializeJSONProfile(result.Text);
        Text UserMsg = UIFBUserName.GetComponent<Text>();
        if (profile.Count == 2)
        {
            UserMsg.text = profile["first_name"] + " " + profile["last_name"];
        }
        else
        {
            UserMsg.text = profile["first_name"] + " " + profile["middle_name"] + " " + profile["last_name"];
        }
    }
    public void ShareWithFriends()
    {
        FB.Feed(
            linkCaption: "I'm playing this awesome game",
            picture: "http://api.ning.com/files/KJoM9AiK6kAoFEppJZZq8RHn3TXBkQ8IdW98RlKw1PUS3-KJW9MVINUD8ryTOUgYJQzkaB5A-rXWdQNNsKDmk7DlVNra7tPI/FullMoonET.jpg",
            linkName: "Take Me To The Moon",
            link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest")
            );
    }

    public void InviteFriends()
    {
        FB.AppRequest(
            message: "This game is awesome, come and join",
            title: "Invite your friends to join you"
            );
    }
    public void QueryScores()
    {
        SetScores();
        FB.API("/app/scores?field=score,user.limit(30)", Facebook.HttpMethod.GET, ScoresCallback);
    }
    private void ScoresCallback(FBResult result)
    {
        Debug.Log("Scores callback: " + result.Text);
        scoresList = FacebookUtil.DeserializeScores(result.Text);
        int a = 5;
        //foreach (Transform child in UIFBScoreList.transform)
        //{
        //    GameObject.Destroy(child.gameObject);
        //}
        //foreach (object score in scoresList)
        //{
        //    var entry = (Dictionary<string, object>)score;
        //    var user = (Dictionary<string, object>)entry["user"];

        //    GameObject ScorePanel;
        //    ScorePanel = Instantiate(UIFBScoreEntryPanel) as GameObject;
        //    ScorePanel.transform.SetParent(UIFBScoreList.transform,false);
        //    // Sets the transform for the three 
        //    Transform FriendName = ScorePanel.transform.Find("FriendName");
        //    Transform FriendScore = ScorePanel.transform.Find("FriendScore");
        //    Transform FriendAvatar = ScorePanel.transform.Find("FriendAvatar");
        //    Text FriendTextName = FriendName.GetComponent<Text>();
        //    Text FriendTextScore = FriendScore.GetComponent<Text>();
        //    Image FriendImageAvatar = FriendAvatar.GetComponent<Image>();
        //    FriendTextName.text = user["name"].ToString();
        //    FriendTextScore.text = entry["score"].ToString();

        //    // Gets the friends avatar + checks for errors before creating the sprite
        //    FB.API(FacebookUtil.GetPictureURL(user["id"].ToString(), 128, 128), Facebook.HttpMethod.GET, delegate(FBResult pictureResult)
        //    {
        //        if (pictureResult.Error != null)
        //        {
        //            Debug.Log(pictureResult.Error);
        //        }
        //        else { FriendImageAvatar.sprite = Sprite.Create(pictureResult.Texture, new Rect(0, 0, 128, 128), new Vector2(0, 0)); }
        //    });
        //}
    }
    public void SetScores()
    {
        var scoreData = new Dictionary<string, string>();
        scoreData["Moon"] = Random.Range(10, 200).ToString();
        FB.API("/me/scores", Facebook.HttpMethod.POST, delegate(FBResult result) { Debug.Log("Score Submit result: " + result.Text); }, scoreData);
    }
    public void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
            print("Android");
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
            print("Iphone");
    }
}
