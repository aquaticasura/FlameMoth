using TMPro;
using UnityEngine;

public class ItemName : MonoBehaviour
{
    private TMP_Text itemname;
    public Item item;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        itemname = GetComponentInChildren<TMP_Text>();
        itemname.text = item.itemobject.Name;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
