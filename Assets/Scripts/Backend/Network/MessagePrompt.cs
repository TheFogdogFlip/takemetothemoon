using UnityEngine;
using System.Collections;

public class MessagePrompt : MonoBehaviour {
    private MessagePromptDelegate messagePromptCallback;
    public UnityEngine.UI.Button Button;
    public UnityEngine.UI.InputField email;
    public UnityEngine.UI.Text Text;
    public UnityEngine.UI.Toggle Toggle;
    public UnityEngine.UI.InputField pass;

    void Start()
    {

    }
    public void SetCallback(MessagePromptDelegate callback)
    {
        this.messagePromptCallback = callback;
    }

    public void StartCallback()
    {
       messagePromptCallback(this.gameObject);
       Button.interactable = false;
    }
}