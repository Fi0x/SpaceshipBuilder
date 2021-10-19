using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Go : MonoBehaviour
{
    private GameObject Inventory;
    private void OnMouseDown()
    {
    
            Inventory = GameObject.Find("Inventory");
            Inventory.SetActive(false);

    }
}

