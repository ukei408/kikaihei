using UnityEngine;

public class AutoDestroyByDistance : MonoBehaviour
{
    public float maxXDistance = 20f;
    public float maxYDistance = 15f;

    private Transform mainCamera;

    void Start()
    {
        if (Camera.main != null)
        {
            mainCamera = Camera.main.transform;
        }
        else
        {
            Debug.LogError("Main Camera がシーンに存在しません！");
        }
    }

    void Update()
    {
        if (mainCamera == null) return;

        Vector3 camPos = mainCamera.position;
        Vector3 objPos = transform.position;

        // XまたはYが閾値を超えたら
        if (Mathf.Abs(objPos.x - camPos.x) >= maxXDistance ||
            Mathf.Abs(objPos.y - camPos.y) >= maxYDistance)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject); // 親を破壊
            }
            else
            {
                Destroy(gameObject); // 念のため自分も破壊
            }
        }
    }
}
