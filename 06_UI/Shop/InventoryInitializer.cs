using System;
using _01_Script._06_UI.Inventory;
using UnityEditor;
using UnityEngine;

namespace _01_Script._06_UI.Shop
{
    public class InventoryInitializer : MonoBehaviour
    {
        [SerializeField] private InventorySO inventorySO;
        [SerializeField] private GameObject item;
        [SerializeField] private Transform root;
        [SerializeField] private InventoryInfoUI inventoryInfo;

        private void Awake()
        {
            SetUpUI();
        }

        private void SetUpUI()
        {
            foreach (Transform child in root)
            { 
                Destroy(child.gameObject);
            }
            
            foreach (var unit in inventorySO.inventory)
            {
                var newItem = Instantiate(this.item, root);
                newItem.GetComponent<InventoryItem>().InitUI(unit);
            }

            inventoryInfo.InitializeInfoUI();
        }
    }
}