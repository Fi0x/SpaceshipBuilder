using System;
using System.Collections.Generic;
using BuildingScripts;
using Control;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Parts
{
    public class SpaceshipPart : MonoBehaviour
    {
        private bool _isDrifting;

        public static event EventHandler ShipPartLostEvent;
        
        public GameObject OriginalInventory { get; set; }

        protected virtual void FixedUpdate()
        {
            if (this._isDrifting)
                this.transform.position += GameManager.Instance.GetBackgroundMovement() * Time.deltaTime;
            if ( SceneManager.GetActiveScene().name == "FlyingScene"&&this.CompareTag("Part"))//this.GetComponent<SpriteRenderer>().color == new Color(1, 0.5f, 0.5f, 1))
                Invoke(nameof(DestroyByConnection), 0.3f);
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

        private void DestroyByConnection()
        {
            {
                if (!this.CompareTag("Part")) return;
                this._isDrifting = true;
                foreach (var script in this.gameObject.GetComponentsInChildren<Weapon>())
                    script.Working = false;
                this.transform.parent = null;
                Destroy(this.gameObject, 5);
            }
        }
        
        private void CollideWithAsteroid(GameObject asteroid)
        {   
            ConnectionCheck.ClearShip();
            this._isDrifting = true;
            foreach (var script in this.gameObject.GetComponentsInChildren<Weapon>())
                script.Working = false;
            if(asteroid!=null)
                ShipPartLostEvent?.Invoke(null, null);
        
            this.transform.parent = null;
            if(asteroid!=null)
                Destroy(asteroid);
            Destroy(this.gameObject, 5);
        }

    }
}