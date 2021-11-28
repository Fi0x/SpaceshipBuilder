using UnityEngine;

namespace BuildingScripts
{
    public class Docking : MonoBehaviour
    {
        public GameObject otherBody;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("DockEmpty"))
                return;
            
            this.tag = "DockFull";
            other.tag = "DockFull";
            this.otherBody = other.gameObject;
            other.GetComponent<Docking>().SetOtherBody(this.gameObject);
            this.transform.parent.tag = "Ship";
            other.transform.parent.tag = "Ship";
        }  
    
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("DockFull"))
                return;
            
            this.tag = "DockEmpty";
            other.tag = "DockEmpty";
            other.GetComponent<Docking>().SetOtherBody(null);
            this.transform.parent.tag = "Part";
            other.transform.parent.tag = "Part";
            this.otherBody = null;
        }

        private void SetOtherBody(GameObject other)
        {
            this.otherBody = other;
        }
    }
}