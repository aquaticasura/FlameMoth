using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private PlayerMovement playerMovement;
    [SerializeField] private Enemy enemyData;
    [SerializeField] private float aggroRange = 5f;
    private Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        enemyData.currentHealth = enemyData.maxHealth;
        enemyData.isAgressive = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerMovement == null)
        {
            return; //put logics n stuff that requiere the player beyonder ts point
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerMovement.transform.position);
        enemyData.isAgressive = distanceToPlayer <= aggroRange;

        if (!enemyData.isAgressive)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        float dir = Mathf.Sign(playerMovement.transform.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(dir * enemyData.moveSpeed, rb.linearVelocity.y);
    }
    public void TakeDamage(float amount)
    {
        enemyData.currentHealth -= amount;
        if (enemyData.currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

}
