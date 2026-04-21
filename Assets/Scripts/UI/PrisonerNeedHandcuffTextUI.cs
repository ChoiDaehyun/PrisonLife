using UnityEngine;

public class PrisonerNeedHandcuffTextUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PrisonerUnit ownerPrisoner;
    [SerializeField] private PrisonerQueueManager queueManager;
    [SerializeField] private TextMesh needText;
    [SerializeField] private GameObject visualRoot;

    public void Initialize(PrisonerUnit prisonerUnit, PrisonerQueueManager prisonerQueueManager)
    {
        ownerPrisoner = prisonerUnit;
        queueManager = prisonerQueueManager;
    }

    private void OnEnable()
    {
        if (queueManager != null)
        {
            queueManager.OnServicePrisonerChanged += HandleServicePrisonerChanged;
        }

        if (ownerPrisoner != null)
        {
            ownerPrisoner.OnHandcuffProgressChanged += HandleHandcuffProgressChanged;
            ownerPrisoner.OnPrisonerStateChanged += HandlePrisonerStateChanged;
        }
    }

    private void OnDisable()
    {
        if (queueManager != null)
        {
            queueManager.OnServicePrisonerChanged -= HandleServicePrisonerChanged;
        }

        if (ownerPrisoner != null)
        {
            ownerPrisoner.OnHandcuffProgressChanged -= HandleHandcuffProgressChanged;
            ownerPrisoner.OnPrisonerStateChanged -= HandlePrisonerStateChanged;
        }
    }

    private void Start()
    {
        RefreshVisibilityAndText();
    }

    private void HandleServicePrisonerChanged(PrisonerUnit currentServicePrisoner)
    {
        RefreshVisibilityAndText();
    }

    private void HandleHandcuffProgressChanged(int received, int required)
    {
        RefreshVisibilityAndText();
    }

    private void HandlePrisonerStateChanged(PrisonerUnit prisoner)
    {
        if (prisoner != ownerPrisoner)
        {
            return;
        }

        RefreshVisibilityAndText();
    }

    private void RefreshVisibilityAndText()
    {
        if (visualRoot == null || ownerPrisoner == null || queueManager == null || needText == null)
        {
            return;
        }

        bool shouldShow =
            queueManager.CurrentServicePrisoner == ownerPrisoner &&
            ownerPrisoner.IsAtServicePoint &&
            !ownerPrisoner.IsMovingToJail &&
            ownerPrisoner.RemainingHandcuffCount > 0;

        if (visualRoot != null)
        {
            visualRoot.SetActive(shouldShow);
        }
        else
        {
            needText.gameObject.SetActive(shouldShow);
        }


        if (!shouldShow)
        {
            return;
        }

        needText.text = $"{ownerPrisoner.RemainingHandcuffCount}";
    }
}