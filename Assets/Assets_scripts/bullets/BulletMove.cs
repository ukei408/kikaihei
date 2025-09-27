using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float speed = 5f;
    private Transform target;
    public bool OnPlaced = false;

    private Vector2 moveDir;
    private bool initialized = false;

    void Update()
    {
        if (!OnPlaced) return;
        MoveStart();
    }

    private void MoveStart()
    {
        if (!initialized)
        {
            Transform parent = transform.parent;
            if (parent != null)
            {
                Transform found = parent.Find("Target");
                if (found != null)
                {
                    target = found;
                    moveDir = ((Vector2)target.position - (Vector2)transform.position).normalized;
                    initialized = true;
                }
            }

            if (target == null)
            {
                Debug.LogWarning("Target が見つかりません。");
                return;
            }
        }

        transform.position += (Vector3)(moveDir * speed * Time.deltaTime);
    }
}
