using UnityEngine;

[CreateAssetMenu(fileName = "SpriteMoveData", menuName = "Sprite/SpriteMoveData")]
public class SpriteMoveData : ScriptableObject
{
    public Sprite[] frames;        // 再生するスプライト
    public float frameRate = 0.1f; // 1フレームの表示時間
    public bool loop = true;       // ループするかどうか
}