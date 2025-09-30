using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TileMapAnim : MonoBehaviour
{
    [Header("Animation Data")]
    public SpriteAnimData animData;

    private SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float timer;

    private bool nextCalled = false; // Next() を一度しか呼ばない制御

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
                    nextCalled = false; // ループ時は再度呼べるようにする
                }
                else
                {
                    currentFrame = animData.frames.Length - 1; // 最終フレームで止める
                    enabled = false; // アニメ終了
                }
            }

            spriteRenderer.sprite = animData.frames[currentFrame];
            timer = animData.frameRate;

            // 「最終フレームの1つ前」に到達した瞬間
            if (!nextCalled && currentFrame == animData.frames.Length - 2)
            {
                CallGeneratorNext();
                nextCalled = true;
            }
        }
    }

    private void CallGeneratorNext()
    {
        // 兄弟階層の TileMapGenerator を探す
        Transform parent = transform.parent;
        if (parent == null) return;

        TileMapGenerator generator = parent.GetComponentInChildren<TileMapGenerator>();
        if (generator != null)
        {
            generator.SendMessage("Next", SendMessageOptions.DontRequireReceiver);
            Debug.Log("TileMapGenerator.Next() を呼び出しました");
        }
        else
        {
            Debug.LogWarning("兄弟階層に TileMapGenerator が見つかりませんでした");
        }
    }
}
