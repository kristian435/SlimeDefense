using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("UI tekstit")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI livesText;

    [Header("Viittaukset")]
    [SerializeField] private WaveSpawner waveSpawner;

    private void Update()
    {
        UpdateWaveUI();
        UpdatePlayerStatsUI();
    }
    private void UpdateWaveUI()
    {
        if (waveText  != null && waveSpawner != null)
        {
            waveText.text = "Round: " + waveSpawner.waveNumber;
        }
    }
    private void UpdatePlayerStatsUI()
    {
        if (GameManager.Instance != null)
        {
            if (moneyText != null)
            {
                moneyText.text = "$ " + GameManager.Instance.PlayerMoney;
            }
            if (livesText != null)
            {
                livesText.text = "HP: " + GameManager.Instance.PlayerLives;
            }
        }
    }
}
