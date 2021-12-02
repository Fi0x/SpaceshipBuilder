using System;
using System.Diagnostics;
using FlightScripts;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Control
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject stationPrefab;
        [SerializeField] private long secondsBetweenStations = 20;
        
        private Stopwatch _stopwatch;
        private long _nextStationStopwatchTime;
    
        public static event EventHandler<NewGameManagerEventArgs> GameManagerInstantiatedEvent;
        public static event EventHandler<LevelCompletedEventArgs> LevelCompletedEvent;
        
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
        public GameObject Ship { get; private set; }
        public Spaceship ShipScript { get; private set; }
        public GameObject ItemInventory { get; set; }
        public static bool Running { get; private set; }

        private void Awake()
        {
            Instance = this;
            this._stopwatch = new Stopwatch();
        }

        public void StartGame()
        {
            Running = true;
            this._stopwatch = new Stopwatch();
            this._stopwatch.Start();
            this._nextStationStopwatchTime = this.secondsBetweenStations * 1000;
        }

        public void GameOver(bool won)
        {
            if (!Running)
                return;
            
            this._stopwatch.Stop();
            Running = false;

            var eventArgs = new LevelCompletedEventArgs
            {
                Won = won,
                TimeForLevel = this._stopwatch.ElapsedMilliseconds
            };
            LevelCompletedEvent?.Invoke(null, eventArgs);
            
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
            if(!Running)
                return;
            if(!this._stopwatch.IsRunning)
                return;
            
            if (this._stopwatch.ElapsedMilliseconds < this._nextStationStopwatchTime)
                return;
            
            this.SpawnStation();
            this._nextStationStopwatchTime += this.secondsBetweenStations * 1000;
            
        }

        private void SpawnStation()
        {
            var station = Instantiate(this.stationPrefab);
            station.GetComponent<Station>().SpawnStation();
        }

        public class NewGameManagerEventArgs : EventArgs
        {
            public GameManager NewInstance;
        }
        public class LevelCompletedEventArgs : EventArgs
        {
            public bool Won;
            public long TimeForLevel;
        }
    }
}