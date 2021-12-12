using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTracker : MonoBehaviour
{
    public IDictionary<String, int> _inventory = new Dictionary<string, int>();
    public  void Init()
    {
        Debug.Log("VAR");
        _inventory.Add("Body0", 4);
        _inventory.Add("Body1", 0);
        _inventory.Add("Body2", 0);
        _inventory.Add("Body3", 0);
        _inventory.Add("Body4", 0);
        _inventory.Add("Body5", 0);
        
        _inventory.Add("Weapon0", 1);
        _inventory.Add("Weapon1", 0);
        _inventory.Add("Weapon2", 0);
        
        _inventory.Add("Thruster0", 0);
        _inventory.Add("Thruster1", 0);
        _inventory.Add("Thruster2", 0);
    }
    
}

