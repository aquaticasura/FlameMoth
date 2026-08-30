using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public static Movement Instance;
    public float JumpPower = 5;
    public Rigidbody2D rb;
    public Vector2 playerInput;
    public float move_Speed;
    public bool CanDashTimeCheck = true;//time check checks wether the cooldown for dashing is over
    public bool CanDashGroundCheck = true;// ground check checks if youve touched the ground since the last dash

    public Vector2 faux_Velocity;
    public LayerMask GroundLayer;
    public float DashMultiplier;
    private bool dashing;
        
    
    void Start()
    {
        CanDashTimeCheck = true;
        rb = GetComponent<Rigidbody2D>();
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Grounded())
        {
            CanDashGroundCheck = true;
        }
    }
    private void FixedUpdate()
    {
        
        rb.linearVelocity = new Vector2((!dashing ?(playerInput.x*move_Speed) : 0)+faux_Velocity.x, rb.linearVelocity.y);
        faux_Velocity = Vector2.Lerp(faux_Velocity, Vector2.zero, 2f * Time.fixedDeltaTime);

    }
    public void GetMovement(InputAction.CallbackContext context)
    {
        
         playerInput = context.ReadValue<Vector2>();
        
        
    }
    public void ResetVelocity()
    {
        faux_Velocity = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
    }
    public void ApplyVelocity(Vector2 applied_Vel)//use this for knockback
    {
        faux_Velocity += applied_Vel;
        rb.linearVelocity += new Vector2(0, faux_Velocity.y);
    }
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (Grounded() && ctx.started)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(new Vector2(0, JumpPower), ForceMode2D.Impulse);
        }
        
    }
    public void OnDash(InputAction.CallbackContext ctx)
    {
       
        if(CanDashTimeCheck && ctx.started && playerInput != Vector2.zero)
        {
            
            
            int count = 0;
            float tempMult = DashMultiplier;
            if (playerInput.x != 0) 
            {
                count++;
                
            }
            if (playerInput.y != 0) 
            {
                  count++;
                
            }
            
            rb.linearVelocity = Vector2.zero;
            Debug.Log(playerInput * tempMult * 2);
            ApplyVelocity(playerInput * tempMult*2);
            StartCoroutine(DashCooldown());
        }
    }
    public bool Grounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, GroundLayer);
        if(hit.collider != null)
        {
            return true;
        }
        return false;
    }
    private IEnumerator DashCooldown()
    {
        CanDashTimeCheck = false;
        dashing = true;
        rb.gravityScale = 0f;
        yield return new WaitForSeconds(0.3f);
        rb.gravityScale = 1f;
        ResetVelocity();
        dashing = false;
        yield return new WaitForSeconds(0.7f);
        CanDashTimeCheck = true;

    }

   
}
