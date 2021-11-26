using Control;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FlightScripts
{
    public class AsteroidBehaviour : MonoBehaviour
    {
        [SerializeField] private float asteroidMaxSpeed = 10;
        private Vector3 _vel;

        public void Init()
        {
            /* Position
             * Velocity
             * Rotation
             */
            var newPos = new Vector3(Random.Range(-120, 120), 50, 0);
            this.transform.position = newPos;
            this._vel = new Vector3(
                Random.Range(-this.asteroidMaxSpeed, this.asteroidMaxSpeed),
                Random.Range(-this.asteroidMaxSpeed, 0),
                0);
        }

        public bool Move(float time)
        {
            /* Position
             * Velocity
             * Rotation
             */
            this.transform.position += this._vel * time;
            if (!GameManager.Instance.Running)
                return false;

            this.transform.position += GameManager.Instance.GetBackgroundMovement() * time;

            return this.transform.position.y < -30;
        }
    }
}