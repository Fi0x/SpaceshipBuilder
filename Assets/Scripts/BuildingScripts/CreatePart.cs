using System;
using Parts;
using UnityEngine;
using UnityEngine.UI;

namespace BuildingScripts
{
    public class CreatePart : MonoBehaviour
    {
        public GameObject partPrefab;
        private GameObject _gminventory;
        private GameObject _currentChild;
        private GameObject _gameManager;
        
        
        private void Start()
        {
            this._gminventory = this.gameObject;
            this._gameManager=GameObject.Find("GameManager(Clone)");
            this.SpawnPart();
        }

        private void Update()
        {
            UpdateCount();
        }

        public void UpdateCount()
        {
            
            if (this.transform.childCount==0 && _gameManager.GetComponentInChildren<InventoryTracker>()._inventory[this.transform.name] >= 1)
                SpawnPart();
            if (GameObject.Find(this.name + "(Value)"))
            {
                if (!(this.transform.childCount == 0 || _gameManager.GetComponentInChildren<InventoryTracker>()._inventory[this.transform.name] == 0))
                {
                    int temp;
                    temp = _gameManager.GetComponentInChildren<InventoryTracker>()._inventory[this.transform.name];
                    temp += 1;
                    GameObject.Find(this.name + "(Value)").GetComponent<Text>().text = temp.ToString();
                }
                else
                {
                    GameObject.Find(this.name + "(Value)").GetComponent<Text>().text = "";
                }
            }
        }
        
        public void SpawnPart()
        {
            for (var i = 0; i < this.transform.childCount; i++)
            {
                Destroy(this.transform.GetChild(i).gameObject);
            }

            if (this.transform.childCount==0 && _gameManager.GetComponentInChildren<InventoryTracker>()._inventory[this.transform.name] >= 1)
            {
                _gameManager.GetComponentInChildren<InventoryTracker>()._inventory[this.transform.name]--;
                var part = Instantiate(this.partPrefab, this._gminventory.transform, true);
                this._currentChild = part;
                part.transform.position = this.transform.position;
                part.GetComponent<SpaceshipPart>().OriginalInventory = this.gameObject;
            }
            
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