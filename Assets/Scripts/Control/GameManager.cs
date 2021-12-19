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

        // Bug in inspector?
        [Header("Level Length")]
        [Header("Level Params")]
        [Space(5)]
        [SerializeField] private int LengthStarting;
        [SerializeField] private int LenghtIncrement;
        [Space(5)]
        [Header("Asteroid Count")]
        [SerializeField] private int AsteroidsStarting;
        [SerializeField] private int AsteroidsIncrement;
        [Space(5)]
        [Header("Enemy Count")]
        [SerializeField] private int EnemiesStarting;
        [SerializeField] private int EnemiesIncrement;

        private Stopwatch _stopwatch;
        private double _distanceBetweenStations;
        private int _level = 0;
    
        public static event EventHandler<LevelCompletedEventArgs> LevelCompletedEvent;
        
        public static GameManager Instance { get; private set; }
        public GameObject Menu { get; set; }
        public GameObject InGameButtons { get; set; }
        public GameObject Ship { get; private set; }
        public Spaceship ShipScript { get; private set; }
        public GameObject ItemInventory { get; set; }
        public static bool Running { get; private set; }
        public double DistanceToNextStation { get; set; }

        private void Awake()
        {
            Instance = this;
            this._stopwatch = new Stopwatch();
            this._distanceBetweenStations = LengthStarting + _level * LenghtIncrement;
        }

        public void StartGame()
        {
            Running = true;
            this._stopwatch = new Stopwatch();
            this._stopwatch.Start();
            this.DistanceToNextStation = this._distanceBetweenStations;
        }

        public void GameOver(bool won)
        {
            if (!Running)
                return;
            
            this._stopwatch.Stop();
            Running = false;

            if (!won)
                this._level = 0;
            else
                this._level++;
                var inventoryTracker = this.gameObject.GetComponentInChildren<InventoryTracker>();
                if (inventoryTracker._inventory["Weapon0"] == 0 &&
                    inventoryTracker._inventory["Weapon1"] == 0 &&
                    inventoryTracker._inventory["Weapon2"] == 0) {
                inventoryTracker.AddGun(0);
                }

            var eventArgs = new LevelCompletedEventArgs
            {
                Won = won,
                TimeForLevel = this._stopwatch.ElapsedMilliseconds
            };
            LevelCompletedEvent?.Invoke(null, eventArgs);
            
            SceneChanger.LoadStatScreen(won);
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
            
            //Debug.Log("Distance to Go: " + this.DistanceToNextStation);
            if(this.DistanceToNextStation > 0)
                return;
            
            this.SpawnStation();
            this.DistanceToNextStation = this._distanceBetweenStations;
        }

        private void SpawnStation()
        {
            var station = Instantiate(this.stationPrefab);
            station.GetComponent<Station>().SpawnStation();
        }
        // Level Management
        public Vector2 GetEnemyManagerParams()
        {
            int enemyCount = EnemiesStarting + _level * EnemiesIncrement;
            return new Vector2(enemyCount, (float)(_distanceBetweenStations / (20f * enemyCount)));
        }

        public int GetAsteroidManagerParams()
        {
            return AsteroidsStarting + _level * AsteroidsIncrement;
        }
        
        public class LevelCompletedEventArgs : EventArgs
        {
            public bool Won;
            public long TimeForLevel;
        }

    }
}