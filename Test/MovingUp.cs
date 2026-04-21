using System;
using UnityEngine;

public class MovingUp : MonoBehaviour
{
    private void OnEnable()
    {
        transform.position = new Vector3(0, 200f, 0);
    }
}
