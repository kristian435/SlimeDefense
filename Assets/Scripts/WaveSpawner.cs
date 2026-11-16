using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class WaveData
    {
        public string waveName = "aalto";
        public GameObject enemyPrefab;
        public int count = 5;
    }
    [Header("Asetukset")]
    [SerializeField] private WaveData[] waves;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float spawnInterval = 1f;

    [Header("Voittoasetukset")]
    [SerializeField] private int totalWaves = 20;

    private float countdown = 2f;
    public int waveNumber = 0;
    private bool allWavesSpawned = false;
    private bool isSpawning = false;

    // Update is called once per frame
    private void Update()
    {
        if (allWavesSpawned || isSpawning) return;

        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }
        countdown -= Time.deltaTime;
    }
    private IEnumerator SpawnWave()
    {
        if (waveNumber >= waves.Length)
        {
            allWavesSpawned = true;
            yield break;
        }
        isSpawning = true;
        WaveData currentWave = waves[waveNumber];
        waveNumber++;
        Debug.Log($"Aalto {waveNumber} ({currentWave.waveName}) alkaa! Vihollisia tulossa: {currentWave.count}");

        for (int i = 0; i < currentWave.count; i++)
        {
            SpawnEnemy(currentWave.enemyPrefab);
            yield return new WaitForSeconds(spawnInterval);
        }
        isSpawning = false;
        if (waveNumber >= waves.Length)
        {
            allWavesSpawned =true;
            Debug.Log("Viimeinen aalto lähetetty matkaan!");
        }
    }
    private void SpawnEnemy(GameObject prefabToSpawn)
    {
        if (prefabToSpawn == null)
        {
            Debug.LogWarning("WaveSpawner: Enemy Prefab puuttuu!");
            return;
        }
        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;
        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
    }
    public bool AllWavesFinished()
    {
        if (allWavesSpawned && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            return true;
        }
        return false;
    }
}
