using UnityEngine;

public class BuildSite : MonoBehaviour
{
    private GameObject builtTower;

    public bool IsAvailable()
    {
        return builtTower == null;
    }
    public void SetBuiltTower(GameObject tower)
    {
        builtTower = tower;
    }
    public GameObject GetBuiltTower()
    {
        return builtTower;
    }
}
