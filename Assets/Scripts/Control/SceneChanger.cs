using System;
using FlightScripts;
using Parts;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Control
{
    public class SceneChanger : MonoBehaviour
    {
        public static event EventHandler<SceneChangedEventArgs> SceneChangedEvent;
        public static event EventHandler InGameButtonClickedEvent;

        public static void LoadBuildingScene()
        {
            GameManager.Instance.Ship.GetComponent<Spaceship>().ResetShip();
            SceneManager.LoadScene("BuildingScene");

            var args = new SceneChangedEventArgs { NewScene = Scene.Build};
            SceneChangedEvent?.Invoke(null, args);
        }

        private static void LoadFlyingScene()
        {
            GameManager.Instance.StartGame();
        
            // Activate Ship
            GameManager.Instance.Ship.transform.position = new Vector3(0, -10, 0);
            GameManager.Instance.Ship.transform.localScale = Vector3.one * 2;
        
            foreach (var script in GameManager.Instance.Ship.GetComponentsInChildren<Weapon>())
                script.Working = true;

            GameManager.Instance.ShipScript.currentMaxSpeed = Spaceship.MaxSpeed;
            foreach (var script in GameManager.Instance.Ship.GetComponentsInChildren<Thruster>())
            {
                GameManager.Instance.ShipScript.currentMaxSpeed += script.SpeedIncrease;
                script.ThrusterDestroyedEvent += GameManager.Instance.ShipScript.ThrusterDestroyedEventHandler;
            }
            
            GameManager.Instance.InGameButtons.SetActive(false);
            GameManager.Instance.ItemInventory.SetActive(false);
        
            SceneManager.LoadScene("FlyingScene");
            
            var args = new SceneChangedEventArgs { NewScene = Scene.Flight};
            SceneChangedEvent?.Invoke(null, args);
        }
        
        public static void LoadStatScreen()
        {
            Time.timeScale = 0;
            var statScreenPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Menus/StatScreen.prefab");
            Instantiate(statScreenPrefab);
            
            var args = new SceneChangedEventArgs { NewScene = Scene.Menu};
            SceneChangedEvent?.Invoke(null, args);
        }

        public void StartButtonClicked()
        {
            InGameButtonClickedEvent?.Invoke(null, null);
            LoadFlyingScene();
        }

        public void MenuButtonClicked()
        {
            InGameButtonClickedEvent?.Invoke(null, null);
            this.gameObject.SetActive(false);
            GameManager.Instance.Menu.gameObject.SetActive(true);
            
            var args = new SceneChangedEventArgs { NewScene = Scene.Menu};
            SceneChangedEvent?.Invoke(null, args);
        }
        
        public class SceneChangedEventArgs : EventArgs
        {
            public Scene NewScene;
        }
        
        public enum Scene
        {
            Menu,
            Build,
            Flight
        }
    }
}