using UnityEngine;

public class knife_01 : MonoBehaviour, IPremoveProvider
{
    private Vector2 preMoveTarget;
    private bool initialized = false;

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

    public bool Premove(Transform bulletTransform, ref float speed, Vector2 moveDir)
    {
        if (!initialized)
        {
            preMoveTarget = (Vector2)bulletTransform.position - moveDir * 2f;
            initialized = true;
        }

        float dist = Vector2.Distance(bulletTransform.position, preMoveTarget);

        if (dist > 0.01f)
        {
            // 距離に応じて減速
            float t = dist / 2f;
            speed = 20f * Mathf.Clamp01(t); // MaxSpeed = 20 例

            // 移動
            Vector2 dir = (preMoveTarget - (Vector2)bulletTransform.position).normalized;
            bulletTransform.position += (Vector3)(dir * speed * Time.deltaTime);
            if (speed < 2f)
            {
                return true;
            }
            else
            {
                return false; // まだ準備中
            }

        }
        else
        {
            speed = 0f;
            return true; // prepared!
        }
    }
}
