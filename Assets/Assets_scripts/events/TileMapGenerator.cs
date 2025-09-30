using UnityEngine;

public class TileMapGenerator : MonoBehaviour
{
    [Header("Vertical Prefabs")]
    public GameObject topBottomPrefab;
    public GameObject centerPrefab;

    [Header("Horizontal Prefabs")]
    public GameObject loopXPrefab;
    public GameObject lastXPrefab;

    [Header("Map Size")]
    public int yWidth = 3;
    public int xWidth = 3;

    void Start()
    {
        Generate();
    }

    void Generate()
    {
        // --- 縦方向生成 ---
        // 既存の子を削除
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        // Top（上側）
        Instantiate(topBottomPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, transform);

        // Center（真ん中の繰り返し）
        for (int y = 1; y <= yWidth; y++)
        {
            Vector3 pos = new Vector3(0f, -0.5f * y, 0f);
            Instantiate(centerPrefab, pos, Quaternion.identity, transform);
        }

        // Bottom（下側、yスケール反転）
        Vector3 bottomPos = new Vector3(0f, -0.5f * (yWidth + 1), 0f);
        GameObject bottom = Instantiate(topBottomPrefab, bottomPos, Quaternion.identity, transform);
        bottom.transform.localScale = new Vector3(1f, -1f, 1f);

        // --- 横方向生成 ---
        // transform と同じ階層（親は共有するが、子にはならない）
        Vector3 basePos = transform.position;

        // X方向のループ配置
        for (int x = 1; x <= xWidth; x++)
        {
            float xPos = basePos.x + 0.5f * x;
            Vector3 pos = new Vector3(xPos, basePos.y, 0f);
            Instantiate(loopXPrefab, pos, Quaternion.identity, transform.parent);
        }

        // 最後に last X prefab を配置
        Vector3 lastPos = new Vector3(basePos.x + 0.5f * (xWidth + 1), basePos.y, 0f);
        Instantiate(lastXPrefab, lastPos, Quaternion.identity, transform.parent);
    }
}
