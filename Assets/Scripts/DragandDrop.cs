using System;
using UnityEngine;

public class DragandDrop : MonoBehaviour
{
    
    private GameObject[] _part;
    private GameObject _closestPart;
    private GameObject _spaceship;
    private GameObject _inventory;

    private bool _inInventory;
    private Vector3 _inPos;

   //bad practice need change on full inventory implementation
   private void OnTriggerStay2D(Collider2D other)
    {
        _inInventory = true;
        _inPos = transform.position;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        _inInventory = false;
    }
    private void OnMouseDown()
    {
        this.tag = "Part";
        _inventory = GameObject.Find("Building");
        this.transform.SetParent(_inventory.transform);
        _spaceship = GameObject.Find("Spaceship");
    }

    void OnMouseDrag()
    {
        transform.position = GetMousePos();
    }
    private void OnMouseUp()
    {
        if(!_inInventory){
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
        this.transform.SetParent(_spaceship.transform);
        }
        else
        {
            transform.position = _inPos;
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
        _part = GameObject.FindGameObjectsWithTag("Ship");
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject temp in _part)
        {
            Vector3 directionToTarget = temp.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                _closestPart = temp;
            }
        }

        return _closestPart;
    }
}
 