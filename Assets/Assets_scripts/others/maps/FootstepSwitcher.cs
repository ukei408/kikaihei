using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FootstepSwitcher : MonoBehaviour
{
    [SerializeField] private ActionMove.FootSteps footstepType = ActionMove.FootSteps.Stone;

    private void Awake()
    {
        // BoxCollider2D を取得して設定
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.size = new Vector2(0.5f, 0.5f);
        collider.isTrigger = true; // トリガーに設定
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ActionMove actionMove = collision.GetComponent<ActionMove>();
        if (actionMove != null)
        {
            actionMove.currentFootstep = footstepType;
        }
    }
}
