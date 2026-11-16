using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Pelaajan tiedot")]
    [SerializeField] private int playerLives = 100;
    [SerializeField] private int playerMoney = 200;

    [Header("UI viittaukset")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;

    public static GameManager Instance { get; private set; }
    private bool isGameOver = false;
    private bool isGameWon = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Update()
    {
        if (isGameOver || isGameWon) return;
        WaveSpawner spawner = FindAnyObjectByType<WaveSpawner>();
        if (spawner != null && spawner.AllWavesFinished())
        {
            HandleVictory();
        }
    }
    public void LoseLife(int damage = 1)
    {
        if (isGameOver || isGameWon) return;
        playerLives -= damage;
        Debug.Log($"Vihollinen p‰‰si maaliin! El‰m‰pisteit‰ j‰ljell‰: {playerLives}");

        if (playerLives <= 0)
        {
            playerLives = 0;
            HandleGameOver();
        }
    }
    private void HandleGameOver()
    {
        isGameOver = true;
        Debug.LogWarning("Peli ohi! Pelaajan el‰m‰pisteet loppuivat.");

        if (gameOverPanel  != null)
        {
            gameOverPanel.SetActive(true);
        }
        Time.timeScale = 0f;
    }
    private void HandleVictory()
    {
        isGameWon = true;

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
        Time.timeScale = 0f;
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddMoney(int amount)
    {
        playerMoney += amount;
        Debug.Log($"Vihollinen tuhottu! Rahaa ansaitettu: +{amount}. Yhteens‰ rahaa: {playerMoney}");
    }

    public bool SpendMoney(int amount)
    {
        if (playerMoney >= amount)
        {
            playerMoney -= amount;
            return true;
        }
        Debug.LogWarning("Ei tarpeeksi rahaa!");
        return false;
    }

    public int PlayerLives => playerLives;
    public int PlayerMoney => playerMoney;
}
