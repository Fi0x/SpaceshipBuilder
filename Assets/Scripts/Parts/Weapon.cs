using System;
using UnityEngine;

namespace Parts
{
    public class Weapon : SpaceshipPart
    {
        [SerializeField] private GameObject prefabProjectile;

        public static event EventHandler ShotFiredEvent;

        private int _weaponDelay;
        
        public bool Working { get; set; }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            
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
            var tf = this.transform;
            Instantiate(this.prefabProjectile, tf.position, tf.rotation);
            this._weaponDelay = 15;
            
            ShotFiredEvent?.Invoke(null, null);
        }
    }
}