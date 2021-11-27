using FlightScripts;
using UnityEngine;

namespace Parts
{
    public class Weapon : SpaceshipPart
    {
        [SerializeField] private GameObject prefabProjectile;

        private int _weaponDelay;
        
        public bool Working { get; set; }

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
            
            if (!Input.GetKey(KeyCode.Space))
                return;

            var tf = this.transform;
            var projectile = Instantiate(this.prefabProjectile, tf.position, tf.rotation);
            projectile.AddComponent(typeof(Projectile));
            this._weaponDelay = 15;
        }
    }
}