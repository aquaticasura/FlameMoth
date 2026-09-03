using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, Interactable
{


    public ItemObject itemobject;

    private TMP_Text quantityText;
    private GameObject NameImage;
    private InventoryController inventoryController;
    private Image spriteimage;




    private void Awake()
    {
        spriteimage = GetComponent<Image>();
        quantityText = GetComponentInChildren<TMP_Text>();
        inventoryController = Object.FindFirstObjectByType<InventoryController>();
        UpdateQuantityDisplay();
        spriteimage.sprite = itemobject.itemsprite;

    }
    
    public void UpdateQuantityDisplay()
    {
        if (quantityText != null)
        {
            quantityText.text = itemobject.quantity > 1 ? itemobject.quantity.ToString() : "";
        }
    }

    public void AddToStack(int amount = 1)
    {
        itemobject.quantity += amount;
        UpdateQuantityDisplay();
    }

    public int RemoveFromStack(int amount = 1)
    {
        int removed = Mathf.Min(amount, itemobject.quantity);
        itemobject.quantity -= removed;
        UpdateQuantityDisplay();
        return removed;
    }

    public GameObject CloneItem(int newQuantity)
    {
        GameObject clone = Instantiate(gameObject);
        Item cloneItem = clone.GetComponent<Item>();
        cloneItem.itemobject.quantity = newQuantity;
        cloneItem.UpdateQuantityDisplay();
        return clone;
    }





    private void Start()
    {
        
    }
    public virtual void UseItem()
    {
        //effect when item used
    }

    public virtual void PickUp()
    {
        Sprite itemIcon = GetComponent<Image>().sprite;
        if (ItemPickupUIController.Instance != null )
        {
            ItemPickupUIController.Instance.ShowItemPickup(itemobject.Name, itemIcon);
        }
    }

    public void Interact()
    {

        bool itemAdded = inventoryController.AddItem(gameObject);

        if (itemAdded)
        {
            Debug.Log("Interact");
            Destroy(gameObject);
            PickUp();
        }

    }

    public bool CanInteract()
    {
        return true;
    }
}
