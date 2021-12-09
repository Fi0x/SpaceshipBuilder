using System;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingScripts
{
    public class Docking : MonoBehaviour
    {
        public List<GameObject> connectionToMain;

        public String GetParentTag()
        {
            return this.transform.parent.tag;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if ((other.GetComponent<Docking>() != null))
            {
                if (GameObject.Find("Spaceship(Clone)").GetComponent<AntiRace>()._building == false)
                {
                    if (other.gameObject.GetComponent<Docking>().GetParentTag() == "Ship")
                    {
                        this.gameObject.transform.parent.tag = "Ship";
                    }
                }

                if (!other.CompareTag("DockEmpty"))
                    return;
                this.tag = "DockFull";
                other.tag = "DockFull";
                this.transform.parent.tag = "Ship";
            }
        }


        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("DockFull"))
                return;
            
            this.tag = "DockEmpty";
            other.tag = "DockEmpty";
            this.transform.parent.tag = "Part";
            other.transform.parent.tag = "Part";
        }
    }
}