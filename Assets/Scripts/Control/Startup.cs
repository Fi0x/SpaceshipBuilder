using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Control
{
    [InitializeOnLoad]
    public class Startup
    {
        private static Startup Instance { get; }
    
        static Startup()
        {
            Instance = new Startup();

            SceneManager.sceneLoaded += Instance.FillScene;
        
            var gm = GameObject.Find("GameManager");
            if (gm == null)
            {
                SceneManager.GetActiveScene();
            }
        }

        private void FillScene(Scene scene, LoadSceneMode sceneMode)
        {
            var gameManagerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/GameManager.prefab");
            var gameManager = Object.Instantiate(gameManagerPrefab).GetComponent<GameManager>();
            Debug.Log("Game manager loaded");
        
            var spaceshipPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Ship/Spaceship.prefab");
            var spaceShip = Object.Instantiate(spaceshipPrefab);
            Debug.Log("Spaceship loaded");

            var menuPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Menu.prefab");
            var menu = Object.Instantiate(menuPrefab);
            Debug.Log("Menu loaded");

            gameManager.InitShip(spaceShip);
            gameManager.Menu = menu;

            SceneManager.sceneLoaded -= this.FillScene;
        }
    }
}