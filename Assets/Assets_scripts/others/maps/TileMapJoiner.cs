using UnityEngine;

public class TileMapJoiner : MonoBehaviour
{
    [Header("Joiner用 Prefabs")]
    public GameObject joinerTopBottomPrefab;
    public GameObject joinerCenterPrefab;

    private void Start()
    {
        TileMapGenerator generator = transform.parent.GetComponentInChildren<TileMapGenerator>();
        if (generator == null)
        {
            Debug.LogError("兄弟階層に TileMapGenerator が見つかりません");
            return;
        }

        // Joiner用のPrefabを渡して生成
        generator.GenerateVertical(transform, transform.position, generator.yWidth,
                                   joinerTopBottomPrefab, joinerCenterPrefab);
    }
}
