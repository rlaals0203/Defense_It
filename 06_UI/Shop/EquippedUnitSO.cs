using System.Collections.Generic;
using System.IO;
using _01_Script._02_Unit;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace _01_Script._06_UI.Shop
{
    [CreateAssetMenu(fileName = "InventorySO", menuName = "SO/EquippedUnit", order = 0)]
    public class EquippedUnit : ScriptableObject
    {
        public List<UnitSO> inventory = new List<UnitSO>();
        public List<UnitSO> defaultInventory = new List<UnitSO>();

        public UnitSODatabase database;
        
        private const string FileName = "equipped.json";

        [System.Serializable]
        private class EquippedUnitData
        {
            public List<string> unitIDs;

            public EquippedUnitData(List<UnitSO> units)
            {
                unitIDs = new List<string>();
                foreach (var unit in units)
                {
                    if (unit != null)
                        unitIDs.Add(unit.name);
                }
            }
        }

        private string GetSavePath()
        {
            return Path.Combine(Application.persistentDataPath, FileName);
        }

        public void SaveToJson()
        {
            var data = new EquippedUnitData(inventory);
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(GetSavePath(), json);
            
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }

        public void LoadFromJson()
        {
            string path = GetSavePath();
            if (!File.Exists(path))
            {
                Debug.LogWarning("Inventory JSON not found.");
                return;
            }

            string json = File.ReadAllText(path);
            EquippedUnitData data = JsonUtility.FromJson<EquippedUnitData>(json);
            inventory.Clear();

            foreach (var id in data.unitIDs)
            {
                UnitSO unit = database.GetUnitByName(id);
                if (unit != null)
                {
                    inventory.Add(unit);
                }
                else
                {
                    Debug.LogWarning($"UnitSO not found in database: {id}");
                }
            }
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }
        
        [ContextMenu("Reset Inventory Data")]
        public void ResetInventory()
        {
            inventory.Clear();
    
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif

            // 저장도 반영
            SaveToJson();
            Debug.Log("Inventory has been reset.");
        }
    }
}