using UnityEngine;

public class PickaxeSwingVisual : MonoBehaviour
{
    [Header("Swing Settings")]
    [SerializeField] private float swingAngle = 45f;
    [SerializeField] private float swingSpeed = 6f;
    [SerializeField] private Vector3 swingAxis = Vector3.forward;

    [Header("Runtime")]
    [SerializeField] private bool isSwinging;

    private Quaternion initialLocalRotation;

    private void Awake()
    {
        initialLocalRotation = transform.localRotation;
    }

    private void Update()
    {
        if (!isSwinging)
        {
            return;
        }

        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        Quaternion swingRotation = Quaternion.AngleAxis(angle, swingAxis.normalized);

        transform.localRotation = initialLocalRotation * swingRotation;
    }

    public void StartSwing()
    {
        isSwinging = true;
    }
    public void StopSwing()
    {
        isSwinging = false;
        transform.localRotation = initialLocalRotation;
    }

    private void OnDisable()
    {
        StopSwing();
    }
}