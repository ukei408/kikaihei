using UnityEngine;

[RequireComponent(typeof(FootstepSwitcher))]
[RequireComponent(typeof(SpriteRenderer))]
public class TileMapAnim : MonoBehaviour
{
    [Header("Animation Data")]
    public SpriteAnimData animData;

    [Header("Generator Control")]
    public bool callNext = true; // trueのときだけNext()を呼ぶ

    private SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float timer;

    private bool nextCalled = false;
    private TileMapGenerator generator; // 自動探索用キャッシュ

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // --- Generator を自動で探す ---
        // 1. 親階層から探す
        generator = GetComponentInParent<TileMapGenerator>();
        if (generator == null && transform.parent != null)
        {
            // 2. 親の兄弟階層から探す
            generator = transform.parent.GetComponentInChildren<TileMapGenerator>();
        }
        if (generator == null)
        {
            // 3. 最後の保険: シーン全体から探す
            // 3. 最後の保険: シーン全体から探す
            generator = FindFirstObjectByType<TileMapGenerator>();

        }

        if (generator == null)
        {
            Debug.LogWarning($"TileMapGenerator が見つかりませんでした: {name}");
        }
    }

    void Start()
    {
        if (animData == null || animData.frames.Length == 0)
        {
            Debug.LogError("SpriteAnimData が設定されていません: " + name);
            enabled = false;
            return;
        }

        currentFrame = 0;
        spriteRenderer.sprite = animData.frames[currentFrame];
        timer = animData.frameRate;
    }

    void Update()
    {
        if (animData == null || animData.frames.Length == 0) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            currentFrame++;
            if (currentFrame >= animData.frames.Length)
            {
                if (animData.loop)
                {
                    currentFrame = 0;
                    nextCalled = false; // ループ時はリセット
                }
                else
                {
                    currentFrame = animData.frames.Length - 1;
                    enabled = false; // 終了
                }
            }

            spriteRenderer.sprite = animData.frames[currentFrame];
            timer = animData.frameRate;

            // 最終フレームの1つ前
            if (!nextCalled && currentFrame == animData.frames.Length - 2)
            {
                if (callNext && generator != null)
                {
                    generator.Next();
                    Debug.Log("TileMapGenerator.Next() を呼び出しました");
                }
                nextCalled = true;
            }
        }
    }
}
