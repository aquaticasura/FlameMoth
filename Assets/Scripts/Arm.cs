using UnityEngine;
using UnityEngine.InputSystem;

public class Arm : MonoBehaviour
{
    private Vector2 mouseScreenPos;
    private Vector2 mouseWorldPos;
    private Vector2 mouseDir;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mouseScreenPos = Mouse.current.position.ReadValue();
        mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseDir = (mouseWorldPos- (Vector2)transform.position).normalized;

    }
    private void FixedUpdate()
    {
        transform.right = mouseDir;
    }
}
