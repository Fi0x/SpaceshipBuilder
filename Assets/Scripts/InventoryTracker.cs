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
        _inventory.Add("Body1", 3);
        _inventory.Add("Body2", 3);
        _inventory.Add("Body3", 2);
        _inventory.Add("Body4", 2);
        _inventory.Add("Body5", 2);
        
        _inventory.Add("Weapon0", 1);
        _inventory.Add("Weapon1", 0);
        _inventory.Add("Weapon2", 0);
        
        _inventory.Add("Thruster0", 1);
        _inventory.Add("Thruster1", 0);
        _inventory.Add("Thruster2", 0);
    }

    public void AddBodyPart(int part)
    {
        _inventory["Body"+part]++;
        Debug.Log("Added Bodypart: " + part);
        Debug.Log("Now At" + _inventory["Body" + part]);
    }
    public void AddGun(int part)
    {
        _inventory["Weapon" + part]++;
        Debug.Log("Added Gun: " + part);
        Debug.Log("Now At" + _inventory["Weapon" + part]);
    }
    public void AddThruster(int part)
    {
        _inventory["Thruster" + part]++;
        Debug.Log("Added Thruster: " + part);
        Debug.Log("Now At" + _inventory["Thruster" + part]);
    }

}

