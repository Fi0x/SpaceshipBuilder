using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace BuildingScripts
{
    public class DragAndDrop : MonoBehaviour
    {
        public GameObject[] part;
        public GameObject closestPart;
        public GameObject spaceship;
        public GameObject inventory;
        private bool _inInventory;
        private GameObject _snapShadow;
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject != this.inventory)
                return;
            
            for (var i = 0; i < this.transform.childCount; i++)
                Destroy(this.transform.GetChild(i).gameObject.GetComponent<Docking>());
                
            this._inInventory = true;
        }
   
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject != inventory)
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
            this.inventory = GameObject.Find("Building Inventory");
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
            if(!this._inInventory)
                this.Snap();
            else
                this.transform.position = this.inventory.transform.position;
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
        
        private void Snap()
        {
            (GameObject gobj, Transform tf) docking = this.GetClosestDockingPoint();
            var transform1 = this.transform;
            var position = docking.gobj.transform.position;
            var localPosition = docking.tf.localPosition;
            var localPosRot= transform1.rotation * localPosition;
            transform1.position = position-localPosRot;
            this.transform.SetParent(this.spaceship.transform);
        }
        
        private void InitShadow()
        {
            this._snapShadow = Instantiate(this.gameObject, this.inventory.transform, true);
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
            (GameObject gobj, Transform tf) docking =  this.GetClosestDockingPoint(); 
            var transform1 = this.transform;
            var position = docking.gobj.transform.position;
            var localPosition = docking.tf.localPosition;
            var localPosRot= transform1.rotation * localPosition;
            spriteRenderer.transform.position = position-localPosRot;
        }

        private void DestroyShadow()
        {
            Destroy(this._snapShadow);
        }
    }
}