using System;
using System.Linq;
using Parts;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace BuildingScripts
{
    public class DragAndDrop : MonoBehaviour
    {
        [HideInInspector] public GameObject[] part;
        [HideInInspector] public GameObject closestPart;
        [HideInInspector] public GameObject spaceship;

        private bool _inInventory;
        private GameObject _snapShadow;
        private SpaceshipPart _partType;

        public static event EventHandler ShipPartAddedEvent;
    
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

        private void OnMouseDown()
        {
            this.InitShadow();
            this.part = GameObject.FindGameObjectsWithTag("DockEmpty");
            this.tag = "Part";
        }

        private void OnMouseDrag()
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
            this.RenderShadow();
        }

        private void OnMouseUp()
        {
            this.DestroyShadow();
            if (this._inInventory)
            {
                this._partType.SpawnInInventory();
                Destroy(this.gameObject);
                Destroy(this);
            }
            else if(!this.Snap())
            {
                this._partType.SpawnInInventory();
                Destroy(this.gameObject);
                Destroy(this);
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

        private (GameObject, Transform) GetClosestDockingPoint()
        {
            Transform dockingPoint = null;
            var children = new GameObject[this.transform.childCount];
            for (var i = 0; i < this.transform.childCount; i++)
                children[i] = this.transform.GetChild(i).gameObject;
            
            this.part = GameObject.FindGameObjectsWithTag("DockEmpty");
            var possibleDocks = this.part.Except(children);
            var closestDistanceSqr = Mathf.Infinity;
            foreach (Transform child in this.transform)
            {
                foreach (var temp in possibleDocks)
                {
                    var localRotVec = -(this.transform.rotation * child.localPosition);
                    if((temp.transform.parent.rotation*temp.transform.localPosition)!=localRotVec)
                        continue;
                    if(!temp.transform.parent.CompareTag("Ship"))
                        continue;
                    
                    var directionToTarget = temp.transform.position - child.position;
                    var dSqrToTarget = directionToTarget.sqrMagnitude;
                    if (dSqrToTarget >= closestDistanceSqr)
                        continue;
                    
                    closestDistanceSqr = dSqrToTarget;
                    this.closestPart = temp;
                    dockingPoint = child;
                }
            }
            return (this.closestPart,dockingPoint);
        }
        
        private bool Snap()
        {
            (var dockingObject, var dockingTransform) = this.GetClosestDockingPoint();
            if (dockingObject == null || dockingTransform == null)
                return false;
            
            var transform1 = this.transform;
            var position = dockingObject.transform.position;
            var localPosition = dockingTransform.localPosition;
            var localPosRot= transform1.rotation * localPosition;
            transform1.position = position-localPosRot;
            this.transform.SetParent(this.spaceship.transform);

            ShipPartAddedEvent?.Invoke(null, null);
            this._partType.SpawnInInventory();
            return true;
        }
        
        private void InitShadow()
        {
            this._snapShadow = Instantiate(this.gameObject, this._partType.OriginalInventory.transform, true);
            foreach (var comp in this._snapShadow.GetComponents<Component>())
            {
                if (!(comp is Transform || comp is SpriteRenderer))
                    Destroy(comp);
            }
            for (var i = 0; i < this._snapShadow.transform.childCount; i++)
                Destroy(this._snapShadow.transform.GetChild(i).gameObject);
            
            this._snapShadow.tag = "Untagged";
            var tmp = this._snapShadow.GetComponent<SpriteRenderer>().color;
            tmp.a = 0.4f;
            this._snapShadow.GetComponent<SpriteRenderer>().color = tmp;
        }
        
        private void RenderShadow()
        {
            this._snapShadow.transform.rotation = this.gameObject.transform.rotation;
            var spriteRenderer = this._snapShadow.gameObject.GetComponent<SpriteRenderer>();
            
            (var dockingObject, var dockingTransform) =  this.GetClosestDockingPoint();
            if(dockingObject == null || dockingTransform == null)
                return;
            
            var transform1 = this.transform;
            var position = dockingObject.transform.position;
            var localPosition = dockingTransform.localPosition;
            var localPosRot= transform1.rotation * localPosition;
            spriteRenderer.transform.position = position-localPosRot;
        }

        private void DestroyShadow()
        {
            Destroy(this._snapShadow);
        }
    }
}