using UnityEngine;
using UnityEngine.AI;

public class EnemyMover : MonoBehaviour
{
    [Header("Liikkumisasetukset")]
    [SerializeField] private float moveSpeed = 3f;

    private WaypointPath waypointPath;
    private int currentWaypointIndex = 0;
    private bool hasReachedEnd = false;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = moveSpeed;
        }
        else
        {
            Debug.LogError("EnemyMover: NavMeshAgent-komponentti puuttuu viholliselta!", gameObject);
        }

        waypointPath = Object.FindAnyObjectByType<WaypointPath>();

        if (waypointPath == null)
        {
            Debug.LogError("EnemyMover: Kentältä ei löydy WaypointPath-komponenttia!", gameObject);
            enabled = false;
            return;
        }

        Transform firstPoint = waypointPath.GetWaypoint(0);
        if (firstPoint != null)
        {
            if (agent != null)
            {
                agent.Warp(firstPoint.position);
            }
            else
            {
                transform.position = firstPoint.position;
            }
        }

        SetNextWaypoint();
    }

    private void Update()
    {
        if (waypointPath == null || hasReachedEnd || agent == null) return;

        // Tarkistetaan, onko agentti saavuttanut nykyisen waypointin
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypointPath.WaypointCount)
            {
                ReachedEndOfPath();
            }
            else
            {
                SetNextWaypoint();
            }
        }
    }

    private void SetNextWaypoint()
    {
        Transform targetWaypoint = waypointPath.GetWaypoint(currentWaypointIndex);
        if (targetWaypoint != null && agent != null)
        {
            agent.SetDestination(targetWaypoint.position);
        }
    }

    private void ReachedEndOfPath()
    {
        if (hasReachedEnd) return;
        hasReachedEnd = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoseLife();
        }
        else
        {
            Debug.LogWarning("EnemyMover: Kentältä ei löytynyt GameManager-komponenttia!", gameObject);
        }
        Destroy(gameObject);
    }
}