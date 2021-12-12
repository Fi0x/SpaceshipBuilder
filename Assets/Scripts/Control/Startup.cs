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

            var partInventoryPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PartInventory.prefab");
            var partInventory = Object.Instantiate(partInventoryPrefab);
            Object.DontDestroyOnLoad(partInventory);
            Debug.Log("Part Inventory loaded");

            var menuPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Menus/MainMenu.prefab");
            var menu = Object.Instantiate(menuPrefab);
            Object.DontDestroyOnLoad(menu);
            Debug.Log("Menu loaded");

            var buttonsPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Menus/InGameButtons.prefab");
            var buttons = Object.Instantiate(buttonsPrefab);
            Object.DontDestroyOnLoad(buttons);
            Debug.Log("Buttons loaded");

            var itemInventoryPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Menus/InventoryReduced.prefab");
            var itemInventory = Object.Instantiate(itemInventoryPrefab);
            Object.DontDestroyOnLoad(itemInventory);
            Debug.Log("Inventory loaded");

            gameManager.InitShip(spaceShip);
            gameManager.Menu = menu;
            gameManager.InGameButtons = buttons;
            gameManager.ItemInventory = itemInventory;

            StatTracker.InstantiateTracker();

            SceneManager.sceneLoaded -= this.FillScene;
        }
    }
}