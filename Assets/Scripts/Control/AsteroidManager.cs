using System.Collections.Generic;
using FlightScripts;
using Unity.Collections;
using UnityEngine;

namespace Control
{
    public class AsteroidManager : MonoBehaviour
    {
        private int asteroidCount = 15;
        [SerializeField] private GameObject[] presets;
        [SerializeField, ReadOnly]private List<GameObject> asteroids;

        private void Awake()
        {
            asteroidCount = GameManager.Instance.GetAsteroidManagerParams();
            Debug.Log("Asteroid Manager: " + asteroidCount);
        }
        private void Start()
        {
            this.asteroids = new List<GameObject>();
            for (var i = 0; i < this.asteroidCount; i++)
                this.AddRandomAsteroid();
        }

        private void FixedUpdate()
        {
            this.RespawnAsteroidsBelowCamera();

            var missingAsteroids = this.asteroidCount - this.asteroids.Count;
            for (var i = 0; i < missingAsteroids; i++)
                this.AddRandomAsteroid();
        }

        private void RespawnAsteroidsBelowCamera()
        {
            var asteroidsToDestroy = new List<GameObject>();
            foreach (var asteroid in this.asteroids)
            {
                if (asteroid == null)
                    continue;
                if(asteroid.transform.position.y < -30)
                    asteroidsToDestroy.Add(asteroid);
            }

            while (asteroidsToDestroy.Count > 0)
            {
                this.asteroids.Remove(asteroidsToDestroy[0]);
                Destroy(asteroidsToDestroy[0]);
                asteroidsToDestroy.RemoveAt(0);
            
                this.AddRandomAsteroid();
            }
        }

        private void AddRandomAsteroid()
        {
            var type = Random.Range(0, this.presets.Length);
            var newPos = new Vector3(Random.Range(-120, 120), 50, 0);
            var asteroid = Instantiate(this.presets[type], newPos, new Quaternion());
            this.asteroids.Add(asteroid);
        }
    }
}