using Control;
using FlightScripts;
using UnityEngine;

namespace Parts
{
    public class Weapon : SpaceshipPart
    {
        [SerializeField] private GameObject prefabProjectile;

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
            
            if (!Input.GetKey(KeyCode.Space))
                return;

            var tf = this.transform;
            var projectile = Instantiate(this.prefabProjectile, tf.position, tf.rotation);
            
            projectile.GetComponent<Projectile>().dir = this.GetDirection();

            this._weaponDelay = 15;
        }

        private Vector3 GetDirection()
        {
            var angle = this.transform.localRotation.z * 2 + (GameManager.Instance.ShipScript.zAngle - 90) * Mathf.Deg2Rad;
            var toReturn = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
            return toReturn;
        }
    }
}