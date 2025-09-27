using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BulletAnim : MonoBehaviour
{
    public SpriteAnimData animData; // ScriptableObjectで設定
    private BulletMove bulletMove;   // BulletMove スクリプトへの参照

    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;
    private float timer = 0f;
    private bool isPlaying = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        bulletMove = GetComponent<BulletMove>();
    }

    /// <summary>
    /// アニメーション再生開始
    /// </summary>
    public void Play()
    {
        if (animData == null || animData.frames.Length == 0) return;

        currentFrame = 0;
        timer = 0f;
        isPlaying = true;
        spriteRenderer.sprite = animData.frames[currentFrame];
    }

    void Update()
    {
        if (!isPlaying || animData == null || animData.frames.Length == 0) return;

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
                    // 最終フレーム到達で停止
                    currentFrame = animData.frames.Length - 1;
                    isPlaying = false;

                    // BulletMove の OnPlaced を true にする
                    if (bulletMove != null)
                    {
                        bulletMove.OnPlaced = true;
                    }
                }
            }

            spriteRenderer.sprite = animData.frames[currentFrame];
        }
    }
}
