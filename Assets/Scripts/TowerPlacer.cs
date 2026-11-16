using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    private GameObject currentPreviewTower;
    private int currentTowerCost;
    private GameObject currentSelectedTowerPrefab;
    private Camera cam;
    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (currentPreviewTower != null)
        {
            FollowMousePosition();

            if (Input.GetMouseButtonDown(0))
            {
                TryBuildTower();
            }
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(currentPreviewTower);
                currentPreviewTower = null;
                currentSelectedTowerPrefab = null;
            }
        }
    }
    
    public void SelectTowerToBuild(GameObject towerPrefab, int cost)
    {
        currentTowerCost = cost;
        currentSelectedTowerPrefab = towerPrefab;

        if (GameManager.Instance != null && GameManager.Instance.PlayerMoney < currentTowerCost)
        {
            Debug.LogWarning("Ei tarpeeksi rahaa torniin!");
            return;
        }
        if (currentPreviewTower != null)
        {
            Destroy(currentPreviewTower);
        }
        currentPreviewTower = Instantiate(towerPrefab);

        Tower towerScript = currentPreviewTower.GetComponent<Tower>();
        if (towerScript != null) towerScript.enabled = false;

        Collider[] colliders = currentPreviewTower.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
    }

    private void FollowMousePosition()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 spawnPos = new Vector3(hit.point.x,hit.point.y + 0.5f, hit.point.z);
            currentPreviewTower.transform.position = spawnPos;
        }
    }
    private void TryBuildTower()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            BuildSite buildSite = hit.collider.GetComponent<BuildSite>();

            if (buildSite  != null && buildSite.IsAvailable())
            {
                if (GameManager.Instance != null && GameManager.Instance.SpendMoney(currentTowerCost))
                {
                    Transform indicator = currentPreviewTower.transform.Find("RangeIndicator");
                    if (indicator != null)
                    {
                        Destroy(indicator.gameObject);
                    }
                    Tower towerScript = currentPreviewTower.GetComponent<Tower>();
                    if (towerScript != null) towerScript.enabled = true;
                    currentPreviewTower = null;
                    Debug.Log("Torni sijoitettu onnistuneesti!");
                }
                else
                {
                    Destroy(currentPreviewTower);
                    currentPreviewTower = null;
                }
            }
        }
    }
}
