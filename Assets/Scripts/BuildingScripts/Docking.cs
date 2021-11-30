using UnityEngine;

namespace BuildingScripts
{
    public class Docking : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("DockEmpty"))
                return;
            
            this.tag = "DockFull";
            other.tag = "DockFull";
            this.transform.parent.tag = "Ship";
            other.transform.parent.tag = "Ship";
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