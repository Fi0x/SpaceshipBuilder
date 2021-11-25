using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Control
{
    public class GameManager : MonoBehaviour
    {
        public float asteroidMaxSpeed = 10;
        public GameObject prefabProjectile;
    
        public static event EventHandler<NewGameManagerEventArgs> GameManagerInstantiatedEvent;
        
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
        public GameObject Menu { get; set; }
        public GameObject InGameButtons { get; set; }
        public GameObject Ship { get; set; }
        public Spaceship ShipScript { get; set; }
        public bool Running { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void GameOver(bool won)
        {
            if (!this.Running)
                return;
        
            this.Running = false;
            StatTracker.Instance.PlayerWon = won;
            SceneChanger.LoadStatScreen();
        }

        public void StartGame()
        {
            this.Running = true;
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
            if(this.Running)
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
}