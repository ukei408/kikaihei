using UnityEngine;
using UnityEngine.InputSystem;  

public class ActionMove : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private ActionController actionController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        actionController = GetComponent<ActionController>();
    }

    private void Update()
    {
        if (gameObject.CompareTag("Player"))
        {
            
        }
        else
        {

        }
    }

    //Controller.cs側でキャラクターが移動できる状況に限り、座標を動かす
    private void FixedUpdate()
    {
        if (actionController.ThisAction == ActionController.ActionType.Move)
        {
            rb.linearVelocity = moveInput * speed;
        }

        if (gameObject.CompareTag("Player"))
        {
            if(moveInput == Vector2.zero)
            {
                if (actionController != null)
                {
                    actionController.IsMove = false;
                }
            }
        }
    }

    // 移動コマンド入力を受けつけ、Controller.csのIsMoveをtrueに
    private void OnMove(InputValue value)
    {
        if (gameObject.CompareTag("Player"))
        {
            moveInput = value.Get<Vector2>();
            if (actionController != null)
            {
                actionController.IsMove = true;
            }
        }
    }
}
