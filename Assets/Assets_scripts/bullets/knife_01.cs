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
                    BulletAnim bulletAnim = sibling.GetComponent<BulletAnim>();
                    if (bulletAnim != null)
                    {
                        bulletAnim.Play();
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
