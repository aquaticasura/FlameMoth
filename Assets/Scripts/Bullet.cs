using UnityEngine;
using System.Collections;
public class Bullet : MonoBehaviour
{
    private float damage = 10f;
    private Collider2D bulletCollider;

    void Awake()
    {
        bulletCollider = GetComponent<Collider2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DestroyBullet());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyMovement enemy = collision.collider.GetComponentInParent<EnemyMovement>();
        if (enemy == null)
        {
            enemy = collision.collider.GetComponent<EnemyMovement>();
        }
        if (enemy == null)
        {
            enemy = collision.collider.GetComponentInChildren<EnemyMovement>();
        }
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    public void IgnoreShooterCollider(Collider2D shooterCollider)
    {
     

        Physics2D.IgnoreCollision(bulletCollider, shooterCollider, true);
        StartCoroutine(Collisionize(shooterCollider));
    }

    private IEnumerator Collisionize(Collider2D shooterCollider)
    {
        yield return new WaitForSeconds(0.2f);
        
        Physics2D.IgnoreCollision(bulletCollider, shooterCollider, false);
        
    }
}
