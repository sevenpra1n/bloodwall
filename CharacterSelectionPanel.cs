using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays two character selection icons (Knight / Archer).
/// Wire up in Unity Inspector:
///   - knightButton / archerButton  – the two clickable icon buttons
///   - archerLockOverlay            – grey overlay shown when Archer is locked
///   - characterDetailPanel         – opened when an unlocked character is clicked
///   - purchaseConfirmPanel         – opened when the locked Archer is clicked
///   - equipmentUIManager           – needed to refresh coin display after purchase
/// </summary>
public class CharacterSelectionPanel : MonoBehaviour
{
    [SerializeField] private Button knightButton;
    [SerializeField] private Button archerButton;
    [SerializeField] private Image  archerLockOverlay;   // semi-transparent grey overlay
    [SerializeField] private Text   archerPriceText;     // optional "5000" label on locked icon

    [SerializeField] private CharacterDetailPanel  characterDetailPanel;
    [SerializeField] private PurchaseConfirmPanel  purchaseConfirmPanel;
    [SerializeField] private EquipmentUIManager    equipmentUIManager;

    private void Start()
    {
        if (knightButton != null)
            knightButton.onClick.AddListener(OnKnightClicked);

        if (archerButton != null)
            archerButton.onClick.AddListener(OnArcherClicked);

        RefreshUI();
    }

    private void OnEnable()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (CharacterManager.Instance == null) return;

        bool archerUnlocked = CharacterManager.Instance.IsCharacterUnlocked(CharacterType.Archer);

        if (archerLockOverlay != null)
            archerLockOverlay.gameObject.SetActive(!archerUnlocked);

        if (archerPriceText != null)
        {
            CharacterData archerData = CharacterManager.Instance.GetCharacterData(CharacterType.Archer);
            archerPriceText.gameObject.SetActive(!archerUnlocked);
            archerPriceText.text = archerData != null ? archerData.price.ToString() : "5000";
        }
    }

    private void OnKnightClicked()
    {
        if (CharacterManager.Instance == null) return;
        OpenDetailPanel(CharacterType.Knight);
    }

    private void OnArcherClicked()
    {
        if (CharacterManager.Instance == null) return;

        if (CharacterManager.Instance.IsCharacterUnlocked(CharacterType.Archer))
        {
            OpenDetailPanel(CharacterType.Archer);
        }
        else
        {
            OpenPurchasePanel();
        }
    }

    private void OpenDetailPanel(CharacterType type)
    {
        if (characterDetailPanel == null) return;
        characterDetailPanel.gameObject.SetActive(true);
        characterDetailPanel.Show(type, this);
    }

    private void OpenPurchasePanel()
    {
        if (purchaseConfirmPanel == null) return;
        purchaseConfirmPanel.gameObject.SetActive(true);
        purchaseConfirmPanel.Show(CharacterType.Archer, this, equipmentUIManager);
    }

    public void OnPurchaseComplete()
    {
        RefreshUI();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
