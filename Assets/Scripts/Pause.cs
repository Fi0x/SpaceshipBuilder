using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pause : MonoBehaviour
{
    
    public GameObject Inventory;
    private bool running = false;
  
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (running)
            {
                Inventory.SetActive(true);
                running = false;

            }
            else
            {
                 Inventory.SetActive(false);
                 running = true;
            }
           
        }
        
    }
}
