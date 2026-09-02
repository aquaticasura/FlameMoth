using UnityEditor.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class HoverItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject NameText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NameText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");
        NameText.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exit");
        NameText.SetActive(false);
    }
}
