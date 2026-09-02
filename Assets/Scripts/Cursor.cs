using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
public class Cursor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UnityEngine.Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue())+new Vector3(0,0,10);
    }
}
