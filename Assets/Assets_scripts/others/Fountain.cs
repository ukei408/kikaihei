using UnityEngine;

public class Fountain : MonoBehaviour
{
    [Header("Initial settings")]
    public float initialSpeed = 6f;       // 発射時の速度
    [Range(0f, 180f)]
    public float angleDegrees = 60f;     // 発射角度（度）: 0 = 右水平, 90 = 上
    public float gravity = 9.81f;        // 重力加速度（正の値）

    [Header("Optional")]
    public bool rotateToVelocity = true; // 速度方向にスプライトを回転させる
    public bool randomizeAngle = false;  // 少しばらつきを与える
    public float angleSpread = 10f;      // ばらつき幅（度）

    Vector2 velocity;

    void Start()
    {
        float angle = angleDegrees;
        if (randomizeAngle)
            angle += Random.Range(-angleSpread * 0.5f, angleSpread * 0.5f);

        float rad = angle * Mathf.Deg2Rad;
        velocity = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * initialSpeed;
    }

    void Update()
    {
        // 重力による加速
        velocity += Vector2.down * gravity * Time.deltaTime;

        // 位置更新
        transform.position += (Vector3)(velocity * Time.deltaTime);

        // 見た目を速度方向に回転
        if (rotateToVelocity && velocity.sqrMagnitude > 0.0001f)
        {
            float ang = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, ang);
        }
    }
}
