using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Control
{
    public class Startup : MonoBehaviour
    {
        private void Start()
        {
            FillScene();
            SceneManager.LoadScene("BuildingScene");
        }

        private static void FillScene()
        {
            var gameManagerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/GameManager.prefab");
            var gameManager = Instantiate(gameManagerPrefab).GetComponent<GameManager>();
            gameManager.GetComponentInChildren<InventoryTracker>().Init();
            DontDestroyOnLoad(gameManager);
            Debug.Log("Game manager loaded");
        
            var spaceshipPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Ship/Spaceship.prefab");
            var spaceShip = Instantiate(spaceshipPrefab);
            DontDestroyOnLoad(spaceShip);
            Debug.Log("Spaceship loaded");

            var menuPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Menus/MainMenu.prefab");
            var menu = Instantiate(menuPrefab);
            DontDestroyOnLoad(menu);
            Debug.Log("Menu loaded");

            var buttonsPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Menus/InGameButtons.prefab");
            var buttons = Instantiate(buttonsPrefab);
            DontDestroyOnLoad(buttons);
            Debug.Log("Buttons loaded");

            var itemInventoryPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Menus/Inventory.prefab");
            var itemInventory = Instantiate(itemInventoryPrefab);
            DontDestroyOnLoad(itemInventory);
            Debug.Log("Inventory loaded");

            gameManager.InitShip(spaceShip);
            gameManager.Menu = menu;
            gameManager.InGameButtons = buttons;
            gameManager.ItemInventory = itemInventory;

            StatTracker.InstantiateTracker();
        }
    }
}