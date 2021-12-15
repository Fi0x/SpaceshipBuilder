using System;
using BuildingScripts;
using Control;
using UnityEngine;

namespace Parts
{
    public class SpaceshipPart : MonoBehaviour
    {
        private bool _isDrifting;

        public static event EventHandler ShipPartLostEvent;
        public static event EventHandler ResourceCollectedEvent;
        
        public GameObject OriginalInventory { get; set; }

        protected virtual void FixedUpdate()
        {
            if (this._isDrifting)
                this.transform.position += GameManager.Instance.GetBackgroundMovement() * Time.deltaTime;
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if(!GameManager.Running)
                return;
            switch (collision.gameObject.tag)
            {
                case "Ship":
                case "Projectile":
                    break;
                case "Station":
                    break;
                case "Asteroid":
                    this.CollideWithAsteroid(collision.gameObject);
                    break;
                case "Resource":
                    break;
                case "Enemy":
                    this.CollideWithAsteroid(null);
                    break;
                case "EnemyProjectile":
                    this.CollideWithAsteroid(null);
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
            this._isDrifting = true;
            foreach (var script in this.gameObject.GetComponentsInChildren<Weapon>())
                script.Working = false;
            
            ShipPartLostEvent?.Invoke(null, null);
        
            this.transform.parent = null;
            Destroy(asteroid);
            Destroy(this.gameObject, 5);
        }

    }
}