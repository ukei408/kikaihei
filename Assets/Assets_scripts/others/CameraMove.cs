using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Playerタグを持つオブジェクトが見つかりませんでした");
        }
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // プレイヤーにぴったり固定
            Vector3 targetPos = player.position;
            targetPos.z = transform.position.z;

            // カメラ位置を直接セット
            transform.position = targetPos;
        }
    }
}
