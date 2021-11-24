using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    public int asteroidCount = 15;
    [SerializeField] private GameObject[] presets;
    [SerializeField, ReadOnly]private List<GameObject> asteroids;

    private void Start()
    {
        this.asteroids = new List<GameObject>();
        for (var i = 0; i < this.asteroidCount; i++)
        {
            var type = Random.Range(0, this.presets.Length);
            this.AddAsteroid(type);
        }
    }

    private void FixedUpdate()
    {
        var asteroidsToDestroy = new List<GameObject>();
        foreach (var asteroid in this.asteroids)
        {
            if (asteroid == null)
                continue;
            if(!asteroid.GetComponent<AsteroidBehaviour>().Move(Time.deltaTime))
                continue;
            
            asteroidsToDestroy.Add(asteroid);
        }

        while (asteroidsToDestroy.Count > 0)
        {
            this.asteroids.Remove(asteroidsToDestroy[0]);
            Destroy(asteroidsToDestroy[0]);
            asteroidsToDestroy.RemoveAt(0);
            
            var type = Random.Range(0, this.presets.Length);
            this.AddAsteroid(type);
        }
    }

    private void AddAsteroid(int type)
    {
        var asteroid = Instantiate(this.presets[type]);
        this.asteroids.Add(asteroid);
        asteroid.GetComponent<AsteroidBehaviour>().Init();
    }
}