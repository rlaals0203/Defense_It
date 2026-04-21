using System;
using UnityEngine;

namespace _01_Script._05_GameSystem.ItemDrop
{
    public interface IDropable
    {
        void DropItem(Vector3 origin, Vector3 destination);
        void PickupItem(Action callback = null);
    }
}