using System.Collections.Generic;
using FlightScripts.Enemies;
using UnityEngine;

namespace Control
{
    public class EnemyManager : MonoBehaviour
    {
        private int numberOfEnemies = 3;
        private float spawnDelay = 2;
        [SerializeField] private GameObject[] presets;

        private void Awake()
        {
            Vector2 managerParams = GameManager.Instance.GetEnemyManagerParams();
            numberOfEnemies =(int) managerParams.x;
            spawnDelay = managerParams.y;
            Debug.Log("Enemy Manager: " + numberOfEnemies + " with Delay:" + spawnDelay);
        }
        private void Start()
        {
            for(var i = 0; i<this.numberOfEnemies; i++)
                this.Invoke(nameof(this.SpawnEnemy), (i + 1) * this.spawnDelay);
        }

        private void SpawnEnemy()
        {
            var type = Random.Range(0, this.presets.Length);
            var enemy = Instantiate(this.presets[type]);
        }
    }
}