using System;
using Control;
using Parts;
using UnityEngine;

namespace FlightScripts
{
    public class Spaceship : MonoBehaviour
    {
        public float accelerationPerSecond = 100;
        public const float MaxSpeed = 20;
        public float currentMaxSpeed = 20;
        public int maxAngle = 45;
        public int turnSpeed = 100;

        [HideInInspector] public int zAngle;
        private float _horizontalOffset;

        public float Speed { get; private set; }
        public float HorizontalOffset { get; set; }

        private void FixedUpdate()
        {
            if (!GameManager.Running)
                return;
        
            this.UpdateRotation();
            this.UpdateSpeed();
        }

        private void UpdateRotation()
        {
            //Rotate
            if (this.zAngle == 360 && this.zAngle == -360)
                this.zAngle = 0;

            if (Input.GetKey(KeyCode.A))
                this.zAngle += this.turnSpeed / 60;
        
            if (Input.GetKey(KeyCode.D))
                this.zAngle -= this.turnSpeed / 60;

            var threshold = this.CalcAngleThreshold();
            this.zAngle = Mathf.Clamp(this.zAngle, threshold.x, threshold.y);
            this.transform.rotation = Quaternion.AngleAxis(this.zAngle, Vector3.forward);
        }

        private void UpdateSpeed()
        {
            //Adjust speed
            if (Input.GetKey(KeyCode.W))
                this.Speed += this.accelerationPerSecond / 60;
            else if (Input.GetKey(KeyCode.S))
                this.Speed -= this.accelerationPerSecond / 60;
        
            if (this.Speed > this.currentMaxSpeed)
                this.Speed = this.currentMaxSpeed;
            if (this.Speed < 10)
                this.Speed = 10;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "Station":
                    GameManager.Instance.GameOver(true);
                    break;
                case "Asteroid":
                    GameManager.Instance.GameOver(false);
                    break;
            }
        }

        public void ResetShip()
        {
            var tf = this.transform;
            tf.position = Vector3.zero;
            tf.localScale = new Vector3(1, 1, 1);
            tf.rotation = new Quaternion();
            this.zAngle = 0;
            this.Speed = 0;
        }

        private Vector2Int CalcAngleThreshold()
        {
            var offset = Mathf.Abs(this._horizontalOffset);
            if (!(offset > 120 - this.maxAngle / 2)) 
                return new Vector2Int(-this.maxAngle, this.maxAngle);
        
            Debug.Log("DangerZone!" + this._horizontalOffset);
            return this._horizontalOffset < 0
                ? new Vector2Int(-(int) (120 - offset) * 2, this.maxAngle)
                : new Vector2Int(-this.maxAngle, (int) (120 - offset) * 2);
        }

        public Vector3 GetDirection()
        {
            var angle = (this.zAngle - 90) * Mathf.Deg2Rad;
            var toReturn = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
            return toReturn;
        }

        public void ThrusterDestroyedEventHandler(object sender, EventArgs args)
        {
            if (sender is Thruster thruster)
            {
                thruster.ThrusterDestroyedEvent -= this.ThrusterDestroyedEventHandler; 
                this.currentMaxSpeed -= thruster.SpeedIncrease;
            }
        }
    }
}