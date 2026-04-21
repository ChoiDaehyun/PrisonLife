using UnityEngine;

public class PlayerInventoryFullTextUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerRoot;
    [SerializeField] private TextMesh fullText;

    [Header("Display Settings")]
    [SerializeField] private float visibleDuration = 0.5f;
    [SerializeField] private float forwardOffset = 0.8f;
    [SerializeField] private float heightOffset = 0f;
    [SerializeField] private string displayMessage = "FULL";

    [Header("Runtime")]
    [SerializeField] private float remainingTime;
    [SerializeField] private bool isShowing;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;

        if (fullText != null)
        {
            fullText.text = displayMessage;
            fullText.gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        if (!isShowing || fullText == null)
        {
            return;
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        UpdateWorldPosition();
        FaceCamera();

        remainingTime -= Time.deltaTime;

        if (remainingTime > 0f)
        {
            return;
        }

        Hide();
    }

    public void Show()
    {
        if (fullText == null)
        {
            return;
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        isShowing = true;
        remainingTime = visibleDuration;

        fullText.text = displayMessage;
        fullText.gameObject.SetActive(true);

        UpdateWorldPosition();
        FaceCamera();
    }

    public void Hide()
    {
        isShowing = false;
        remainingTime = 0f;

        if (fullText != null)
        {
            fullText.gameObject.SetActive(false);
        }
    }

    private void UpdateWorldPosition()
    {
        if (playerRoot == null || fullText == null)
        {
            return;
        }

        Vector3 forward = playerRoot.forward;

        if (mainCamera != null)
        {
            forward = mainCamera.transform.forward;
            forward.y = 0f;

            if (forward.sqrMagnitude < 0.0001f)
            {
                forward = playerRoot.forward;
            }
        }

        forward.Normalize();

        Vector3 targetPosition =
            playerRoot.position +
            Vector3.up * heightOffset +
            forward * forwardOffset;

        fullText.transform.position = targetPosition;
    }

    private void FaceCamera()
    {
        if (mainCamera == null || fullText == null)
        {
            return;
        }

        fullText.transform.forward = mainCamera.transform.forward;
    }

    private void OnDisable()
    {
        Hide();
    }
}
