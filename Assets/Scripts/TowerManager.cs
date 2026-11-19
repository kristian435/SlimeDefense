using UnityEngine;
using TMPro;

public class TowerManager : MonoBehaviour
{
    public GameObject towerPanel;

    [Header("Shop Integraatio")]
    public GameObject shopPanel;


    [Header("UI tekstit")]
    public TMP_Text towerNameText;
    public TMP_Text damageText;
    public TMP_Text rangeText;
    public TMP_Text upgradeCostText;

    private Tower selectedTower;

    void Start()
    {
        if (towerPanel != null)
            towerPanel.SetActive(false);
    }
    public void ShowTower(Tower tower)
    {
        selectedTower = tower;
        if (towerPanel != null) towerPanel.SetActive(true);
        if (shopPanel != null) shopPanel.SetActive(false);

        UpdatePanel();
    }
    public void UpdatePanel()
    {
        if (selectedTower == null)
            return;

        if (towerNameText != null) towerNameText.text = selectedTower.name;
        if (damageText != null) damageText.text = "Damage: " + selectedTower.damage;
        if (rangeText != null) rangeText.text = "Range: " + selectedTower.range;
        if (upgradeCostText != null) upgradeCostText.text = "Upgrade: $" + selectedTower.upgradeCost;
    }
    public void UpgradeTower()
    {
        if (selectedTower == null)
            return;

        if (GameManager.Instance != null)
        {
            bool success = GameManager.Instance.SpendMoney(selectedTower.upgradeCost);
            if (success)
            {
                selectedTower.Upgrade();
                UpdatePanel();
                Debug.Log("Torni päivitetty");
            }
            else
            {
                Debug.Log("Ei tarpeeksi rahaa päivitykseen");
            }
        }
    }
    public void SellTower()
    {
        if (selectedTower == null)
            return;

        if (GameManager.Instance != null)
        {
            int refund = selectedTower.upgradeCost / 2;
            GameManager.Instance.AddMoney(refund);
        }
        Destroy(selectedTower.gameObject);
        ClosePanel();
    }
    public void ClosePanel()
    {
        if (towerPanel != null) towerPanel.SetActive(false);
        if (shopPanel != null) shopPanel.SetActive(true);
        selectedTower = null;
    }
}