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
        [SerializeField] private double distanceBetweenStations = 200;
        
        private Stopwatch _stopwatch;
    
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
        }

        public void StartGame()
        {
            Running = true;
            this._stopwatch = new Stopwatch();
            this._stopwatch.Start();
            this.DistanceToNextStation = this.distanceBetweenStations;
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
            
            Debug.Log("Distance: " + this.DistanceToNextStation);
            if(this.DistanceToNextStation > 0)
                return;
            
            this.SpawnStation();
            this.DistanceToNextStation = this.distanceBetweenStations;
        }

        private void SpawnStation()
        {
            var station = Instantiate(this.stationPrefab);
            station.GetComponent<Station>().SpawnStation();
        }
        
        public class LevelCompletedEventArgs : EventArgs
        {
            public bool Won;
            public long TimeForLevel;
        }
    }
}