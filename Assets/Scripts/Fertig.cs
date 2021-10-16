using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fertig : MonoBehaviour
{
    private bool toggle = true;

    private GameObject Inventory;
    // Start is called before the first frame update
    private void OnMouseDown()
    {
        Debug.Log(Inventory);
        if (toggle)
        {
            Inventory = GameObject.Find("Inventory");
            Inventory.SetActive(false);
        }
        else
        {
            Inventory = GameObject.Find("Inventory");
            Inventory.SetActive(true);
        }
    }
}

