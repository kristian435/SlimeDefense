using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Ampumis Asetukset")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform[] firePoints;
    [SerializeField] private float range = 5f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float damage = 50f;

    [Header("K‰‰ntymis asetukset")]
    [SerializeField] private Transform partToRotate;
    [SerializeField] private float turnSpeed = 20f;

    private Transform target;
    private float fireCountdown = 0f;

    private void Start()
    {
        // Kutsutaan vihollisen etsint‰‰ toistuvasti puolen sekunnin v‰lein
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    private void Update()
    {
        if (target == null) return;

        if (partToRotate != null)
        {
            Vector3 direction = target.position - partToRotate.position;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                partToRotate.rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed);
            }
        }
        

        // Ampuminen
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    private void Shoot()
    {
        if (firePoints == null || firePoints.Length == 0) return;

        foreach (Transform firePoint in firePoints)
        {
            if (bulletPrefab != null && firePoint != null)
            {
                GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Projectile projectile = bulletGO.GetComponent<Projectile>();

                if (projectile != null)
                {
                    projectile.Seek(target, damage);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}