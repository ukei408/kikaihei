using UnityEngine;

public class FootstepSwitcher : MonoBehaviour
{
    [SerializeField] private ActionMove.FootSteps footstepType = ActionMove.FootSteps.Stone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ActionMove actionMove = collision.GetComponent<ActionMove>();
        if (actionMove != null)
        {
            actionMove.currentFootstep = footstepType;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ActionMove actionMove = collision.GetComponent<ActionMove>();
        if (actionMove != null)
        {
            actionMove.currentFootstep = ActionMove.FootSteps.None;
        }
    }
}
