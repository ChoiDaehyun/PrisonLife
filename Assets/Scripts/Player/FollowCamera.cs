using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Follow Settings")]
    [SerializeField] private Vector3 offset = new Vector3(5f, 10f, -7f);
    [SerializeField] private float followSmoothness = 10f; //따라갈 때 부드러운 정도
    [SerializeField] private float lookSmoothness = 10f; //회전 시 부드러운 정도

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp( //위치 보간(부드럽게)
            transform.position,
            desiredPosition,
            followSmoothness * Time.deltaTime
        );

        Vector3 lookDirection = (target.position - transform.position).normalized;

        if (lookDirection.sqrMagnitude > 0.0001f) //의미 있는 방향만 계산
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp( //회전 보간(부드럽게)
                transform.rotation,
                targetRotation,
                lookSmoothness * Time.deltaTime
            );
        }
    }
}
