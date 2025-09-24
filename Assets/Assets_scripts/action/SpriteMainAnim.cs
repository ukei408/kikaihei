using UnityEngine;

public class SpriteMainAnim : MonoBehaviour
{
    private ActionController actionController;
    private Animator[] animators;

    private void Awake()
    {
        actionController = GetComponent<ActionController>();
        // 子オブジェクトにある全 Animator を取得
        animators = GetComponentsInChildren<Animator>();
    }

    private void Update()
    {
        int state = (int)actionController.ThisAction;
        foreach (var anim in animators)
        {
            anim.SetInteger("Action", (int)actionController.ThisAction);
        }
    }

}
