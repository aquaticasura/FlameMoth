using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Diagnostics;
//Script brought to you by the flipping goat
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Character Stats")]
    public float stamina = 50f;
    public float maxhealth = 100f;
    public float health;

    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private SpriteRenderer spriteRenderer;
    [SerializeField] SpriteRenderer armsprite;
    [SerializeField] ArmAndGunScript mouse;

    [Header("Input")]
    public Vector2 moveInput;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float maxSpeed = 5f;
    public float acceleration = 35f;
    public float deceleration = 25f;
    public float rollForce = 10f;
    public float airControlMultiplier = 0.6f;

    [Header("Jump")]
    public float jumpForce = 10f;
    public int maxJumpCount = 2;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public Vector2 groundCheckSize = new Vector2(1f, 1f);
    public float groundCheckDistance = 0.1f;
    public Color groundCheckGizmoColor = Color.green;

    [Header("Wall Check")]
    public Vector2 wallCheckSize = new Vector2(1f, 1f);
    public float wallCheckDistance = 0.1f;
    public float wallCheckHorizontalOffset = 0.5f;
    public Color wallCheckGizmoColor = Color.cyan;

    [Header("Wall Jump")]
    public int maxWallJumps = 1;
    public float wallJumpHorizontalForce = 8f;
    public float wallJumpVerticalForce = 10f;

 
    private ParticleSystem fart;
    private bool isPressingMove;
    private bool isRolling;
    private int facingdirection = 1;
    private Vector2 recoilOffsett;
    private int jumpCount = 0;
    float r;

    private int wallJumpsRemaining;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        fart = GetComponentInChildren<ParticleSystem>();
    }

    void OnEnable()
    {
        if (playerInput == null)
        {
            return;
        }

        playerInput.ActivateInput();
        if (playerInput.currentActionMap == null)
        {
            playerInput.SwitchCurrentActionMap("Player");
        }

        wallJumpsRemaining = maxWallJumps;
    }

    // Update is called once per frame
    void Update()
    {
        if (Grounded())
        {
            jumpCount = 0;
            wallJumpsRemaining = maxWallJumps;
        }

        isPressingMove = Mathf.Abs(moveInput.x) > 0.01f;
        if (moveInput.x > 0.01f) facingdirection = 1;
        if (moveInput.x < -0.01f) facingdirection = -1;

        if (mouse.mousePos.x < 960 && facingdirection > 0)
        {
            FlipSprite("left");
        }
        else //(mouse.mousePos.x > 960 && facingdirection < 0)
        {
            FlipSprite("right");
        }

 
    }
    void FixedUpdate(){
        Vector2 velocity = rb.linearVelocity;
        float targetSpeed = moveInput.x * maxSpeed;
        float tempAccel = isPressingMove ? acceleration : deceleration;
        if (!Grounded())
        {
            tempAccel *= airControlMultiplier;
        }
        velocity.x = Mathf.MoveTowards(velocity.x, targetSpeed, tempAccel * Time.fixedDeltaTime);

        rb.linearVelocity = velocity + recoilOffsett;
        recoilOffsett = Vector2.Lerp(recoilOffsett, Vector2.zero, Time.fixedDeltaTime * 10f);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        
    }
    public void OnJump(InputAction.CallbackContext context){
        if (!context.performed) return;
        bool isGrounded = Grounded();
        bool onLeftWall = WallLeft();
        bool onRightWall = WallRight();

        if (!isGrounded && wallJumpsRemaining > 0 && (onLeftWall || onRightWall))
        {
            rb.linearVelocity = new Vector2(0f, 0f);

            int wallSide = 0; // faggot
            if (onLeftWall && !onRightWall) wallSide = -1;
            if (onRightWall && !onLeftWall) wallSide = 1;
            if (wallSide == 0) wallSide = facingdirection > 0 ? 1 : -1;

            Vector2 wallJumpVelocity = new Vector2(-wallSide * wallJumpHorizontalForce, wallJumpVerticalForce);
            rb.AddForce(wallJumpVelocity, ForceMode2D.Impulse);
            wallJumpsRemaining--;
            jumpCount--;
            return;
        }

        if (isGrounded || jumpCount < maxJumpCount)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;
            return;
        }
    }
    public bool Grounded(){
        return Physics2D.BoxCast(transform.position, groundCheckSize, 0f, Vector2.down, groundCheckDistance, groundLayer);
    }

    public bool WallLeft()
    {
        Vector2 origin = (Vector2)transform.position + Vector2.left * wallCheckHorizontalOffset;
        return Physics2D.BoxCast(origin, wallCheckSize, 0f, Vector2.left, wallCheckDistance, groundLayer);
    }

    public bool WallRight()
    {
        Vector2 origin = (Vector2)transform.position + Vector2.right * wallCheckHorizontalOffset;
        return Physics2D.BoxCast(origin, wallCheckSize, 0f, Vector2.right, wallCheckDistance, groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = groundCheckGizmoColor;

        Vector3 startCenter = transform.position;
        Vector3 endCenter = startCenter + Vector3.down * groundCheckDistance;
        Vector3 half = new Vector3(groundCheckSize.x * 0.5f, groundCheckSize.y * 0.5f, 0f);

        Gizmos.DrawWireCube(startCenter, groundCheckSize);
        Gizmos.DrawWireCube(endCenter, groundCheckSize);

        Gizmos.DrawLine(startCenter + new Vector3(-half.x, half.y, 0f), endCenter + new Vector3(-half.x, half.y, 0f));
        Gizmos.DrawLine(startCenter + new Vector3(half.x, half.y, 0f), endCenter + new Vector3(half.x, half.y, 0f));
        Gizmos.DrawLine(startCenter + new Vector3(-half.x, -half.y, 0f), endCenter + new Vector3(-half.x, -half.y, 0f));
        Gizmos.DrawLine(startCenter + new Vector3(half.x, -half.y, 0f), endCenter + new Vector3(half.x, -half.y, 0f));

        Gizmos.color = wallCheckGizmoColor;

        Vector3 leftStartCenter = startCenter + Vector3.left * wallCheckHorizontalOffset;
        Vector3 rightStartCenter = startCenter + Vector3.right * wallCheckHorizontalOffset;
        Vector3 leftEndCenter = leftStartCenter + Vector3.left * wallCheckDistance;
        Vector3 rightEndCenter = rightStartCenter + Vector3.right * wallCheckDistance;
        Vector3 wallHalf = new Vector3(wallCheckSize.x * 0.5f, wallCheckSize.y * 0.5f, 0f);

        Gizmos.DrawWireCube(leftStartCenter, wallCheckSize);
        Gizmos.DrawWireCube(rightStartCenter, wallCheckSize);
        Gizmos.DrawWireCube(leftEndCenter, wallCheckSize);
        Gizmos.DrawWireCube(rightEndCenter, wallCheckSize);

        Gizmos.DrawLine(leftStartCenter + new Vector3(-wallHalf.x, wallHalf.y, 0f), leftEndCenter + new Vector3(-wallHalf.x, wallHalf.y, 0f));
        Gizmos.DrawLine(leftStartCenter + new Vector3(-wallHalf.x, -wallHalf.y, 0f), leftEndCenter + new Vector3(-wallHalf.x, -wallHalf.y, 0f));
        Gizmos.DrawLine(rightStartCenter + new Vector3(wallHalf.x, wallHalf.y, 0f), rightEndCenter + new Vector3(wallHalf.x, wallHalf.y, 0f));
        Gizmos.DrawLine(rightStartCenter + new Vector3(wallHalf.x, -wallHalf.y, 0f), rightEndCenter + new Vector3(wallHalf.x, -wallHalf.y, 0f));
    }
    public void OnRoll(InputAction.CallbackContext context){
        if(context.performed && !isRolling){
            StartCoroutine(Roll());
        }
    }
    private IEnumerator Roll(){
        isRolling = true;
        rb.AddForce(Vector2.right * moveInput.x * rollForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.8f);
        isRolling = false;
    }

    public void FlipSprite(string direction)
    {
        if (direction == "right")
        {
            spriteRenderer.flipX = false;
            armsprite.flipX = false;
            armsprite.flipY = false;
        }
        else if (direction == "left")
        {
            spriteRenderer.flipX = true;
            armsprite.flipY = true;
        }



    }
    public void GetRecoiled(Vector2 direction)
    {
        rb.AddForce(direction, ForceMode2D.Impulse);
    }
}
