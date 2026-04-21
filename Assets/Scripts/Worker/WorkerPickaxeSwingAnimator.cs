using UnityEngine;

public class WorkerPickaxeSwingAnimator : MonoBehaviour
{
    [Header("Swing Settings")]
    [SerializeField] private float swingAngle = 45f;
    [SerializeField] private float swingSpeed = 6f;
    [SerializeField] private Vector3 localRotationAxis = Vector3.forward;
    [SerializeField] private bool invertDirection = false;

    [Header("Return Settings")]
    [SerializeField] private float returnSpeed = 10f;

    private Quaternion initialLocalRotation;
    private bool isSwinging;

    private void Awake()
    {
        initialLocalRotation = transform.localRotation;
    }

    private void Update()
    {
        if (isSwinging)
        {
            AnimateSwing();
        }
        else
        {
            ReturnToInitialRotation();
        }
    }

    public void SetSwinging(bool value)
    {
        isSwinging = value;
    }

    private void AnimateSwing()
    {
        Vector3 axis = localRotationAxis.normalized;

        if (axis == Vector3.zero)
        {
            axis = Vector3.forward;
        }

        float direction = invertDirection ? -1f : 1f;
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle * direction;

        transform.localRotation = initialLocalRotation * Quaternion.AngleAxis(angle, axis);
    }

    private void ReturnToInitialRotation()
    {
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            initialLocalRotation,
            returnSpeed * Time.deltaTime);
    }

    private void OnDisable()
    {
        isSwinging = false;
        transform.localRotation = initialLocalRotation;
    }
}
