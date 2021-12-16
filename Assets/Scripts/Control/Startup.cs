using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Control
{
    public class Startup : MonoBehaviour
    {
        [SerializeField] GameObject gameManagerPrefab;
        [SerializeField] GameObject spaceshipPrefab;
        [SerializeField] GameObject menuPrefab;
        [SerializeField] GameObject buttonsPrefab;
        [SerializeField] GameObject itemInventoryPrefab;

        private void Start()
        {
            FillScene();
            SceneManager.LoadScene("BuildingScene");
        }

        private void FillScene()
        {
            var gameManager = Instantiate(gameManagerPrefab).GetComponent<GameManager>();
            gameManager.GetComponentInChildren<InventoryTracker>().Init();
            DontDestroyOnLoad(gameManager);
            Debug.Log("Game manager loaded");
        
            var spaceShip = Instantiate(spaceshipPrefab);
            DontDestroyOnLoad(spaceShip);
            Debug.Log("Spaceship loaded");

            var menu = Instantiate(menuPrefab);
            DontDestroyOnLoad(menu);
            Debug.Log("Menu loaded");

            var buttons = Instantiate(buttonsPrefab);
            DontDestroyOnLoad(buttons);
            Debug.Log("Buttons loaded");

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