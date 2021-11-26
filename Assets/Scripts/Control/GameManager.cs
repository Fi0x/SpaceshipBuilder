using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Control
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject stationPrefab;
        [SerializeField] private long secondsUntilNextStation;
        
        private Stopwatch _stopwatch;
        private GameObject _station;
    
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
            this._stopwatch = new Stopwatch();
        }

        public void StartGame()
        {
            this.Running = true;
            this._stopwatch = new Stopwatch();
            this._stopwatch.Start();
        }

        public void GameOver(bool won)
        {
            if (!this.Running)
                return;
            
            this._stopwatch.Stop();
            this.Running = false;

            var tracker = StatTracker.Instance;
            tracker.PlayerWon = won;
            tracker.TimeInLevel = this._stopwatch.ElapsedMilliseconds;
            
            SceneChanger.LoadStatScreen();
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
            if(!this.Running)
                return;
            if(!this._stopwatch.IsRunning)
                return;
            
            if (this._stopwatch.ElapsedMilliseconds > 1000 * this.secondsUntilNextStation)
                this.SpawnStation();
        }

        private void SpawnStation()
        {
            if (this._station == null)
            {
                this._station = Instantiate(this.stationPrefab);
            }
        }

        public class NewGameManagerEventArgs : EventArgs
        {
            public GameManager NewInstance;
        }
    }
}