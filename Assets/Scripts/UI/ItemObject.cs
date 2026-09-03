using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Items")]
public class ItemObject : ScriptableObject
{
    public int ID;
    public string Name;
    public int quantity = 1;
    public string itemtype;
    public float SpeedBoost;
    public float JumpBoost;
    public int AirJumps;
    public float RollStrength;
    public Sprite itemsprite;
}
