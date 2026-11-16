using UnityEngine;

public class WaypointPath : MonoBehaviour
{
    [Header("Radan Pisteet")]
    [SerializeField] private Transform[] waypoints;

    private void Awake()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("WaypointPath: Radalle ei ole m‰‰ritetty yht‰‰n waypoint-pistett‰!", gameObject);
        }
    }
    public Transform GetWaypoint(int index)
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("WaypointPath: Waypoint-taulukko on tyhj‰!", gameObject);
            return null;
        }
        if (index < 0 || index >= waypoints.Length)
        {
            Debug.LogWarning($"WaypointPath: Yritettiin hakea virheellinen waypoint-indeksi: {index}. Maksimi on {waypoints.Length - 1}.");
            return null;
        }
        return waypoints[index];
    }
    public int WaypointCount
    {
        get
        {
            if (waypoints == null) return 0;
            return waypoints.Length;
        }
    }
}
