using UnityEngine;
using UnityEngine.EventSystems;

public class TowerSelection : MonoBehaviour
{
    public TowerManager manager;
    public LayerMask towerLayerMask;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, towerLayerMask))
            {
                Tower tower = hit.collider.GetComponent<Tower>();
                if (tower != null && manager != null)
                {
                    manager.ShowTower(tower);
                }
            }
            else
            {
                if (manager != null)
                {
                    manager.ClosePanel();
                }
            }
        }
    }
}
