using Control;
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
            var projectile = Instantiate(this.prefabProjectile, tf.position, tf.rotation);
            projectile.GetComponent<Projectile>().dir = this.GetDirection();
            
            this._timeToNextShot = this.weaponDelay;
        }

        private Vector3 GetDirection()
        {
            var angle = this.transform.localRotation.z * 2 + (GameManager.Instance.ShipScript.zAngle - 90) * Mathf.Deg2Rad;
            var toReturn = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
            return toReturn;
        }
    }
}