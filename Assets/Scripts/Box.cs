using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Box : MonoBehaviour
{
    public bool IsTaken { get; set; }

    public event Action Taked;

    public void Init(Vector3 position)
    {
        transform.SetPositionAndRotation(position, Quaternion.identity);
        IsTaken = false;
    }
}