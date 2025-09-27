using UnityEngine;

[CreateAssetMenu(fileName = "SpriteAnimData", menuName = "Sprite/SpriteAnimData")]
public class SpriteAnimData : ScriptableObject
{
    public Sprite[] frames;        // 再生するスプライト
    public float frameRate = 0.1f; // 1フレームの表示時間
    public bool loop = true;       // ループするかどうか
}