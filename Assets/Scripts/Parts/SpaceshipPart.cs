using System;
using BuildingScripts;
using Control;
using UnityEngine;

namespace Parts
{
    public class SpaceshipPart : MonoBehaviour
    {
        [HideInInspector] public bool drift;
        private GameManager _gameManager;

        public static event EventHandler ShipPartLostEvent;
        public static event EventHandler ResourceCollectedEvent;
        
        public GameObject OriginalInventory { get; protected set; }
        
        private void Start()
        {
            this._gameManager = GameManager.Instance;
            GameManager.GameManagerInstantiatedEvent += (sender, args) => { this._gameManager = args.NewInstance; };
        }

        private void FixedUpdate()
        {
            if (this.drift)
                this.transform.position += this._gameManager.GetBackgroundMovement() * Time.deltaTime;
        }

        public void SpawnInInventory()
        {
            var partInventory = this.OriginalInventory.GetComponent<CreatePart>();
            partInventory.SpawnPart();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(this._gameManager == null)
                return;
            if(!this._gameManager.Running)
                return;
            
            switch (collision.gameObject.tag)
            {
                case "Ship":
                case "Projectile":
                case "Station":
                    break;
                case "Asteroid":
                    this.CollideWithAsteroid(collision.gameObject);
                    break;
                case "Resource":
                    CollectResource(collision.gameObject);
                    break;
            }
        }

        private void CollideWithAsteroid(GameObject asteroid)
        {
            this.drift = true;
            foreach (var script in this.gameObject.GetComponentsInChildren<Weapon>())
                script.Working = false;
            
            ShipPartLostEvent?.Invoke(null, null);
        
            this.transform.parent = null;
            Destroy(asteroid);
        }

        private static void CollectResource(GameObject resource)
        {
            ResourceCollectedEvent?.Invoke(null, null);
            Destroy(resource);
        }
    }
}