using UnityEngine;

public class knife_01 : MonoBehaviour
{
    void Start()
    {
        // Player タグのオブジェクトを探す
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // knife を player の位置に移動
            transform.position = player.transform.position;

            // 同じ階層にあるオブジェクトを走査
            Transform parent = transform.parent;
            if (parent != null)
            {
                foreach (Transform sibling in parent)
                {
                    if (sibling.CompareTag("Bullet"))
                    {
                        // knifeへの方向ベクトルを計算
                        Vector2 dir = transform.position - sibling.position;

                        // atan2で角度(ラジアン)を計算 → 度に変換
                        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                        // Z回転に反映（必要なら -90f 補正）
                        sibling.rotation = Quaternion.Euler(0, 0, angle);

                        // BulletAnim があれば再生
                        BulletAnim bulletAnim = sibling.GetComponent<BulletAnim>();
                        if (bulletAnim != null)
                        {
                            bulletAnim.Play();
                        }

                        Debug.Log($"Bullet {sibling.name} を knife 方向へ回転: {angle}度");
                    }

                }
            }
        }
        else
        {
            Debug.LogWarning("Player タグのオブジェクトが見つかりません。");
        }
    }
}
