using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnim : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("通常アクション用アニメーション")]
    public List<SpriteAnimData> defaultAnimations;  // SOのリスト

    [Header("スキル用アニメーション（未使用）")]
    public List<SpriteAnimData> skillAnimations;    // 今後使用予定

    private int currentAnimIndex = 0;
    private int frameIndex = 0;

    private ActionController.ActionType prevAction = ActionController.ActionType.None;
    private ActionController actionController; // 親オブジェクトのActionController参照

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        actionController = GetComponentInParent<ActionController>();

        FastPlay();
    }

    void Update()
    {
        if (actionController == null || defaultAnimations.Count == 0) return;

        // ActionType の変化をチェックしてアニメ切り替え
        if (actionController.ThisAction != prevAction)
        {
            prevAction = actionController.ThisAction;
            SetAnimationByAction(prevAction);

            // 切り替えた瞬間に最初のフレームを即座に表示
            SpriteAnimData data = defaultAnimations[currentAnimIndex];
            if (data != null && data.frames.Length > 0)
                spriteRenderer.sprite = data.frames[0];
        }

        Play();
    }

    private void Play()
    {
        if (defaultAnimations == null || defaultAnimations.Count == 0 || actionController == null) return;

        SpriteAnimData data = defaultAnimations[currentAnimIndex];
        if (data == null || data.frames.Length == 0) return;

        // ActionTimer を参照してフレームを決定
        float timer = actionController.ActionTimer;
        int calculatedFrame = Mathf.FloorToInt(timer / data.frameRate);

        if (!data.loop && calculatedFrame >= data.frames.Length)
            calculatedFrame = data.frames.Length - 1;
        else if (data.loop)
            calculatedFrame = calculatedFrame % data.frames.Length;

        if (calculatedFrame != frameIndex)
        {
            frameIndex = calculatedFrame;
            spriteRenderer.sprite = data.frames[frameIndex];
        }
    }

    private void SetAnimationByAction(ActionController.ActionType action)
    {
        switch (action)
        {
            case ActionController.ActionType.None:   SetAnimation(0); break;
            case ActionController.ActionType.Move:   SetAnimation(1); break;
            case ActionController.ActionType.Trans:  SetAnimation(2); break;
            case ActionController.ActionType.Damage: SetAnimation(3); break;
        }
    }

    public void SetAnimation(int index)
    {
        if (index < 0 || index >= defaultAnimations.Count) return;

        if (currentAnimIndex != index)
        {
            currentAnimIndex = index;
            frameIndex = 0;

            // 切り替えた瞬間に最初のフレームを表示
            if (defaultAnimations[currentAnimIndex] != null && defaultAnimations[currentAnimIndex].frames.Length > 0)
                spriteRenderer.sprite = defaultAnimations[currentAnimIndex].frames[0];

            // ActionTimer をリセットして同期
            if (actionController != null)
                actionController.ResetActionTimer();
        }
    }

    private void FastPlay()
    {
        if (defaultAnimations.Count > 0 && actionController != null)
        {
            prevAction = actionController.ThisAction;
            SetAnimationByAction(prevAction);

            // 実行直後に1枚目を強制描画
            SpriteAnimData data = defaultAnimations[currentAnimIndex];
            if (data != null && data.frames.Length > 0)
            {
                frameIndex = 0;
                spriteRenderer.sprite = data.frames[0];
            }
        }
    }
}
