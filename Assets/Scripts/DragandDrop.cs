using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class DragandDrop : MonoBehaviour
{
    
    public GameObject[] part;
    public GameObject closestPart;
    public GameObject spaceship;
    public GameObject inventory;
    private bool _inInventory;
    private GameObject _snapShadow;
    
   private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == inventory)
        {
            for (int i=0; i<transform.childCount; i++) {
                Destroy(transform.GetChild(i).gameObject.GetComponent<Docking>());
            }
            _inInventory = true;
        }
         
    }
   
   private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == inventory)
        {
            _inInventory = false;

            for (int i=0; i<transform.childCount; i++) {
                if(!transform.GetChild(i).gameObject.GetComponent<Docking>())
                    transform.GetChild(i).gameObject.AddComponent<Docking>();
            }
        }
           
    }

    private void Start()
    {
        spaceship = GameObject.Find("Spaceship(Clone)");
        inventory = GameObject.Find("Building Inventory");
    }

    private void OnMouseDown()
    {
        Debug.Log(gameObject.name);
        InitShadow();
        part = GameObject.FindGameObjectsWithTag("DockEmpty");
        this.tag = "Part";
            //this.transform.SetParent(inventory.transform);
        }
    

    void OnMouseDrag()
    {
        if (true)
        {
            transform.position = GetMousePos();
            Transform transform1;
            if (this.gameObject.name != "Body(Clone)")
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
                {
                    Debug.Log("90°");
                    Vector3 newRotation = new Vector3(0, 0, 90);
                    transform1 = this.transform;
                    transform1.eulerAngles = transform1.eulerAngles + newRotation;
                }
                else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
                {
                    Vector3 newRotation = new Vector3(0, 0, -90);
                    transform1 = this.transform;
                    transform1.eulerAngles = transform1.eulerAngles + newRotation;
                }
            }

            RenderShadow();

        }
    }

    private void OnMouseUp()
    {
        DestroyShadow();
        if(!_inInventory){ 
            Snap();
        }
        else
        {
            transform.position = inventory.transform.position;
        }
    }
    
    Vector3 GetMousePos()
    {
        var mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousPos.z = 0;
            return mousPos;
    }
    

    private Tuple<GameObject,Transform> GetClosestDockingPoint()
    {
        Transform dockingpoint=null;
        GameObject[] children = new GameObject[transform.childCount];
        for (int i=0; i<transform.childCount; i++) {
            children[i]=transform.GetChild(i).gameObject;
        }
        part = GameObject.FindGameObjectsWithTag("DockEmpty");
        IEnumerable<GameObject>possibledocks = part.Except(children);
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform child in transform)
        {

            foreach (GameObject temp in possibledocks )
            {
                Vector3 localrotavec = -(this.transform.rotation * child.localPosition);
                if((temp.transform.parent.rotation*temp.transform.localPosition)!=localrotavec)
                   continue;
                if(!temp.transform.parent.CompareTag("Ship"))
                    continue;
                // Debug.DrawLine(temp.transform.position,child.position,Color.red,2.5f);
                Vector3 directionToTarget = temp.transform.position - child.position;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    closestPart = temp;
                    dockingpoint = child;
                }
            }
            
        }
        return new Tuple<GameObject, Transform>(closestPart,dockingpoint);
    }
    
    
    private void Snap()
    {
        Tuple<GameObject,Transform> docking =  GetClosestDockingPoint();
        var transform1 = this.transform;
        var position = docking.Item1.transform.position;
        Vector3 temp = (transform1.position - position).normalized;
        var localPosition = docking.Item2.localPosition;
        Vector3 locpos = localPosition;
        Vector3 localposrotat= transform1.rotation * localPosition;
        transform1.position = position-localposrotat;
        //this.tag = "Ship"; 
        this.transform.SetParent(spaceship.transform);
    }
    private void InitShadow()
    {
       
        _snapShadow = Instantiate(this.gameObject, inventory.transform, true) as GameObject;
        foreach (var comp in _snapShadow.GetComponents<Component>())
        {
            if (!(comp is Transform || comp is SpriteRenderer))
            {
                Destroy(comp);
            }
        }
        for (int i=0; i<_snapShadow.transform.childCount; i++) {
            Destroy(_snapShadow.transform.GetChild(i).gameObject);
        }
        _snapShadow.tag = "Untagged";
        Color tmp = _snapShadow.GetComponent<SpriteRenderer>().color;
        tmp.a = 0.4f;
        _snapShadow.GetComponent<SpriteRenderer>().color = tmp;
    }
    private void RenderShadow()
    {
        _snapShadow.transform.rotation = this.gameObject.transform.rotation;
        SpriteRenderer _renderer = _snapShadow.gameObject.GetComponent<SpriteRenderer>();
        Tuple<GameObject,Transform> docking =  GetClosestDockingPoint(); 
        var transform1 = this.transform;
        var position = docking.Item1.transform.position;
        Vector3 temp = (transform1.position - position).normalized;
        var localPosition = docking.Item2.localPosition;
        Vector3 locpos = localPosition;
        Vector3 localposrotat= transform1.rotation * localPosition;
        _renderer.transform.position = position-localposrotat;
    }

    private void DestroyShadow()
    {
        Destroy(_snapShadow);
    }
}
 