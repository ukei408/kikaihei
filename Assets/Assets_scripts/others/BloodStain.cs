using UnityEngine;

public class BloodStain : MonoBehaviour
{
    private BloodParticle bloodParticle;

    void Start()
    {
        // 同じオブジェクトにある BloodParticle を取得
        bloodParticle = GetComponent<BloodParticle>();
    }

    void Update()
    {
        // bloodStainOn が false の場合は何もしない
        if (bloodParticle == null || !bloodParticle.bloodStainOn)
            return;

        // ↓ここに実際の処理を書く
        // 例: transform.position += Vector3.down * Time.deltaTime;
    }
}
