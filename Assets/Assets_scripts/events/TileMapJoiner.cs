using UnityEngine;

public class TileMapJoiner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject topBottomPrefab;
    public GameObject centerPrefab;

    private void Start()
    {
        // 同じ親階層にある TileMapGenerator を探す
        TileMapGenerator generator = transform.parent.GetComponentInChildren<TileMapGenerator>();

        if (generator == null)
        {
            Debug.LogError("兄弟階層に TileMapGenerator が見つかりません");
            return;
        }

        int yWidth = generator.yWidth;

        GenerateYLoop(yWidth);
    }

    private void GenerateYLoop(int yWidth)
    {
        if (topBottomPrefab == null || centerPrefab == null)
        {
            Debug.LogError("topBottomPrefab または centerPrefab が設定されていません");
            return;
        }

        // ルート位置
        Vector3 basePos = transform.position;

        // top (最初の端)
        GameObject top = Instantiate(topBottomPrefab, basePos, Quaternion.identity, transform);
        top.name = "Top";

        // center を yWidth 回生成
        for (int i = 1; i <= yWidth; i++)
        {
            Vector3 pos = basePos + new Vector3(0, -0.5f * i, 0);
            GameObject center = Instantiate(centerPrefab, pos, Quaternion.identity, transform);
            center.name = "Center_" + i;
        }

        // bottom (最後の端, yスケール反転)
        Vector3 bottomPos = basePos + new Vector3(0, -0.5f * (yWidth + 1), 0);
        GameObject bottom = Instantiate(topBottomPrefab, bottomPos, Quaternion.identity, transform);
        bottom.transform.localScale = new Vector3(1, -1, 1);
        bottom.name = "Bottom";

        // --- 追加: Bottom の TileMapAnim を無効化 ---
        TileMapAnim bottomAnim = bottom.GetComponentInChildren<TileMapAnim>();
        if (bottomAnim != null)
        {
            bottomAnim.callNext = false;
            Debug.Log("Joiner の Bottom の TileMapAnim.callNext を false にしました");
        }
    }
}
