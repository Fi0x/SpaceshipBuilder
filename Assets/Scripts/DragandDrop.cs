using System;
using UnityEngine;

public class DragandDrop : MonoBehaviour
{
    
    public GameObject[] part;
    public GameObject closestPart;
    public GameObject spaceship;
    public GameObject inventory;

    private bool InInventory;
    private Vector3 Inpos;

   //bad practice need change on full inventory implementation
   private void OnTriggerStay2D(Collider2D other)
    {
        InInventory = true;
        Inpos = transform.position;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        InInventory = false;
    }

    private void Start()
    {
        spaceship = GameObject.Find("MainBody");
        inventory = GameObject.Find("Inventory");
    }
    private void OnMouseDown()
    {
        this.tag = "Part";
        //this.transform.SetParent(inventory.transform);
    }

    void OnMouseDrag()
    {
        transform.position = GetMousePos();
    }
    private void OnMouseUp()
    {
        if(!InInventory){
        GameObject otherbody = GetClosestEnemy();
        Vector3 temp = (transform.position - otherbody.transform.position).normalized;
        if (Math.Abs(temp.x) <= Math.Abs(temp.y))
        {
            Debug.Log(("Y"));
            if (temp.y > 0)
            {
                Debug.Log(("Y-"));
                this.transform.position = new Vector3(otherbody.transform.position.x,
                    otherbody.transform.position.y + transform.localScale.y);
            }
            else
            {
                Debug.Log(("Y+"));
                this.transform.position = new Vector3(otherbody.transform.position.x,
                    otherbody.transform.position.y - transform.localScale.y);
            }
        }
        else
        {
            Debug.Log("X");
            if (temp.x > 0) 
            {
                Debug.Log(("X+"));
                this.transform.position = new Vector3(otherbody.transform.position.x+ transform.localScale.x,
                    otherbody.transform.position.y);
            }
            else
            {
                Debug.Log(("X-"));
                this.transform.position = new Vector3(otherbody.transform.position.x- transform.localScale.x,
                    otherbody.transform.position.y);
            }
        }

        this.tag = "Ship";
        this.transform.SetParent(spaceship.transform);
        }
        else
        {
            transform.position = Inpos;
        }
    }
    
    Vector3 GetMousePos()
    {
        var mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousPos.z = 0;
        return mousPos;
    }

    GameObject GetClosestEnemy()
    {
        part = GameObject.FindGameObjectsWithTag("Ship");
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject temp in part)
        {
            Vector3 directionToTarget = temp.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestPart = temp;
            }
        }

        return closestPart;
    }
}
 