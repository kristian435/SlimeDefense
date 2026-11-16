using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerUI : MonoBehaviour
{
    public GameObject ui;
    private Tower targetTower;

    [Header("UI")]
    public TextMeshProUGUI upgradeCostText;
    public Button upgradeButton;

    public void SetTarget(Tower tower)
    {
        targetTower = tower;
        ui.SetActive(true);
    }
    public void Hide()
    {
        ui.SetActive(false);
    }
    public void Upgrade()
    {
        Hide();
    }
    public void Sell()
    {
        Hide();
    }
}
