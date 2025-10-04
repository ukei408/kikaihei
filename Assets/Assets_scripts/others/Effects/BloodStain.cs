using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class BloodStain : MonoBehaviour
{
    [Header("生成するプレハブ")]
    public GameObject spawnPrefab;

    private BloodParticle bloodParticle;
    private CircleCollider2D circleCollider;

    void Awake()
    {
        // CircleCollider2Dを取得 or 自動アタッチ
        circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider == null)
            circleCollider = gameObject.AddComponent<CircleCollider2D>();

        // isTrigger = true にして大きさを設定
        circleCollider.isTrigger = true;
        circleCollider.radius = 0.1f;

        // Rigidbody2Dを取得 or 自動アタッチ
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;

        // BloodParticle参照
        bloodParticle = GetComponent<BloodParticle>();
    }

    void Update()
    {
        // 出現していないときは処理しない
        if (bloodParticle == null || !bloodParticle.bloodStainOn)
            return;

        DetectTilemapCollision();
    }

    private void DetectTilemapCollision()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, circleCollider.radius);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Tilemap"))
            {
                if (spawnPrefab != null)
                {
                    Instantiate(spawnPrefab, transform.position, Quaternion.identity, transform.parent);
                }

                Destroy(gameObject);
                break;
            }
        }
    }
}
