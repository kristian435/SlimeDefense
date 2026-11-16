using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance {  get; private set; }
    private GameObject towerToBuild;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void SetTowerToBuild(GameObject tower)
    {
        towerToBuild = tower;
    }
    public GameObject GetTowerToBuild()
    {
        return towerToBuild;
    }
    public void ClearTowerToBuild()
    {
        towerToBuild = null;
    }
}
