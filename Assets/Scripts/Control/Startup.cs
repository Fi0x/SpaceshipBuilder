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
            Object.DontDestroyOnLoad(gameManager);
            Debug.Log("Game manager loaded");
        
            var spaceshipPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Ship/Spaceship.prefab");
            var spaceShip = Object.Instantiate(spaceshipPrefab);
            Object.DontDestroyOnLoad(spaceShip);
            Debug.Log("Spaceship loaded");

            var menuPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Menus/MainMenu.prefab");
            var menu = Object.Instantiate(menuPrefab);
            Object.DontDestroyOnLoad(menu);
            Debug.Log("Menu loaded");

            var buttonsPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Menus/InGameButtons.prefab");
            var buttons = Object.Instantiate(buttonsPrefab);
            Object.DontDestroyOnLoad(buttons);
            Debug.Log("Buttons loaded");

            var buildInventoryPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Building/BuildingInventory.prefab");
            var buildInventory = Object.Instantiate(buildInventoryPrefab);
            Object.DontDestroyOnLoad(buildInventory);
            var weaponInventoryPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Building/WeaponInventory.prefab");
            var weaponInventory = Object.Instantiate(weaponInventoryPrefab);
            Object.DontDestroyOnLoad(weaponInventory);
            var thrusterInventoryPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Building/ThrusterInventory.prefab");
            var thrusterInventory = Object.Instantiate(thrusterInventoryPrefab);
            Object.DontDestroyOnLoad(thrusterInventory);
            Debug.Log("Inventories loaded");

            gameManager.InitShip(spaceShip);
            gameManager.Menu = menu;
            gameManager.InGameButtons = buttons;
            gameManager.BuildInventory = buildInventory;
            gameManager.WeaponInventory = weaponInventory;
            gameManager.ThrusterInventory = thrusterInventory;

            StatTracker.InstantiateTracker();

            SceneManager.sceneLoaded -= this.FillScene;
        }
    }
}