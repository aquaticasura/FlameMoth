using System;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuCanvas.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Menu()
    {
        if (!menuCanvas.activeSelf && PauseController.IsGamePaused)
        {
            return;
        }
        menuCanvas.SetActive(!menuCanvas.activeSelf);
        PauseController.SetPause(menuCanvas.activeSelf);
        Debug.Log("menu()");
    }
    public void Menu(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            Menu();
        }

    }
}
