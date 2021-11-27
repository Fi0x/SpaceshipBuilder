using System;
using Parts;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Control
{
    public class SceneChanger : MonoBehaviour
    {
        public static event EventHandler InGameButtonClickedEvent;
        
        public static void LoadBuildingScene()
        {
            GameManager.Instance.Ship.GetComponent<Spaceship>().ResetShip();
            SceneManager.LoadScene("BuildingScene");
        }

        private static void LoadFlyingScene()
        {
            var gameManager = GameObject.Find("GameManager(Clone)");
            var gameManagerScript = gameManager.GetComponent<GameManager>();
            gameManagerScript.StartGame();
            DontDestroyOnLoad(gameManager);
            DontDestroyOnLoad(gameManagerScript.Ship);
            DontDestroyOnLoad(gameManagerScript.Menu);
            DontDestroyOnLoad(gameManagerScript.InGameButtons);
        
            // Activate Ship
            gameManagerScript.Ship.transform.position = new Vector3(0, -10, 0);
            gameManagerScript.Ship.transform.localScale = Vector3.one * 2;
        
            foreach (var script in gameManagerScript.Ship.GetComponentsInChildren<Weapon>())
                script.Working = true;

            gameManagerScript.ShipScript.currentMaxSpeed = Spaceship.MaxSpeed;
            foreach (var script in gameManagerScript.Ship.GetComponentsInChildren<Thruster>())
            {
                gameManagerScript.ShipScript.currentMaxSpeed += Thruster.SpeedIncrease;
                script.ThrusterDestroyedEvent += gameManagerScript.ShipScript.ThrusterDestroyedEventHandler;
            }
            
            GameManager.Instance.InGameButtons.SetActive(false);
        
            SceneManager.LoadScene("FlyingScene");
        }
        
        public static void LoadStatScreen()
        {
            Time.timeScale = 0;
            var statScreenPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Menus/StatScreen.prefab");
            Instantiate(statScreenPrefab);
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
        }
    }
}