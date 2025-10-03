using UnityEngine;
using UnityEngine.InputSystem;

public class ActionMove : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 moveInput;
    private ActionController actionController;
    private Rigidbody2D rb;

    private const float PPU = 120f; // Pixels Per Unit
    private float pixelSize => 1f / PPU;

    public enum FootSteps { None, Stone }
    public FootSteps currentFootstep = FootSteps.None; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        actionController = GetComponent<ActionController>();
    }

    private void FixedUpdate()
    {
        if (actionController.ThisAction == ActionController.ActionType.Move)
        {
            // 通常移動
            Vector2 targetPos = rb.position + moveInput * speed * Time.fixedDeltaTime;

            // ★ ピクセル単位にスナップ
            targetPos.x = Mathf.Round(targetPos.x / pixelSize) * pixelSize;
            targetPos.y = Mathf.Round(targetPos.y / pixelSize) * pixelSize;

            rb.MovePosition(targetPos);
        }

        if (CompareTag("Player") && moveInput == Vector2.zero)
        {
            if (actionController != null)
                actionController.IsMove = false;
        }
    }

    private void OnMove(InputValue value)
    {
        if (CompareTag("Player"))
        {
            moveInput = value.Get<Vector2>();
            if (actionController != null)
                actionController.IsMove = true;
        }
    }
}
