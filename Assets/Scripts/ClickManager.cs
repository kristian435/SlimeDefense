using UnityEngine;
using UnityEngine.EventSystems;

public class ClickManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Tower clickedTower = hit.collider.GetComponent<Tower>();

                if (clickedTower != null)
                {
                    if (GameUI.Instance != null)
                    {
                        GameUI.Instance.SelectTower(clickedTower);
                    }
                }
                else
                {
                    if (GameUI.Instance != null)
                    {
                        GameUI.Instance.DeSelectAll();
                    }
                }
            }
        }
    }
}