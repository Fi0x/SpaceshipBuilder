using Parts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Control
{
    public class SceneChanger : MonoBehaviour
    {
        public static void LoadBuildingScene()
        {
            var gameManager = GameObject.Find("GameManager(Clone)");
            var gameManagerScript = gameManager.GetComponent<GameManager>();
            gameManagerScript.Ship.GetComponent<Spaceship>().ResetShip();
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
        
            // Activate Ship
            gameManagerScript.Ship.transform.position = new Vector3(0, -10, 0);
            gameManagerScript.Ship.transform.localScale = Vector3.one * 2;
        
            foreach (var script in gameManagerScript.Ship.GetComponentsInChildren<Weapon>())
                script.Working = true;

            foreach (var script in gameManagerScript.Ship.GetComponentsInChildren<Thruster>())
            {
                gameManagerScript.ShipScript.currentMaxSpeed += Thruster.SpeedIncrease;
                script.ThrusterDestroyedEvent += gameManagerScript.ShipScript.ThrusterDestroyedEventHandler;
            }
        
            SceneManager.LoadScene("FlyingScene");
        }

        public void StartButtonClicked()
        {
            LoadFlyingScene();
        }

        public void MenuButtonClicked()
        {
            this.gameObject.SetActive(false);
            GameManager.Instance.Menu.gameObject.SetActive(true);
        }
    }
}