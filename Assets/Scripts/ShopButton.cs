using UnityEngine;

public class ShopButton : MonoBehaviour
{
    [Header("Kauppa")]
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private int towerCost = 100;

    public void OnBuyButtonPressed()
    {
        TowerPlacer placer = Object.FindAnyObjectByType<TowerPlacer>();
        if (placer != null)
        {
            placer.SelectTowerToBuild(towerPrefab, towerCost);
        }
        else
        {
            Debug.LogWarning("TowerPlacer-skriptiä ei löytynyt kentästä!");
        }
    }
}
