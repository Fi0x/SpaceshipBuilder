using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float asteroidMaxSpeed = 10;
    public GameObject prefabProjectile;
    
    public static event EventHandler<NewGameManagerEventArgs> GameManagerInstantiatedEvent;
    
    public GameObject Ship { get; set; }
    public Spaceship ShipScript { get; set; }
    public bool Alive { get; private set; }
    private static GameManager _instance;
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
        private set
        {
            _instance = value;
            var args = new NewGameManagerEventArgs { NewInstance = _instance};
            GameManagerInstantiatedEvent?.Invoke(_instance, args);
        }
    }

    private void Awake()
    {
        Debug.Log("Instance: " + this);
        Instance = this;
    }

    public void GameOver()
    {
        if (!this.Alive)
            return;
        
        Debug.Log("Game Over!");
        this.Alive = false;
        SceneChanger.Instance.LoadBuildingScene();
    }

    public void StartGame()
    {
        this.Alive = true;
        Debug.Log("Game Started");
    }
    public void InitShip(GameObject ship)
    {
        this.Ship = ship;
        this.ShipScript = ship.GetComponent<Spaceship>();
    }

    public Vector3 GetBackgroundMovement()
    {
        return this.ShipScript.Speed * this.ShipScript.GetDirection();
    }

    private void Update()
    {
        if(this.Alive)
            return;
        if (!Input.GetKeyDown(KeyCode.R))
            return;
        
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public class NewGameManagerEventArgs : EventArgs
    {
        public GameManager NewInstance;
    }
}