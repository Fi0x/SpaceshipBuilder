using System;
using System.Collections.Generic;
using Parts;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace BuildingScripts
{
    public class DragAndDrop : MonoBehaviour
    {
        [HideInInspector] public GameObject spaceship;

        private bool _inInventory;
        private GameObject _snapShadow;
        private SpaceshipPart _partType;
        private IEnumerable<GameObject> _possibleDocks;

        public static event EventHandler ShipPartAddedEvent;
        public static event EventHandler ShipPartRemovedEvent;
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(this._partType == null)
                return;
            if (other.gameObject != this._partType.OriginalInventory)
                return;

            this._inInventory = true;
        }
   
        private void OnTriggerExit2D(Collider2D other)
        {
            if(this._partType == null)
                return;
            if (other.gameObject != this._partType.OriginalInventory)
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
            this._possibleDocks = SnapHelper.GetPossibleDockingPoints(this.gameObject);
            this._snapShadow = Preview.InitShadow(this.gameObject, this._partType.OriginalInventory.transform);
            this.tag = "Part";
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
                    Debug.Log("90°");
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
            SnapHelper.MakeDockingPointsInvisible();
            
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
            else
            {
                this._partType.SpawnInInventory();
                Destroy(this.gameObject);
                Destroy(this);
                ShipPartRemovedEvent?.Invoke(null, null);
            }
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