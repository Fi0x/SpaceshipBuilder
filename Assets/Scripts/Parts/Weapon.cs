using FlightScripts;
using UnityEngine;

namespace Parts
{
    public class Weapon : SpaceshipPart
    {
        [SerializeField] private GameObject prefabProjectile;
        [SerializeField] private int weaponDelay;

        private int _timeToNextShot;
        
        public bool Working { get; set; }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if (!this.Working)
                return;
            if (this._timeToNextShot > 0)
            {
                this._timeToNextShot--;
                return;
            }
            
            if (!Input.GetKey(KeyCode.Space))
                return;

            var tf = this.transform;
            Instantiate(this.prefabProjectile, tf.position, tf.rotation);
            this._timeToNextShot = this.weaponDelay;
        }
    }
}