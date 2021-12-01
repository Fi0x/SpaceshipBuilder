using System.Collections.Generic;
using FlightScripts.Enemies;
using UnityEngine;

namespace Control
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private int numberOfEnemies = 3;
        [SerializeField] private float spawnDelay = 2;
        [SerializeField] private GameObject[] presets;
        private List<GameObject> enemies;
        
        private void Start()
        {
            this.enemies = new List<GameObject>();
            for(var i = 0; i<this.numberOfEnemies; i++)
                this.Invoke(nameof(this.SpawnEnemy), (i + 1) * this.spawnDelay);
        }

        private void SpawnEnemy()
        {
            var type = Random.Range(0, this.presets.Length);
            var enemy = Instantiate(this.presets[type]);
            enemy.GetComponent<Enemy>().Init();
            this.enemies.Add(enemy);
        }
    }
}