using UnityEngine;

public class Docking : MonoBehaviour
{
    public GameObject otherbody = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DockEmpty"))
        {
            this.tag = "DockFull";
            other.tag = "DockFull";
            otherbody = other.gameObject;
            other.GetComponent<Docking>().SetOtherbody(this.gameObject);
            this.transform.parent.tag = "Ship";
            other.transform.parent.tag = "Ship";
            Debug.Log(otherbody);
        }
    }  
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("DockFull"))
        {
            this.tag = "DockEmpty";
            other.tag = "DockEmpty";
            other.GetComponent<Docking>().SetOtherbody(null);
            this.transform.parent.tag = "Part";
            other.transform.parent.tag = "Part";
            otherbody = null;
        }
    }

    public void SetOtherbody(GameObject other)
    {
        otherbody = other;
    }

}
