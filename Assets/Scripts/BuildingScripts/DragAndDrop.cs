using System;
using System.Collections.Generic;
using Parts;
using UnityEngine;

namespace BuildingScripts
{
    public class DragAndDrop : MonoBehaviour
    {
        [HideInInspector] public GameObject spaceship;

        private bool _inInventory;
        private GameObject _snapShadow;
        private SpaceshipPart _partType;
        private List<GameObject> _possibleDocks;

        public static event EventHandler ShipPartAddedEvent;
        public static event EventHandler ShipPartRemovedEvent;
    
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
            for (var i = 0; i < this.transform.childCount; i++)
            {
                if(!this.transform.GetChild(i).gameObject.GetComponent<Docking>())
                    this.transform.GetChild(i).gameObject.AddComponent<Docking>();
            }
        }

        private void Start()
        {
            this.spaceship = GameObject.Find("Spaceship(Clone)");
            this._partType = this.gameObject.GetComponent<SpaceshipPart>();
        }

        public void OnMouseDown()
        {
            this.transform.parent = null;
            this._possibleDocks = SnapHelper.GetPossibleDockingPoints();
            this._snapShadow = Preview.InitShadow(this.gameObject, this._partType.OriginalInventory.transform);
            this.tag = "Part";

            // TODO: Remove part from ship, iterate through all parts and "disable" non connected
        }

        public void OnMouseDrag()
        {
            var mouseReturn = GetMousePos();
            if (!mouseReturn.HasValue)
                return;
            
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
            Preview.RenderShadow(this._snapShadow, this.gameObject.transform.rotation, this.transform, this._possibleDocks);
        }

        public void OnMouseUp()
        {
            Destroy(this._snapShadow);
            
            if (this._inInventory)
            {
                this._partType.SpawnInInventory();
                Destroy(this.gameObject);
                Destroy(this);
                ShipPartRemovedEvent?.Invoke(null, null);
            }
            else if (SnapHelper.Snap(this.transform, this.spaceship, this._partType, this._possibleDocks))
            {
                ShipPartAddedEvent?.Invoke(null, null);
            }
            
            if(this.transform.parent == null)
            {
                this._partType.SpawnInInventory();
                Destroy(this.gameObject);
                Destroy(this);
                ShipPartRemovedEvent?.Invoke(null, null);
            }
            
            SnapHelper.MakeDockingPointsInvisible();
            
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
    }
}