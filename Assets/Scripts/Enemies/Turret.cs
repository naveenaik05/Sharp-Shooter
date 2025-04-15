using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform turretHead;
    [SerializeField] Transform playerTarget;
    [SerializeField] Transform projectileSpawnPoint;
    
    [SerializeField] float fireRate;
    [SerializeField] int damage = 2;

    PlayerHealth player;

    void Start()
    {
        player = FindFirstObjectByType<PlayerHealth>();
        StartCoroutine(FireRoutine());
    }

    void Update()
    {
        turretHead.LookAt(playerTarget);
    }

    IEnumerator FireRoutine()
    {
        while(player)
        {
            yield return new WaitForSeconds(fireRate);
            Projectile newProjectile =  Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity).GetComponent<Projectile>();
            newProjectile.transform.LookAt(playerTarget);
            newProjectile.Init(damage);
        }
    }
}
