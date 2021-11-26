using System;
using Control;
using FlightScripts;
using UnityEngine;

namespace Parts
{
    public class SpaceshipPart : MonoBehaviour
    {
        [HideInInspector] public bool drift;
        private GameManager _gameManager;

        public static event EventHandler ShipPartLostEvent;
        
        public void Start()
        {
            this._gameManager = GameManager.Instance;
            GameManager.GameManagerInstantiatedEvent += (sender, args) => { this._gameManager = args.NewInstance; };
        }

        public void FixedUpdate()
        {
            if (this.drift)
                this.transform.position += this._gameManager.GetBackgroundMovement() * Time.deltaTime;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(!this._gameManager.Running)
                return;
            
            switch (collision.gameObject.tag)
            {
                case "Ship":
                case "Projectile":
                    return;
            }
        
            this.drift = true;
            foreach (var script in this.gameObject.GetComponentsInChildren<Weapon>())
                script.Working = false;
            
            ShipPartLostEvent?.Invoke(null, null);
        
            this.transform.parent = null;
            var asteroid = collision.gameObject.GetComponent<AsteroidBehaviour>();
            if (asteroid != null)
                asteroid.Init();
        }
    }
}