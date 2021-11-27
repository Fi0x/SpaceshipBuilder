using System;
using FlightScripts;
using UnityEngine;

namespace Parts
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private GameObject prefabProjectile;

        public static event EventHandler ShotFiredEvent;
        
        public bool Working { get; set; }

        private int _weaponDelay;

        private void FixedUpdate()
        {
            if(this.gameObject.GetComponent<SpaceshipPart>().drift)
                return;
            if (!this.Working)
                return;
            if (this._weaponDelay > 0)
            {
                this._weaponDelay--;
                return;
            }
            
            if (Input.GetKey(KeyCode.Space))
                this.Shoot();
        }

        private void Shoot()
        {
            var projectile = Instantiate(this.prefabProjectile, this.transform.position, this.transform.rotation);
            projectile.AddComponent(typeof(Projectile));
            this._weaponDelay = 15;
            
            ShotFiredEvent?.Invoke(null, null);
        }
    }
}