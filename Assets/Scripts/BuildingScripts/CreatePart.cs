using System;
using Parts;
using UnityEngine;

namespace BuildingScripts
{
    public class CreatePart : MonoBehaviour
    {
        public GameObject partPrefab;
        private GameObject _inventory;
        private GameObject _currentChild;
        
        private void Start()
        {
            this._inventory = this.gameObject;
            this.SpawnPart();
        }
    
        public void SpawnPart()
        {
            var part = Instantiate(this.partPrefab, this._inventory.transform, true);
            this._currentChild = part;
            part.transform.position = this.transform.position;
            part.GetComponent<SpaceshipPart>().OriginalInventory = this.gameObject;
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