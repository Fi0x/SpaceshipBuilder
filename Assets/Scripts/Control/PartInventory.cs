using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartInventory : MonoBehaviour
{
    public static PartInventory Instance { get; private set; }
    public int[] BodyParts { get; private set; }
    public int[] Guns { get; private set; }
    public int[] Thrusters { get; private set; }

    private void Awake()
    {
        Instance = this;
        BodyParts = new int[6];
        Guns = new int[3];
        Thrusters = new int[3];
    }

    public void AddBodyPart(int part) {
        BodyParts[part] ++;
        Debug.Log("Added Bodypart: " + part);
    }
    public void AddGun(int part)
    {
        Guns[part]++;
        Debug.Log("Added Gun: " + part);
    }
    public void AddThruster(int part)
    {
        Thrusters[part]++;
        Debug.Log("Added Thruster: " + part);
    }
}
