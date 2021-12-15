using System.Collections.Generic;
using System.Text.RegularExpressions;
using Parts;
using UnityEngine;

namespace BuildingScripts
{
    public static class ConnectionCheck
    {
        public static void ClearShip()
        {
            if(GameObject.Find("Spaceship(Clone)"))
                GameObject.Find("Spaceship(Clone)").GetComponent<AntiRace>()._building = true;
            foreach (var a in GameObject.FindGameObjectsWithTag("Ship"))
            {
                if(a.name==("Spaceship(Clone)"))
                    continue;
                a.tag = "Part";
            }
            if(GameObject.Find("Spaceship(Clone)"))
                GameObject.Find("Spaceship(Clone)").GetComponent<AntiRace>()._building = false;
        }
        
        

        public static void DestroynotShip(GameObject go)
        {
            GameObject gm =  GameObject.Find("GameManager(Clone)");
            GameObject.Find("Spaceship(Clone)").GetComponent<AntiRace>()._building = true;
            foreach (var a in GameObject.FindGameObjectsWithTag("Part"))
            {
                if(a.name=="Spaceship(Clone)")
                    continue;
                if (a.GetComponentInChildren<Docking>())
                {
                    //var temp = Regex.Replace(Regex.Replace(a.name, @"\s+", ""), @"\(Clone\)", "");
                    //gm.GetComponentInChildren<InventoryTracker>()._inventory[temp]=gm.GetComponentInChildren<InventoryTracker>()._inventory[temp]+=1;
                    a.GetComponent<DragAndDrop>().DestroyPart(go);
                }
                //TODO:Count Parts and Add Them To the Inventory
                //TODO: Change for FlyingScene 
            }
            GameObject.Find("Spaceship(Clone)").GetComponent<AntiRace>()._building = false;
           
        }
    }
}
