using System.Collections;
using UnityEngine;

public class DelayedCanvasShow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject targetCanvasObject;
    [SerializeField] private GameObject moneyCanvas;
    [SerializeField] private GameObject joystickCanvas;

    [Header("Settings")]
    [SerializeField] private float showDelay = 3f;

    [Header("Runtime")]
    [SerializeField] private bool hasShownOnce;

    private Coroutine showRoutine;

    private void Awake()
    {
        if (targetCanvasObject != null)
        {
            targetCanvasObject.SetActive(false);
        }
    }

    public void ShowCanvasAfterDelay()
    {
        if (hasShownOnce)
        {
            return;
        }

        if (targetCanvasObject == null)
        {
            return;
        }

        if (showRoutine != null)
        {
            StopCoroutine(showRoutine);
        }

        showRoutine = StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        yield return new WaitForSeconds(showDelay);

        moneyCanvas.SetActive(false);
        joystickCanvas.SetActive(false);
        targetCanvasObject.SetActive(true);
        hasShownOnce = true;
        showRoutine = null;
    }

    private void OnValidate()
    {
        if (showDelay < 0f)
        {
            showDelay = 0f;
        }
    }
}