using System;
using UnityEngine;


public class ChildNameChanger : MonoBehaviour
{
#if UNITY_EDITOR
    private void OnValidate()
    {
        int cnt = 1;
        foreach (Transform child in transform)
        {
            child.name = cnt++.ToString();
        }
    }
#endif
}
