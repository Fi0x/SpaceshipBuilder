using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private Spaceship ship;
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("No Game Manager");
            }
            return _instance;
        }
    }
    public GameObject PrefabProjectile; 
    //[HideInInspector]
    public bool alive = false;


    private void Awake()
    {
        _instance = this;
        ship = GameObject.Find("Spaceship").GetComponent<Spaceship>();
    }

    public void GameOver()
    {
        if (alive)
        {
            Debug.Log("Game Over!");
            alive = false;
        }
    }

    public void startGame()
    {
        alive = true;
        Debug.Log("Game Started");
    }
    public void initShip()
    {
        ship = GameObject.Find("Spaceship").GetComponent<Spaceship>();
    }

    public float getShipSpeed()
    {
        return ship.getSpeed();
    }

    public Vector3 getShipDirection()
    {
        return ship.getDirection();
    }

    public Vector3 getBackgroundMovement()
    {
        return ship.getSpeed() * ship.getDirection();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && !alive){
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
