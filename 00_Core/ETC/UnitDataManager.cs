using System;
using _01_Script._06_UI.Shop;
using UnityEngine;

namespace _01_Script._00_Core.ETC
{
    public class UnitDataManager : MonoBehaviour
    {
        public static UnitDataManager Instance;
        
        [SerializeField] private InventorySO inventorySO;
        [SerializeField] private EquippedUnit equippedUnit;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
            
            equippedUnit.LoadFromJson();
            if (equippedUnit.inventory.Count < 5)
            {
                equippedUnit.inventory.Clear();
                
                for (int i = 0; i < 5; i++)
                {
                    var unit = equippedUnit.defaultInventory[i];
                    unit.isOwned = true;
                    unit.isEquipped = false;
                    equippedUnit.inventory.Add(unit);
                }
                
                UnityEditor.EditorUtility.SetDirty(equippedUnit);
                UnityEditor.AssetDatabase.SaveAssets();
                equippedUnit.SaveToJson();
            }
            
            inventorySO.LoadFromJson();
            
            if (inventorySO.inventory.Count < 5)
            {
                inventorySO.inventory.Clear();
                
                for (int i = 0; i < 5; i++)
                {
                    inventorySO.inventory.Add(inventorySO.defaultInventory[i]);
                }
                
                UnityEditor.EditorUtility.SetDirty(inventorySO);
                UnityEditor.AssetDatabase.SaveAssets();
                inventorySO.SaveToJson();
            }
            
            inventorySO.inventory.ForEach(unit => unit.isOwned = true);
        }
    }
}