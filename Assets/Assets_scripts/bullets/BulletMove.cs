using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float speed = 0f;          // 現在速度
    public float InitialSpeed = 0f;   // 初速
    public float Acceleration = 1f;   // 秒間加速度
    public float MaxSpeed = 20f;      // 最高速度


    private Transform target;
    public bool OnPlaced = false;

    private Vector2 moveDir;
    private bool initialized = false;
    private bool prepared = false;

    void Update()
    {
        if (!OnPlaced) return;//BulletAnim.csの処理完了後
        MoveStart();
    }

    private void MoveStart()
    {
        if (!initialized)
        {
            InitializeTarget();   // ← 一行で呼び出し
            if (!initialized) return; // 初期化できなければ移動処理しない
        }
        if (prepared)
        {
            PreMove();
        }
        else
        {
            SetSpeed();
            transform.position += (Vector3)(moveDir * speed * Time.deltaTime);
        }
    }

    public void InitializeTarget()
    {
        Transform parent = transform.parent;
        if (parent == null) return;

        Transform found = parent.Find("Target");
        if (found == null)
        {
            Debug.LogWarning("Target が見つかりません。");
            return;
        }

        target = found;
        moveDir = ((Vector2)target.position - (Vector2)transform.position).normalized;
        speed = InitialSpeed;
        initialized = true;
    }

    private void SetSpeed()
    {
        // 残り分（MaxSpeed - 現在速度）を計算
        float remaining = MaxSpeed - speed;

        // Acceleration は「残り分を埋める割合/秒」
        // Acceleration = 1 → 1秒で残りを埋める
        float delta = remaining * Acceleration * Time.deltaTime;

        speed += delta;

        // 念のため上限でClamp
        speed = Mathf.Clamp(speed, 0, MaxSpeed);
    }

    private void PreMove()
    {

    }
}
