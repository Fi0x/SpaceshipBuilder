using System;
using BuildingScripts;
using Control;
using UnityEngine;

namespace Parts
{
    public class SpaceshipPart : MonoBehaviour
    {
        [HideInInspector] public bool drift;

        public static event EventHandler ShipPartLostEvent;
        public static event EventHandler ResourceCollectedEvent;
        
        public GameObject OriginalInventory { get; set; }

        protected virtual void FixedUpdate()
        {
            if (this.drift)
                this.transform.position += GameManager.Instance.GetBackgroundMovement() * Time.deltaTime;
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("Collision detected");
            if(!GameManager.Running)
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

        public void SpawnInInventory()
        {
            var partInventory = this.OriginalInventory.GetComponent<CreatePart>();
            partInventory.SpawnPart();
        }

        private void CollideWithAsteroid(GameObject asteroid)
        {
            this.drift = true;
            foreach (var script in this.gameObject.GetComponentsInChildren<Weapon>())
                script.Working = false;
            
            ShipPartLostEvent?.Invoke(null, null);
        
            this.transform.parent = null;
            Destroy(asteroid);
            Destroy(this.gameObject, 5);
        }

        private static void CollectResource(GameObject resource)
        {
            ResourceCollectedEvent?.Invoke(null, null);
            Destroy(resource);
        }
    }
}