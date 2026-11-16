using UnityEngine;

public class TowerClick : MonoBehaviour
{
    private TowerUI towerUI;
    void Start()
    {
        towerUI = FindObjectOfType<TowerUI>();
    }
    private void OnMouseDown()
    {
        Tower towerComp = GetComponent<Tower>();
        if (towerComp != null && towerUI != null)
        {
            towerUI.SetTarget(towerComp);
        }
    }
}
