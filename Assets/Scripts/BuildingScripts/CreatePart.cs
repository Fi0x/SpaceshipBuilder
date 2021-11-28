using UnityEngine;

namespace BuildingScripts
{
    public class CreatePart : MonoBehaviour
    {
        public GameObject partPrefab;

        private GameObject _inventory;
        private void Start()
        {
            this._inventory = this.gameObject;
            this.SpawnPart();
        }
    
        public void SpawnPart()
        {
            var part = Instantiate(this.partPrefab, this._inventory.transform, true);
            part.transform.position = this.transform.position;
        }
    }
}