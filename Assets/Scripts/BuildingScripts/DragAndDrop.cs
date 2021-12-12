using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Control;
using Parts;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BuildingScripts
{
    public class DragAndDrop : MonoBehaviour
    {
        [HideInInspector] public GameObject spaceship;
        
        private bool _inInventory;  
        private GameObject _snapShadow;
        private SpaceshipPart _partType;
        private List<GameObject> _possibleDocks;
        private Vector3 _pickupPosition;
        public static event EventHandler ShipPartAddedEvent;
        public static event EventHandler ShipPartRemovedEvent;
        
        private void FixedUpdate()
        {
            //Debug.Log(this.GetComponent<SpriteRenderer>().color)
            
         
                if (this.CompareTag("Part") && this.GetComponentInChildren<Docking>() != null)
                {
                    if (spaceship.GetComponent<AntiRace>()._red)
                        this.GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0.5f, 1);
                }
                else
                {
                    this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(this._partType == null)
                return;
            if (other.gameObject.tag.Equals("Inventory")) 
                this._inInventory = true;
        }
   
        private void OnTriggerExit2D(Collider2D other)
        {
            if(this._partType == null)
                return;
            if (!other.gameObject.tag.Equals("Inventory"))
                return;
            this._inInventory = false;
        }

        private void Start()
        {
            this.spaceship = GameObject.Find("Spaceship(Clone)");
            this._partType = this.gameObject.GetComponent<SpaceshipPart>();
        }

        public void OnMouseDown()
        {
            spaceship.GetComponent<AntiRace>()._red = false;
            wait();
            _pickupPosition = this.transform.position;
            foreach (var a in GetComponentsInChildren<Docking>())
            {
                Destroy(a);
            }

            {
                this.transform.parent = null;
                this._snapShadow = Preview.InitShadow(this.gameObject, this._partType.OriginalInventory.transform);
                this.tag = "Part";
                ConnectionCheck.ClearShip();
                // TODO: Remove part from ship, iterate through all parts and "disable" non connected
            }
        }

        public void OnMouseDrag()
            {
                var mouseReturn = GetMousePos();
                if (!mouseReturn.HasValue)
                    return;
            
                this._possibleDocks = SnapHelper.GetPossibleDockingPoints();
                this.transform.position = mouseReturn.Value;
                if (this.gameObject.name != "Body(Clone)")
                {
                    Transform transform1;
                    if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                    {
                        var newRotation = new Vector3(0, 0, 90);
                        transform1 = this.transform;
                        transform1.eulerAngles = transform1.eulerAngles + newRotation;
                    }
                    else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                    {
                        var newRotation = new Vector3(0, 0, -90);
                        transform1 = this.transform;
                        transform1.eulerAngles = transform1.eulerAngles + newRotation;
                    }
                }
                if(this._snapShadow!=null && this._possibleDocks!=null )
                     Preview.RenderShadow(this._snapShadow, this.gameObject.transform.rotation, this.transform, this._possibleDocks);
            }

            public void OnMouseUp()
            {
                Destroy(this._snapShadow);
            
            
                for (var i = 0; i < this.transform.childCount; i++)
                {
                    if(!this.transform.GetChild(i).gameObject.GetComponent<Docking>())
                        this.transform.GetChild(i).gameObject.AddComponent<Docking>();
                }
            
                if (this._inInventory)
                {
                    this._partType.SpawnInInventory();
                    DestroyPart();
                    ShipPartRemovedEvent?.Invoke(null, null);
                }
                else if (SnapHelper.Snap(this.transform, this.spaceship, this._partType, this._possibleDocks))
                {
                    ShipPartAddedEvent?.Invoke(null, null);
                }
            
                if(this.transform.parent == null)
                {
                    this._partType.SpawnInInventory();
                    DestroyPart();
                    ShipPartRemovedEvent?.Invoke(null, null);
                }
                
              
                SnapHelper.MakeDockingPointsInvisible();
                
               if(this.transform.position!=_pickupPosition)
                    ConnectionCheck.DestroynotShip();
                
                   //TODO: Go through all parts and "enable" all connected ones
           
            }

            private static Vector3? GetMousePos()
            {
                if (Camera.main == null)
                    return null;
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                return mousePos;
            }
            
            private void wait()
            {
                new WaitForSeconds(1);
                spaceship.GetComponent<AntiRace>()._red = true;
            }
            
            private void DestroyPart()
            {
                GameObject gm =  GameObject.Find("GameManager(Clone)");
                Debug.Log(Regex.Replace( Regex.Replace(this.name, @"\s+", ""), @"\(Clone\)", ""));
                gm.GetComponentInChildren<InventoryTracker>()._inventory[Regex.Replace( Regex.Replace(this.name, @"\s+", ""), @"\(Clone\)", "")]++;
                Destroy(this.gameObject);
                Destroy(this);
            }
    }
}