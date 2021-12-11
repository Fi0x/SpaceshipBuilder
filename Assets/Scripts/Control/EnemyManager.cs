using System.Collections.Generic;
using FlightScripts.Enemies;
using UnityEngine;

namespace Control
{
    public class EnemyManager : MonoBehaviour
    {
        public int numberOfEnemies = 3;
        public float spawnDelay = 2;
        [SerializeField] private GameObject[] presets;
        
        private void Start()
        {
            for(var i = 0; i<this.numberOfEnemies; i++)
                this.Invoke(nameof(this.SpawnEnemy), (i + 1) * this.spawnDelay);
        }

        private void SpawnEnemy()
        {
            var type = Random.Range(0, this.presets.Length);
            var enemy = Instantiate(this.presets[type]);
            enemy.GetComponent<Enemy>().Init();
        }
    }
}