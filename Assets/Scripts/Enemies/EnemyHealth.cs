using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] GameObject RobotExplosionVFX;
    [SerializeField] int statringHealth = 3;

    GameManager gameManager;

    int currentHealth;

    void Awake()
    {
        currentHealth = statringHealth;
    }

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        gameManager.AdjustEnemiesLeft(1);
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            gameManager.AdjustEnemiesLeft(-1);
            SelfDestruct();
        }
    }

    public void SelfDestruct()
    {
        Instantiate(RobotExplosionVFX, transform.position, Quaternion.identity);
        Destroy(this.gameObject);

    }
}
