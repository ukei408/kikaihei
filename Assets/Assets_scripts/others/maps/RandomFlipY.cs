using UnityEngine;

public class RandomFlipY : MonoBehaviour
{
    void Start()
    {
        // 0 または 1 のどちらかをランダムに取得
        int random = Random.Range(0, 2);

        if (random == 1)
        {
            Vector3 scale = transform.localScale;
            scale.y *= -1;  // Y軸スケールを反転
            transform.localScale = scale;
        }
    }
}
