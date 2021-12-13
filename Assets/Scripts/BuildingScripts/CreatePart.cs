using System.Text.RegularExpressions;
using Control;
using Parts;
using UnityEngine;
using UnityEngine.UI;

namespace BuildingScripts
{
    public class CreatePart : MonoBehaviour
    {
        public GameObject partPrefab;
        private GameObject _gmInventory;
        private GameObject _currentChild;
        
        
        private void Start()
        {
            this._gmInventory = this.gameObject;
            this.SpawnPart();
        }

        private void Update()
        {
            this.UpdateCount();
        }

        private void UpdateCount()
        {
            if (this.transform.childCount==0 && GameManager.Instance.GetComponentInChildren<InventoryTracker>()._inventory[this.transform.name] >= 1)
                this.SpawnPart();
            if (!GameObject.Find(this.name + "(Value)"))
                return;
            
            if (!(this.transform.childCount == 0 || GameManager.Instance.GetComponentInChildren<InventoryTracker>()._inventory[this.transform.name] == 0))
            {
                var temp = GameManager.Instance.GetComponentInChildren<InventoryTracker>()._inventory[this.transform.name];
                temp += 1;
                GameObject.Find(this.name + "(Value)").GetComponent<Text>().text = temp.ToString();
            }
            else
                GameObject.Find(this.name + "(Value)").GetComponent<Text>().text = "";
        }
        
        public void SpawnPart()
        {

            for (var i = 0; i < this.transform.childCount; i++)
            {
                if(Regex.Replace(this.transform.GetChild(i).gameObject.name, @"\s+", "")!=this.name+"(Clone)")
                    Destroy(this.transform.GetChild(i).gameObject);
            }
            if (this.transform.childCount != 0 || GameManager.Instance.GetComponentInChildren<InventoryTracker>()._inventory[this.transform.name] < 1)
                return;
            
            GameManager.Instance.GetComponentInChildren<InventoryTracker>()._inventory[this.transform.name]--;
            var part = Instantiate(this.partPrefab, this._gmInventory.transform, true);
            this._currentChild = part;
            part.transform.position = this.transform.position;
            part.GetComponent<SpaceshipPart>().OriginalInventory = this.gameObject;
            var currentScale = part.transform.localScale;
            part.GetComponent<DragAndDrop>().OriginalScale = currentScale;
            part.transform.localScale = currentScale * 0.5f;
        }

        private void OnMouseDown()
        {
            if(this._currentChild == null)
                return;
            
            this._currentChild.GetComponent<DragAndDrop>()?.OnMouseDown();
        }

        private void OnMouseDrag()
        {
            if(this._currentChild == null)
                return;
            
            this._currentChild.GetComponent<DragAndDrop>()?.OnMouseDrag();
        }

        private void OnMouseUp()
        {
            if(this._currentChild == null)
                return;
            
            this._currentChild.GetComponent<DragAndDrop>()?.OnMouseUp();
        }
    }
}