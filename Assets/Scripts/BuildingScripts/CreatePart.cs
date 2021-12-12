using Parts;
using UnityEngine;

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
    
        public void SpawnPart()
        {
            for (var i = 0; i < this.transform.childCount; i++)
            {
                Destroy(this.transform.GetChild(i).gameObject);
            }

            if (_gameManager.GetComponentInChildren<InventoryTracker>()._inventory[this.transform.name] > 0)
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