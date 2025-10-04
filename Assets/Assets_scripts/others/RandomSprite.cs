using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(SoundPlayer))]
public class RandomSprite : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite[] randomSprites;

    [Header("BloodStain上限設定")]
    public int maxBloodStains = 10;

    [Header("フェード設定")]
    public float fadeDuration = 0.5f;
    public float postFadeDelay = 0.05f;

    [Header("効果音設定")]
    public AudioClip[] spawnSounds; // 複数の効果音を指定

    private void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        // ランダムスプライト設定
        if (randomSprites != null && randomSprites.Length > 0 && renderer != null)
        {
            renderer.sprite = randomSprites[Random.Range(0, randomSprites.Length)];
        }

        // 生成時にSoundPlayerを再生（ランダム選択）
        SoundPlayer sp = GetComponent<SoundPlayer>();
        if (sp != null && spawnSounds != null && spawnSounds.Length > 0)
        {
            AudioClip clip = spawnSounds[Random.Range(0, spawnSounds.Length)];
            sp.Play(clip);
        }

        // シーン内の BloodStain オブジェクト取得
        var bloodStains = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None)
            .Where(obj => obj.name.Contains("BloodStain") && obj != this.gameObject)
            .ToList();

        if (bloodStains.Count >= maxBloodStains)
        {
            int randomIndex = Random.Range(0, bloodStains.Count);
            GameObject target = bloodStains[randomIndex];

            SpriteRenderer targetRenderer = target.GetComponent<SpriteRenderer>();
            if (targetRenderer != null)
            {
                StartCoroutine(FadeAndDestroy(target, targetRenderer));
            }
        }
    }

    private IEnumerator FadeAndDestroy(GameObject obj, SpriteRenderer renderer)
    {
        if (renderer == null || obj == null)
            yield break;

        float timer = 0f;
        Color c = renderer.color;

        while (timer < fadeDuration)
        {
            if (renderer == null || obj == null)
                yield break;

            timer += Time.deltaTime;
            c.a = Mathf.Clamp01(1f - (timer / fadeDuration));
            renderer.color = c;
            yield return null;
        }

        if (renderer != null)
            renderer.color = new Color(c.r, c.g, c.b, 0f);

        yield return new WaitForSeconds(postFadeDelay);

        if (obj != null)
            Destroy(obj);
    }
}
