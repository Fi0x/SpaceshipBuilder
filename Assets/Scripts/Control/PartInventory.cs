using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartInventory : MonoBehaviour
{
    public static PartInventory Instance { get; private set; }
    public Vector3Int BodyParts { get; private set; }
    public Vector2Int Guns { get; private set; }
    public Vector2Int Thrusters { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void AddBodyPart(Vector3Int part) {
        BodyParts += part;
    }
    public void AddGun(Vector2Int part)
    {
        Guns += part;
    }
    public void AddThruster(Vector2Int part)
    {
        Thrusters += part;
    }
}
