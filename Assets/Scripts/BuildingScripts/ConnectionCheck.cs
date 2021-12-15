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
            GameObject.Find("Spaceship(Clone)").GetComponent<AntiRace>()._building = true;
            foreach (var a in GameObject.FindGameObjectsWithTag("Ship"))
            {
                if(a.name==("Spaceship(Clone)"))
                    continue;
                a.tag = "Part";
            }
            GameObject.Find("Spaceship(Clone)").GetComponent<AntiRace>()._building = false;
        }

        public static IEnumerable<WaitForSeconds> DropShip()
        {
            ClearShip();
            yield return new WaitForSeconds(1);
            foreach (var a in GameObject.FindGameObjectsWithTag("Part"))
            {
                //a.GetComponent<SpaceshipPart>().CollideWithAsteroid(null);
            }
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
