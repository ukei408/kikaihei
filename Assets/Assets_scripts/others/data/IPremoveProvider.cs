using UnityEngine;

public interface IPremoveProvider
{
    /// <summary>
    /// Premoveを処理し、準備が完了したら true を返す
    /// </summary>
    bool Premove(Transform bulletTransform, ref float speed, Vector2 moveDir);
}
