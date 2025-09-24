using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] // ActionMove.csなどで使う
[RequireComponent(typeof(ActionMove))]
public class ActionController : MonoBehaviour
{
    // 行動状態を判定するためのフラグ
    public bool IsDead;
    public bool IsDamage;   // thisAction = Damage
    public bool IsAttack; // thisAction = Attack
    public bool IsMove;    // thisAction = Move

    // キャラクターの行動種類
    public enum ActionType { None, Move, Attack, Damage }
    [SerializeField] private ActionType thisAction = ActionType.None;
    public ActionType ThisAction => thisAction;

    // 死亡処理フラグ
    private bool hasDied = false;

    void Awake()
    {
        if (gameObject.CompareTag("Player"))
        {
            // プレイヤー用の初期化処理を書くならここ
        }
        else
        {
            // 敵やNPC用の初期化処理を書くならここ
        }
    }

    void Update()
    {
        if (IsDamage)
        {
            thisAction = ActionType.Damage;

            if (IsDead)
            {
                Dead();
            }
        }
        else if (IsAttack)
        {
            thisAction = ActionType.Attack;
        }
        else if (IsMove)
        {
            thisAction = ActionType.Move;
        }
        else
        {
            thisAction = ActionType.None;
        }
    }

    private void Dead()
    {
        if (hasDied) return;
        hasDied = true;

        // 死亡時に行いたい処理（アニメ再生・当たり判定無効化など）をここに追加
    }
}
