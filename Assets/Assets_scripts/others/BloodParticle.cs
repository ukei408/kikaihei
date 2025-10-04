using UnityEngine;

/// <summary>
/// 血しぶきパーティクル
/// 一定時間上昇し、その後角度を徐々に下向きへ遷移して落下する。
/// 生成時にランダムスプライト選択、ランダムスケール設定、0.1秒でスケールアップ。
/// 落下開始時に bloodStainOn が true になる。
/// </summary>
public class BloodParticle : MonoBehaviour
{
    [Header("Speed Settings")]
    public float minInitialSpeed = 3f;
    public float maxInitialSpeed = 6f;
    public float minFallSpeed = 8f;
    public float maxFallSpeed = 15f;

    [Header("Angle Settings")]
    [Range(0f, 180f)]
    public float angleDegrees = 60f;
    public bool randomizeAngle = false;
    public float angleSpread = 10f;

    [Header("Timing")]
    public float minRiseTime = 0.1f;
    public float maxRiseTime = 0.5f;
    public float descendTransitionTime = 0.2f;

    [Header("Sprite Variations")]
    public Sprite[] sprites; // 生成時にランダム選択

    [Header("Rotation")]
    public bool rotateToVelocity = true;
    public float rotationSpeed = 180f;

    [Header("Scale")]
    public float minScale = 0.5f;
    public float maxScale = 2f;
    public float scaleUpDuration = 0.1f; // 拡大にかける時間

    // 内部変数
    Vector2 velocity;
    float riseTimer;
    float fallSpeed;
    bool isDescending = false;
    float transitionTimer = 0f;

    SpriteRenderer spriteRenderer;

    // スケール拡大用
    float targetScale;
    float scaleTimer = 0f;

    // 血痕出現フラグ（外部から参照可能）
    public bool bloodStainOn { get; private set; } = false;

    void Start()
    {
        // 初速ランダム
        float speed = Random.Range(minInitialSpeed, maxInitialSpeed);
        float angle = angleDegrees;
        if (randomizeAngle)
            angle += Random.Range(-angleSpread * 0.5f, angleSpread * 0.5f);

        float rad = angle * Mathf.Deg2Rad;
        velocity = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * speed;

        // 上昇時間・落下速度をランダムに決定
        riseTimer = Random.Range(minRiseTime, maxRiseTime);
        fallSpeed = Random.Range(minFallSpeed, maxFallSpeed);

        // SpriteRenderer 取得
        spriteRenderer = GetComponent<SpriteRenderer>();

        // スプライトをランダムで選択
        if (sprites != null && sprites.Length > 0)
        {
            int index = Random.Range(0, sprites.Length);
            spriteRenderer.sprite = sprites[index];
        }

        // スケール初期化（0から拡大）
        targetScale = Random.Range(minScale, maxScale);
        transform.localScale = Vector3.zero;
        scaleTimer = scaleUpDuration;
    }

    void Update()
    {
        // 拡大アニメーション
        if (scaleTimer > 0f)
        {
            scaleTimer -= Time.deltaTime;
            float t = 1f - (scaleTimer / scaleUpDuration);
            float currentScale = Mathf.Lerp(0f, targetScale, t);
            transform.localScale = new Vector3(currentScale, currentScale, 1f);
        }

        // 上昇＆落下処理
        if (!isDescending)
        {
            riseTimer -= Time.deltaTime;
            transform.position += (Vector3)(velocity * Time.deltaTime);

            if (riseTimer <= 0f)
            {
                isDescending = true;
                bloodStainOn = true; // 落下開始時にtrue
                transitionTimer = descendTransitionTime;
            }
        }
        else
        {
            // 下降方向へ遷移
            if (transitionTimer > 0f)
            {
                transitionTimer -= Time.deltaTime;
                float t = 1f - (transitionTimer / descendTransitionTime);
                Vector2 targetVel = Vector2.down * fallSpeed;
                velocity = Vector2.Lerp(velocity, targetVel, t);
            }
            else
            {
                velocity = Vector2.down * fallSpeed;
            }

            transform.position += (Vector3)(velocity * Time.deltaTime);
        }

        // 回転処理
        if (rotateToVelocity && velocity.sqrMagnitude > 0.0001f)
        {
            float ang = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, ang);
        }
        else if (!rotateToVelocity)
        {
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
    }
}
