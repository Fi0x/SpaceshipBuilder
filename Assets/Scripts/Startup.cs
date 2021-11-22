using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        
        var spaceshipPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Spaceship.prefab");
        var spaceShip = Object.Instantiate(spaceshipPrefab);
        Debug.Log("Spaceship loaded");
        
        gameManager.InitShip(spaceShip);

        SceneManager.sceneLoaded -= this.FillScene;
    }
}
