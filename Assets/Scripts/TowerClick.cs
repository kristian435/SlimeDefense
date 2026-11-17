using UnityEngine;

public class TowerClick : MonoBehaviour
{
    private Tower towerComp;
    void Start()
    {
        towerComp = GetComponent<Tower>();
    }
    private void OnMouseDown()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;
        if (towerComp  != null && GameUI.Instance != null)
        {
            GameUI.Instance.SelectTower(towerComp);
        }
    }
}
