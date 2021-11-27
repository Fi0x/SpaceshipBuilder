using System;
using Control;
using UnityEngine;

namespace Parts
{
    public class SpaceshipPart : MonoBehaviour
    {
        [HideInInspector] public bool drift;
        protected GameManager GameManager;

        public static event EventHandler ShipPartLostEvent;
        public static event EventHandler ResourceCollectedEvent;
        
        public void Start()
        {
            this.GameManager = GameManager.Instance;
            GameManager.GameManagerInstantiatedEvent += (sender, args) => { this.GameManager = args.NewInstance; };
        }

        private void FixedUpdate()
        {
            if (this.drift)
                this.transform.position += this.GameManager.GetBackgroundMovement() * Time.deltaTime;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(!this.GameManager.Running)
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