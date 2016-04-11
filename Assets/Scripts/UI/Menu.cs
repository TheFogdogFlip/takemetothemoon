using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
    private Animator animator;
    private CanvasGroup _canvasGroup;

    public bool IsOpen
    {
        get { return animator.GetBool("IsOpen");}
        set { animator.SetBool("IsOpen", value); }
    }



    public void Awake()
    {
        animator = GetComponent<Animator>();
        _canvasGroup = GetComponent <CanvasGroup>();


        var rect = GetComponent<RectTransform>();
        rect.offsetMax = rect.offsetMin = new Vector2(0, 0);
    }

    public void Update()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
        {
            _canvasGroup.blocksRaycasts = _canvasGroup.interactable = false;
        }
        else
        {
            _canvasGroup.blocksRaycasts = _canvasGroup.interactable = true;
        }
    }


}
