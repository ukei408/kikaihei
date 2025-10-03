using UnityEngine;

public class TileMapGenerator : MonoBehaviour
{
    [Header("Vertical Prefabs (Generator用)")]
    public GameObject topBottomPrefab;
    public GameObject centerPrefab;

    [Header("Horizontal Prefabs")]
    public GameObject loopXPrefab;
    public GameObject lastXPrefab;

    [Header("Map Size")]
    public int yWidth = 3;
    public int xWidth = 3;

    private int currentXIndex = 0; 
    private Vector3 startPos;      

    void Start()
    {
        float mapHeight = 1f * (yWidth + 2);
        float mapWidth  = 1f * (xWidth + 1);

        startPos = transform.position + new Vector3(-mapWidth / 2f, mapHeight / 2f, 0f);

        // 自分のPrefabを渡して縦生成
        GenerateVertical(transform, startPos, yWidth, topBottomPrefab, centerPrefab);
    }

    /// <summary>
    /// 縦方向に Top, Center, Bottom を生成する共通処理
    /// Prefabを外部から指定できるようにする
    /// </summary>
    public void GenerateVertical(Transform parent, Vector3 basePos, int yWidth,
                                 GameObject topBottom, GameObject center)
    {
        if (topBottom == null || center == null)
        {
            Debug.LogError("GenerateVertical に必要なPrefabが設定されていません");
            return;
        }

        // Top
        Instantiate(topBottom, basePos, Quaternion.identity, parent).name = "Top";

        // Center
        for (int i = 1; i <= yWidth; i++)
        {
            Vector3 pos = basePos + new Vector3(0, -1f * i, 0);
            Instantiate(center, pos, Quaternion.identity, parent).name = "Center_" + i;
        }

        // Bottom
        Vector3 bottomPos = basePos + new Vector3(0, -1f * (yWidth + 1), 0);
        GameObject bottom = Instantiate(topBottom, bottomPos, Quaternion.identity, parent);
        bottom.transform.localScale = new Vector3(2f, -2f, 1f);
        bottom.name = "Bottom";

        TileMapAnim bottomAnim = bottom.GetComponent<TileMapAnim>();
        if (bottomAnim != null)
        {
            bottomAnim.callNext = false;
            Debug.Log($"{parent.name} の Bottom の TileMapAnim.callNext を false にしました");
        }
    }

    // --- 横方向生成 ---
    public void Next()
    {
        Vector3 basePos = startPos;
        float xPos = basePos.x + 1f * (currentXIndex + 1);
        Vector3 pos = new Vector3(xPos, basePos.y, 0f);

        if (currentXIndex < xWidth)
        {
            Instantiate(loopXPrefab, pos, Quaternion.identity, transform);
        }
        else if (currentXIndex == xWidth)
        {
            GameObject last = Instantiate(lastXPrefab, pos, Quaternion.identity, transform);
            last.transform.SetParent(transform);
        }

        currentXIndex++;
    }
}
