using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Parts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BuildingScripts
{
    public class DragAndDrop : MonoBehaviour
    {
        [HideInInspector] public GameObject spaceship;
        
        private bool _inInventory;  
        private bool _newOne = false;
        private GameObject _snapShadow;
        private SpaceshipPart _partType;
        private List<GameObject> _possibleDocks;
        private Vector3 _pickupPosition;
        private bool _movedBefore = false;
        
        public static event EventHandler ShipPartAddedEvent;
        public static event EventHandler ShipPartRemovedEvent;
        
        
        private void FixedUpdate()
        {
            if (this.CompareTag("Part") && this.GetComponentInChildren<Docking>() != null)
            {
                    if (this.spaceship.GetComponent<AntiRace>()._red)
                    {
                        //if (SceneManager.GetActiveScene().name == "FlyingScene") return;
                        this.GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0.5f, 1);
                    }
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
            {
                this._inInventory = true;
            }
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
            if (SceneManager.GetActiveScene().name == "FlyingScene") return;
            if (!this._movedBefore)
            {
                this.transform.localScale *= 1.37f;
                this._movedBefore = true;
            }

            this.spaceship.GetComponent<AntiRace>()._red = false;
            this.Wait();
            this._pickupPosition = this.transform.position;
            foreach (var a in this.GetComponentsInChildren<Docking>())
                Destroy(a);
            
            this._snapShadow = Preview.InitShadow(this.gameObject, this._partType.OriginalInventory.transform);
            this.tag = "Part";
            ConnectionCheck.ClearShip();
        }

        public void OnMouseDrag()
            {
                if (SceneManager.GetActiveScene().name == "FlyingScene") return;
                var mouseReturn = GetMousePos();
                if (!mouseReturn.HasValue)
                    return;
                this.transform.parent = null;
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
            if (SceneManager.GetActiveScene().name == "FlyingScene") return;
            var gm = GameObject.Find("GameManager(Clone)");
            if (this._snapShadow == null)
            {
                SnapHelper.MakeDockingPointsInvisible();
                Destroy(this._snapShadow);
                return;
            }
            if (this._snapShadow.GetComponent<SpriteRenderer>().color == new Color(1, 0.5f, 0.5f, 0.4f))
            {
                SnapHelper.MakeDockingPointsInvisible();
                this.DestroyPart(null);
                Destroy(this._snapShadow);
                ShipPartRemovedEvent?.Invoke(null, null);
                return;
            }

            if (this._inInventory)
            {
                SnapHelper.MakeDockingPointsInvisible();
                this.DestroyPart(null);
                Destroy(this._snapShadow);
                ConnectionCheck.DestroynotShip(this.gameObject);
                ShipPartRemovedEvent?.Invoke(null, null);
                return;
            }

            Destroy(this._snapShadow);


            if (SnapHelper.Snap(this.transform, this.spaceship, this._partType, this._possibleDocks))
            {
                for (var i = 0; i < this.transform.childCount; i++)
                {
                    if (!this.transform.GetChild(i).gameObject.GetComponent<Docking>())
                        this.transform.GetChild(i).gameObject.AddComponent<Docking>();
                }
                gm.GetComponentInChildren<InventoryTracker>();
                this._partType.SpawnInInventory();
                ShipPartAddedEvent?.Invoke(null, null);
            }

            if (this.transform.parent == null)
            {
                this.DestroyPart(null);
                ShipPartRemovedEvent?.Invoke(null, null);
            }

            SnapHelper.MakeDockingPointsInvisible();

            if (this.transform.position != this._pickupPosition)
                ConnectionCheck.DestroynotShip(this.gameObject);
            
            this._partType.SpawnInInventory();
            if (!this.transform.parent)
            {
                Destroy(this.gameObject);
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
            
            private void Wait()
            {
                new WaitForSeconds(1);
                this.spaceship.GetComponent<AntiRace>()._red = true;
            }
            
            public void DestroyPart(GameObject go)
            {
                this._newOne = true;
                Destroy(this.gameObject);
                Destroy(this);
                this._partType.SpawnInInventory();
            }

            private void OnDestroy()
            {
                if (SceneManager.GetActiveScene().name == "FlyingScene")
                {
                    ConnectionCheck.ClearShip();
                    return;
                }

                var gm =  GameObject.Find("GameManager(Clone)");
                if (!this._newOne) return;
                gm.GetComponentInChildren<InventoryTracker>()
                    ._inventory[Regex.Replace(Regex.Replace(this.name, @"\s+", ""), @"\(Clone\)", "")]++;
            }
    }
}