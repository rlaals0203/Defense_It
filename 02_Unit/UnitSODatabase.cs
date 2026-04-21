using System.Collections.Generic;
using UnityEngine;

namespace _01_Script._02_Unit
{
    [CreateAssetMenu(fileName = "UnitSODatabase", menuName = "SO/UnitSODatabase")]
    public class UnitSODatabase : ScriptableObject
    {
        public List<UnitSO> units;

        public UnitSO GetUnitByName(string name)
        {
            return units.Find(unit => unit != null && unit.name == name);
        }
    }
}