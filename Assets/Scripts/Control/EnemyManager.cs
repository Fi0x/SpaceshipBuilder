using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int NumberOfEnemies = 3;
    public float SpawnDelay = 2;
    [SerializeField] private GameObject[] presets;
    private List<GameObject> enemies;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i<NumberOfEnemies; i++)
        {
            Invoke("spawnEnemy", (i + 1) * SpawnDelay);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void spawnEnemy()
    {
        var type = Random.Range(0, this.presets.Length);
        GameObject enemy = Instantiate(this.presets[type]);
        enemy.GetComponent<Enemy>().Init();
        enemies.Add(enemy);
    }

    private void spawnEnemy(int type)
    {

    }
}
