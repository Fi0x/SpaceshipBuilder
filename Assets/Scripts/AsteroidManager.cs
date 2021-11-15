using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    public int Asteroids = 10;
    public int NumberOfPresets = 12;

    private GameObject[] presets;
    private GameObject[] asteroids;

    private void Awake()
    {
        presets = new GameObject[NumberOfPresets];
        for(int i = 0; i<presets.Length; i++)
        {
            presets[i] = GameObject.Find("Asteroid_" + (i+1));
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        asteroids = new GameObject[Asteroids];
        for (int i = 0;i < Asteroids ;i++ )
        {
            int type = (int)Random.Range(0, NumberOfPresets);
            asteroids[i] = Instantiate(presets[type]);
            asteroids[i].GetComponent<AsteroidBehaviour>().Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Asteroids; i++)
        {
            if (asteroids[i].GetComponent<AsteroidBehaviour>().Move(Time.deltaTime))
            {
                Destroy(asteroids[i]);
                int type = (int)Random.Range(0, NumberOfPresets);
                asteroids[i] = Instantiate(presets[type]);
                asteroids[i].GetComponent<AsteroidBehaviour>().Init();
            }
        }
    }
}
