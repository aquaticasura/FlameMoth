using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public static Movement Instance;
    public Rigidbody2D rb;
    public float playerInput;
    public float move_Speed;
    
    public Vector2 faux_Velocity;

    
    void Start()
    {
        StartCoroutine(TestKnockback());
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

    }
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2((playerInput*move_Speed)+faux_Velocity.x, rb.linearVelocity.y);
        faux_Velocity = Vector2.Lerp(faux_Velocity, Vector2.zero, 2f * Time.fixedDeltaTime);

    }
    public void GetMovement(InputAction.CallbackContext context)
    {
        playerInput = context.ReadValue<float>();
    }
    public void ApplyVelocity(Vector2 applied_Vel)
    {
        faux_Velocity += applied_Vel;
        rb.linearVelocity += new Vector2(0, faux_Velocity.y);
    }
    private IEnumerator TestKnockback()
    {
        yield return new WaitForSeconds(5);
        ApplyVelocity(new Vector2(5, 5));
    }
}
