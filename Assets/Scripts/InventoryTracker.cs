using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTracker : MonoBehaviour
{
    public IDictionary<String, int> _inventory = new Dictionary<string, int>();
    public  void Init()
    {
        _inventory.Add("Body0", 4);
        _inventory.Add("Body1", 6);
        _inventory.Add("Body2", 6);
        _inventory.Add("Body3", 2);
        _inventory.Add("Body4", 2);
        _inventory.Add("Body5", 2);
        
        _inventory.Add("Weapon0", 3);
        _inventory.Add("Weapon1", 3);
        _inventory.Add("Weapon2", 3);
        
        _inventory.Add("Thruster0", 1);
        _inventory.Add("Thruster1", 2);
        _inventory.Add("Thruster2", 3);
    }
    
}

