using UnityEngine;

public class EquipmentSlot : MonoBehaviour
{

    public Slot slot;
    public string EquipmentName;
    public int EquipmentID;
    public int Amount;
    public string EquipmentType;
    public float SpeedBoost;
    public float JumpBoost;
    public int AirJumps;
    public float RollStrength;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        if (slot.currentItem != null)
        {
            Item slotItem = slot.currentItem.GetComponent<Item>();
            if (slotItem.itemobject.ID != EquipmentID)
            {
                EquipmentID = slotItem.itemobject.ID;
                Amount = slotItem.itemobject.quantity;
                EquipmentType = slotItem.itemobject.itemtype;

                SpeedBoost = slotItem.itemobject.SpeedBoost;
                JumpBoost = slotItem.itemobject.JumpBoost;
                AirJumps = slotItem.itemobject.AirJumps;
                RollStrength = slotItem.itemobject.RollStrength;
                Debug.Log(EquipmentID);


            }
        }
        else
        {
            EquipmentID = 0;
            return;
        }
    }
}
