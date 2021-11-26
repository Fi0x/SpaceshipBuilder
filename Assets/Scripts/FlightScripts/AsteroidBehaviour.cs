using Control;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FlightScripts
{
    public class AsteroidBehaviour : MonoBehaviour
    {
        private GameManager _gameManager;
        [SerializeField] private float asteroidMaxSpeed = 10;
        [SerializeField, ReadOnly] private Vector3 vel;

        private void Start()
        {
            this._gameManager = GameManager.Instance;
            GameManager.GameManagerInstantiatedEvent += (sender, args) => { this._gameManager = args.NewInstance; };
        }

        public void Init()
        {
            this._gameManager = GameManager.Instance;
        
            /* Position
             * Velocity
             * Rotation
             */
            var newPos = new Vector3(Random.Range(-120, 120), 50, 0);
            this.transform.position = newPos;
            this.vel = new Vector3(
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
            this.transform.position += this.vel * time;
            if (!this._gameManager.Running)
                return false;

            this.transform.position += this._gameManager.GetBackgroundMovement() * time;

            return this.transform.position.y < -30;
        }
    }
}