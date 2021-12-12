using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace BuildingScripts
{
    public static class ConnectionCheck
    {
        public static void ClearShip()
        {
            GameObject.Find("Spaceship(Clone)").GetComponent<AntiRace>()._building = true;
            foreach (var a in GameObject.FindGameObjectsWithTag("Ship"))
            {
                if(a.name==("Spaceship(Clone)"))
                    continue;
                a.tag = "Part";
            }
            GameObject.Find("Spaceship(Clone)").GetComponent<AntiRace>()._building = false;
        }


        public static void DestroynotShip()
        {
            GameObject.Find("Spaceship(Clone)").GetComponent<AntiRace>()._building = true;
            foreach (var a in GameObject.FindGameObjectsWithTag("Part"))
            {
                if(a.name=="Spaceship(Clone)")
                    continue;
                if (a.GetComponentInChildren<Docking>())
                {
                    GameObject.Find("GameManager(Clone)").GetComponentInChildren<InventoryTracker>()
                        ._inventory[Regex.Replace(Regex.Replace(a.name, @"\s+", ""), @"\(Clone\)", "")]++;
                    Object.Destroy(a.gameObject);
                    Object.Destroy(a);
                }
                //TODO:Count Parts and Add Them To the Inventory
                //TODO: Change for FlyingScene 
            }
            GameObject.Find("Spaceship(Clone)").GetComponent<AntiRace>()._building = false;
           
        }
    }
}