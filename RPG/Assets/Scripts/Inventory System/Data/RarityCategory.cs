using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "Item Config Base", menuName = "ScriptableObjects/Inventory System/Rarity Category")]
    public class RarityCategory : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Presentation { get; private set; }


    }
}
