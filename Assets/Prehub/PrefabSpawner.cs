using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab; // アタッチしたいプレハブ
    [SerializeField] private float interval = 1f; // 生成間隔（秒）

    private void Start()
    {
        // StartSpawnメソッドを interval 秒ごとに繰り返す
        InvokeRepeating(nameof(SpawnPrefab), 0f, interval);
    }

    private void SpawnPrefab()
    {
        if (prefab != null)
        {
            // このオブジェクトの位置に生成
            Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
}
