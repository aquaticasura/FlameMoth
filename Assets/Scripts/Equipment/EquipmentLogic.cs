using UnityEngine;

public class EquipmentLogic : MonoBehaviour
{
    public EquipmentSlot RingSlot, BraceletSlot, AmuletSlot;
    public int RingSlotContainer, BraceletSlotContainer, AmuletSlotContainer;
    private InventoryController inventoryController;
    private PlayerMovement playerMovement;
    private int ringchanged;



    private float defaultspeed;
    private int defaultjumpcount;
    private float defaultjumpforce;
    private float defaultrollforce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        inventoryController = Object.FindFirstObjectByType<InventoryController>();
        playerMovement = Object.FindFirstObjectByType<PlayerMovement>();

        defaultspeed = playerMovement.maxSpeed;
        defaultjumpcount = playerMovement.maxJumpCount;
        defaultrollforce = playerMovement.rollForce;
        defaultjumpforce = playerMovement.jumpForce;

    }

    // Update is called once per frame
    void Update()
    {
        ringchanged = RingSlotContainer;
        RingSlotContainer = RingSlot.EquipmentID;
        ring();



    }
    private void defaultstats()
    {
        playerMovement.maxSpeed = defaultspeed;
        playerMovement.maxJumpCount = defaultjumpcount;
        playerMovement.jumpForce = defaultjumpforce;
        playerMovement.rollForce = defaultrollforce;
    }

    private void ring()
    {
        if (RingSlotContainer != 0)
        {
            if (RingSlot.EquipmentType == "ring")
            {
                if (RingSlotContainer != ringchanged)
                {
                    defaultstats();
                    playerMovement.maxSpeed += RingSlot.SpeedBoost;
                    playerMovement.maxJumpCount += RingSlot.AirJumps;
                    playerMovement.jumpForce += RingSlot.JumpBoost;
                    playerMovement.rollForce += RingSlot.RollStrength;

                }
            }
        }
        else
        {
            defaultstats();
        }
    }

  


}
