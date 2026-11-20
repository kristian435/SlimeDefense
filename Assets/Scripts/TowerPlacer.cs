using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacer : MonoBehaviour
{
    [Header("Sijoittelu ja Layerit")]
    [SerializeField] private LayerMask buildableLayerMask;
    [SerializeField] private string placedTowerLayerName = "Tower";
    [SerializeField] private float heightOffset = 0f;

    [Header("Overlap esto")]
    [SerializeField] private LayerMask occupiedTowerLayerMask;
    [SerializeField] private float occupyCheckRadius = 0.4f;

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
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                    return;
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
    private Vector3 GetSnappedPosition(RaycastHit hit)
    {
        Bounds bounds = hit.collider.bounds;
        return new Vector3(bounds.center.x,bounds.max.y + heightOffset, bounds.center.z);
    }

    private void FollowMousePosition()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildableLayerMask))
        {
            currentPreviewTower.transform.position = GetSnappedPosition(hit);
        }
    }
    private bool IsSpotOccupied(Vector3 snapPosition)
    {
        Vector3 checkCenter = snapPosition + Vector3.up * 0.5f;
        return Physics.CheckSphere(checkCenter, occupyCheckRadius, occupiedTowerLayerMask);
    }
    private void TryBuildTower()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildableLayerMask))
        {
            BuildSite buildSite = hit.collider.GetComponentInParent<BuildSite>();

            if (buildSite  == null || buildSite.IsAvailable())
            {
                Vector3 snappedPos = GetSnappedPosition(hit);

                if (IsSpotOccupied(snappedPos))
                {
                    Debug.LogWarning("Tässä ruudussa on jo torni!");
                    return;
                }
                if (GameManager.Instance != null && GameManager.Instance.SpendMoney(currentTowerCost))
                {
                    currentPreviewTower.transform.position = GetSnappedPosition(hit);
                    Transform indicator = currentPreviewTower.transform.Find("RangeIndicator");
                    if (indicator != null)
                    {
                        Destroy(indicator.gameObject);
                    }
                    Tower towerScript = currentPreviewTower.GetComponent<Tower>();
                    if (towerScript != null) towerScript.enabled = true;

                    Collider[] colliders = currentPreviewTower.GetComponentsInChildren<Collider>();
                    foreach (Collider col in colliders)
                    {
                        col.enabled = true;
                    }
                    int towerLayer = LayerMask.NameToLayer(placedTowerLayerName);
                    if (towerLayer != -1)
                    {
                        currentPreviewTower.layer = towerLayer;
                        foreach (Transform child in currentPreviewTower.GetComponentsInChildren<Transform>())
                        {
                            child.gameObject.layer = towerLayer;
                        }
                    }
                    currentPreviewTower = null;
                    currentSelectedTowerPrefab = null;
                    Debug.Log("Torni sijoitettu onnistuneesti!");
                }
                else
                {
                    Destroy(currentPreviewTower);
                    currentPreviewTower = null;
                    currentSelectedTowerPrefab= null;
                }
            }
        }
    }
}
