using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory")]
public class Item : ScriptableObject
{
    public new string name = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;
}
