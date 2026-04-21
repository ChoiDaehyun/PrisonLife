using UnityEngine;

public class DrillMoveVisual : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private Vector3 startLocalPosition = new Vector3(0f, -0.239999995f, 1.5f);
    [SerializeField] private Vector3 endLocalPosition = new Vector3(0f, -0.239999995f, 1.86000001f);
    [SerializeField] private float moveSpeed = 4f;

    [Header("Runtime")]
    [SerializeField] private bool isMoving;

    private void Awake()
    {
        transform.localPosition = startLocalPosition;
    }

    private void Update()
    {
        if (!isMoving)
        {
            return;
        }

        float t = Mathf.PingPong(Time.time * moveSpeed, 1f);
        transform.localPosition = Vector3.Lerp(startLocalPosition, endLocalPosition, t);
    }

    /// <summary>
    /// Start drill move animation.
    /// </summary>
    public void StartMove()
    {
        isMoving = true;
    }

    /// <summary>
    /// Stop drill move animation and restore initial local position.
    /// </summary>
    public void StopMove()
    {
        isMoving = false;
        transform.localPosition = startLocalPosition;
    }

    private void OnDisable()
    {
        StopMove();
    }
}