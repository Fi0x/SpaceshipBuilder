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
        if(!this.InInventory)
        {
            var otherbody = this.GetClosestEnemy();
            var temp = (this.transform.position - otherbody.transform.position).normalized;
            if (Math.Abs(temp.x) <= Math.Abs(temp.y))
            {
                if (temp.y > 0)
                {
                    this.transform.position = new Vector3(otherbody.transform.position.x,
                        otherbody.transform.position.y + transform.localScale.y);
                }
                else
                {
                    this.transform.position = new Vector3(otherbody.transform.position.x,
                        otherbody.transform.position.y - transform.localScale.y);
                }
            }
            else
            {
                if (temp.x > 0) 
                {
                    this.transform.position = new Vector3(otherbody.transform.position.x + this.transform.localScale.x,
                        otherbody.transform.position.y);
                }
                else
                {
                    this.transform.position = new Vector3(otherbody.transform.position.x - this.transform.localScale.x,
                        otherbody.transform.position.y);
                }
            }

            this.tag = "Ship";
            this.transform.SetParent(this.spaceship.transform);
        }
        else
            this.transform.position = this.Inpos;
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
 