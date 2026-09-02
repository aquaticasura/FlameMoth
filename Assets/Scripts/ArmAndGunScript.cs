using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using TMPro;
public class ArmAndGunScript : MonoBehaviour
{
    [SerializeField] private Transform ArmTransgoon;
    [SerializeField] private GameObject Bullet;
    private PlayerMovement playermovementscript;
    private Vector3 worldPos;
    private Camera mainCam;
    private bool isCooldown;
    public float shootForce = 10f;
    public float muzzleOffset = 0.3f;
    public float bulletDamage = 10f;
    public int ammo;
    public int ammocap;
    public int totalammo;
    public float recoilOffsetttoRotation;
    public Vector2 mousePos;
    public float recoilForce = 5f;
    public bool isMouseRight;

    public TMP_Text AmmoText;
    public TMP_Text totalAmmoText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ammo = 6;
        ammocap = 6;
        totalammo = 12;
        totalAmmoText.text = totalammo.ToString();
        AmmoText.text = ammo.ToString();

        mainCam = Camera.main;
    }
    void Awake(){
        playermovementscript = FindFirstObjectByType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCam == null || ArmTransgoon == null)
        {
            return;
        }


        mousePos = Mouse.current.position.ReadValue();
        worldPos = mainCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0f));
        worldPos.z = ArmTransgoon.position.z;
        isMouseRight = worldPos.x >= ArmTransgoon.position.x;

        playermovementscript.FlipSprite(isMouseRight ? "right" : "left");
        
    }
    void FixedUpdate()
    {
        if (ArmTransgoon == null)
        {
            return;
        }

        Vector3 dir = worldPos - ArmTransgoon.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if(isMouseRight){
            ArmTransgoon.rotation = Quaternion.Euler(0f, 0f, angle+recoilOffsetttoRotation);
        }else{
            ArmTransgoon.rotation = Quaternion.Euler(0f, 0f, angle-recoilOffsetttoRotation);
        }
        
        recoilOffsetttoRotation = Mathf.Lerp(recoilOffsetttoRotation, 0f, Time.fixedDeltaTime * 10f);
    }
    public void OnReload(InputAction.CallbackContext context)
    {
        if(context.performed && totalammo > 0)
        {
            if (totalammo >= ammocap)
            {
                totalammo = totalammo + ammo;
                ammo = ammocap;
                totalammo = totalammo - ammocap;
            }
            else if (totalammo < ammocap)
            {
    
                ammo = totalammo;
                totalammo = totalammo - ammo;
            }

             

            totalAmmoText.text = totalammo.ToString();
            AmmoText.text = ammo.ToString();

        }
    }
    public void OnShoot(InputAction.CallbackContext context)
    {
        if(context.performed && !isCooldown && ammo > 0)
        {
            isCooldown = true;
            ammo = ammo - 1;
            Vector3 spawnPos = ArmTransgoon.position + ArmTransgoon.right * muzzleOffset;
            GameObject bullet = Instantiate(Bullet, spawnPos, ArmTransgoon.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.SetDamage(bulletDamage);
            Collider2D shooterCollider = GetComponentInParent<Collider2D>();
            bulletScript.IgnoreShooterCollider(shooterCollider);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.linearVelocity = ArmTransgoon.right * shootForce;
            recoilOffsetttoRotation += 10f;
            Vector2 recoilDirection = -ArmTransgoon.right.normalized * recoilForce;
            playermovementscript.GetRecoiled(recoilDirection);
            StartCoroutine(Cooldown());
            AmmoText.text = ammo.ToString();

        }
    }
    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(0.5f);
        isCooldown = false;
    }

}
