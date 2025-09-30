using UnityEngine;

[RequireComponent(typeof(BulletAnim))]
public class EnergyMove : MonoBehaviour
{
    private BulletAnim bulletAnim;

    void Awake()
    {
        // 同じオブジェクトにアタッチされている BulletAnim を取得
        bulletAnim = GetComponent<BulletAnim>();

        if (bulletAnim != null)
        {
            bulletAnim.Play();
        }
        else
        {
            Debug.LogWarning("BulletAnim が同じオブジェクトに見つかりませんでした", this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 名前ではなくタグで判定
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("Bulletタグのオブジェクトのサークルコライダーに入りました！");
            // ここに好きな処理を書く
        }
    }
}
