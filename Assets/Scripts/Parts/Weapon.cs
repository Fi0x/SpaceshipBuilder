using System;
using Control;
using FlightScripts;
using UnityEngine;

namespace Parts
{
    public class Weapon : SpaceshipPart
    {
        [SerializeField] private GameObject prefabProjectile;
        [SerializeField] private float weaponDelay;

        private bool _ready = true;
        
        public static event EventHandler ShotFiredEvent;

        public bool Working { get; set; }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if (!this.Working)
                return;
            
            if ((Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space)) && _ready)
                this.Shoot();
        }

        private void Shoot()
        {
            _ready = false;
            Invoke(nameof(Reload), weaponDelay);

            var tf = this.transform;
            var projectile = Instantiate(this.prefabProjectile, tf.position, tf.rotation);
            projectile.GetComponent<Projectile>().dir = this.GetDirection();
            
            ShotFiredEvent?.Invoke(null, null);
        }

        private Vector3 GetDirection()
        {
            var angle = this.transform.localRotation.z * 2 + (GameManager.Instance.ShipScript.zAngle - 90) * Mathf.Deg2Rad;
            var toReturn = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
            return toReturn;
        }

        private void Reload()
        {
            this._ready = true;
        }
    }
}