using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BulletAnim : MonoBehaviour
{
    public SpriteAnimData animData; // ScriptableObjectで設定
    private BulletMove bulletMove;   // BulletMove スクリプトへの参照

    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;

    private int currentFrame = 0;
    private float timer = 0f;
    private bool isPlaying = false;

    // DrainEnergy制御用
    private bool isDraining = false;
    private float drainTimer = 0f;
    private float drainDuration = 0.3f; // 0.3秒で収縮
    private float startRadius = 3f;
    private float endRadius = 0.1f;
    private bool DrainFinish = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        bulletMove = GetComponent<BulletMove>();
        circleCollider = gameObject.AddComponent<CircleCollider2D>();
        circleCollider.isTrigger = true;
        circleCollider.radius = startRadius;
        if (CompareTag("Bullet"))
        {
            circleCollider.enabled = false;
            if (animData != null && animData.frames.Length > 0)
            {
                drainDuration = animData.frames.Length * animData.frameRate;
            }

        }
        else
        {
            startRadius = 0.5f;
        }
        var rb = gameObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true;

    }

    void Update()
    {
        if (!isPlaying || animData == null || animData.frames.Length == 0) return;

        DrainEnergy();

        timer += Time.deltaTime;
        if (timer >= animData.frameRate)
        {
            timer -= animData.frameRate;
            currentFrame++;

            if (currentFrame >= animData.frames.Length)
            {
                if (animData.loop)
                {
                    currentFrame = 0;
                }
                else
                {
                    currentFrame = animData.frames.Length - 1;
                    isPlaying = false;

                    if (bulletMove != null)
                    {
                        bulletMove.OnPlaced = true;
                    }
                }
            }

            spriteRenderer.sprite = animData.frames[currentFrame];
        }
    }

    public void Play()
    {
        if (animData == null || animData.frames.Length == 0) return;

        currentFrame = 0;
        timer = 0f;
        isPlaying = true;
        spriteRenderer.sprite = animData.frames[currentFrame];
    }

    private void DrainEnergy()
    {
        if (!CompareTag("Bullet")) return;
        if (circleCollider == null || DrainFinish) return;

        // 最初に呼ばれたときだけ有効化＆ドレイン開始
        if (!circleCollider.enabled && !isDraining)
        {
            circleCollider.enabled = true;
            isDraining = true;
            drainTimer = 0f;
            Debug.Log("DrainEnergy開始: CircleCollider2D 有効化");
        }

        // ドレイン処理中
        if (isDraining)
        {
            drainTimer += Time.deltaTime;
            float t = Mathf.Clamp01(drainTimer / drainDuration);

            // 半径を徐々に縮小
            circleCollider.radius = Mathf.Lerp(startRadius, endRadius, t);

            // 完了判定
            if (t >= 1f)
            {
                circleCollider.radius = endRadius;
                circleCollider.enabled = false;
                isDraining = false;
                DrainFinish = true;
                Debug.Log("DrainEnergy完了: Collider無効化 & DrainFinish = true");
            }
        }
    }
}
