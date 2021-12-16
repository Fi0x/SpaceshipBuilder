using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTracker : MonoBehaviour
{
    public readonly IDictionary<string, int> Inventory = new Dictionary<string, int>();
    
    public static InventoryTracker Instance { get; private set; }

    private void Start()
    {
        Instance = this;
    }

    public  void Init()
    {
        this.Inventory["Body0"] = 6;
        this.Inventory["Body1"] = 4;
        this.Inventory["Body2"] = 2;
        this.Inventory["Body3"] = 0;
        this.Inventory["Body4"] = 0;
        this.Inventory["Body5"] = 0;
        
        this.Inventory["Weapon0"] = 2;
        this.Inventory["Weapon1"] = 0;
        this.Inventory["Weapon2"] = 0;
        
        this.Inventory["Thruster0"] = 1;
        this.Inventory["Thruster1"] = 0;
        this.Inventory["Thruster2"] = 0;
    }

    public void AddBodyPart(int part)
    {
        this.Inventory["Body"+part]++;
        Debug.Log("Added Bodypart: " + part);
        Debug.Log("Now At" + this.Inventory["Body" + part]);
    }
    public void AddGun(int part)
    {
        this.Inventory["Weapon" + part]++;
        Debug.Log("Added Gun: " + part);
        Debug.Log("Now At" + this.Inventory["Weapon" + part]);
    }
    public void AddThruster(int part)
    {
        this.Inventory["Thruster" + part]++;
        Debug.Log("Added Thruster: " + part);
        Debug.Log("Now At" + this.Inventory["Thruster" + part]);
    }

}

