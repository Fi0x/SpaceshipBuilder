using System;
using Control;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FlightScripts
{
    public class AsteroidBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject resourcePrefab;
        [SerializeField] private float asteroidMaxSpeed = 10;
        [SerializeField, Range(0, 1)] private float resourceChance = 0.5f;
        
        private Vector3 _vel;

        public static event EventHandler AsteroidDestroyedEvent;

        private void Start()
        {
            this._vel = new Vector3(
                Random.Range(-this.asteroidMaxSpeed, this.asteroidMaxSpeed),
                Random.Range(-this.asteroidMaxSpeed, 0),
                0);
        }

        public void FixedUpdate()
        {
            if (!GameManager.Running)
                return;

            this.transform.position += this._vel / 60;
            this.transform.position += GameManager.Instance.GetBackgroundMovement() / 60;
        }

        public void DestroyByShot()
        {
            AsteroidDestroyedEvent?.Invoke(null, null);

            if (Random.value < this.resourceChance)
            {
                Instantiate(resourcePrefab, this.transform.position, new Quaternion());
            }
            
            Destroy(this.gameObject);
        }
    }
}