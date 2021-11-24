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
        public GameObject Ship { get; set; }
        public Spaceship ShipScript { get; set; }
        public bool Alive { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void GameOver()
        {
            if (!this.Alive)
                return;
        
            this.Alive = false;
            SceneChanger.LoadBuildingScene();
        }

        public void StartGame()
        {
            this.Alive = true;
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
}