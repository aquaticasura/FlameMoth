using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using TMPro;
public class EnemyGunScript : MonoBehaviour
{
    [SerializeField] private Transform ArmTransgoon;
    [SerializeField] private GameObject Bullet;
    [SerializeField] private Transform player;
    [SerializeField] private Enemy enemyData;

    public SpriteRenderer enemysprite;
    public SpriteRenderer arm;


    private bool isCooldown;
    public float shootForce = 10f;
    public float muzzleOffset = 0.3f;
    public float bulletDamage = 10f;


    public Vector3 playerPos;
    public bool isAimingRight;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {



   
    }
    void Awake()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
  


        playerPos = player.position;
        isAimingRight = playerPos.x >= ArmTransgoon.position.x;

        FlipEnemy(isAimingRight ? "right" : "left");
        

    }
    void FixedUpdate()
    {
        if (ArmTransgoon == null)
        {
            return;
        }

        Vector3 dir = playerPos - ArmTransgoon.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (isAimingRight)
        {
            ArmTransgoon.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        else
        {
            ArmTransgoon.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        if (enemyData.isAgressive)
        {
            if (!isCooldown)
            {
                OnShoot();
            }// ts dont work
        }

        
    }

    public void OnShoot()
    {
        if (!isCooldown)
        {

            Vector3 spawnPos = ArmTransgoon.position + ArmTransgoon.right * muzzleOffset;
            GameObject bullet = Instantiate(Bullet, spawnPos, ArmTransgoon.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.SetDamage(0);
            Collider2D shooterCollider = GetComponentInParent<Collider2D>();
            //bulletScript.IgnoreShooterCollider(shooterCollider);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.linearVelocity = ArmTransgoon.right * shootForce;
            StartCoroutine(Cooldown());



        }
    }
    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1f);
        isCooldown = false;
    }
    public void FlipEnemy(string direction)
    {
        if (direction == "right")
        {
            enemysprite.flipX = false;
            arm.flipX = false;
            arm.flipY = false;
        }
        else if (direction == "left")
        {
            enemysprite.flipX = true;
            arm.flipY = true;
        }



    }
}
