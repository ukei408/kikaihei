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

    private int currentXIndex = 0; // Next()が呼ばれるたびに進むカウンタ

    void Start()
    {
        GenerateVertical();
    }

    // --- 縦方向のみ生成 ---
    private void GenerateVertical()
    {
        // 既存の子を削除
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        Vector3 basePos = transform.position;

        // Top（上側）
        Instantiate(topBottomPrefab, basePos, Quaternion.identity, transform);

        // Center（真ん中の繰り返し）
        for (int y = 1; y <= yWidth; y++)
        {
            Vector3 pos = basePos + new Vector3(0f, -0.5f * y, 0f);
            Instantiate(centerPrefab, pos, Quaternion.identity, transform);
        }

        // Bottom（下側、yスケール反転）
        Vector3 bottomPos = basePos + new Vector3(0f, -0.5f * (yWidth + 1), 0f);
        GameObject bottom = Instantiate(topBottomPrefab, bottomPos, Quaternion.identity, transform);
        bottom.transform.localScale = new Vector3(1f, -1f, 1f);

        TileMapAnim bottomAnim = bottom.GetComponent<TileMapAnim>();
        if (bottomAnim != null)
        {
            bottomAnim.callNext = false;
            Debug.Log("Bottom の TileMapAnim.callNext を false にしました");
        }
    }

    // --- 横方向の生成（Nextが呼ばれたときだけ） ---
    public void Next()
    {
        Vector3 basePos = transform.position;

        // 次のX座標を計算
        float xPos = basePos.x + 0.5f * (currentXIndex + 1);
        Vector3 pos = new Vector3(xPos, basePos.y, 0f);

        if (currentXIndex < xWidth)
        {
            // loopXPrefab を配置（親は generator 自身）
            Instantiate(loopXPrefab, pos, Quaternion.identity, transform);
        }
        else if (currentXIndex == xWidth)
        {
            // 最後の位置に lastXPrefab を配置（親は generator 自身）
            GameObject last = Instantiate(lastXPrefab, pos, Quaternion.identity, transform);
            last.transform.SetParent(transform); // 念のため明示的に設定
        }

        currentXIndex++;
    }
}
