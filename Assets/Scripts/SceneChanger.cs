using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private GameObject ship;

    private static SceneChanger _instance;
    public static SceneChanger Instance => _instance;

    private void Start()
    {
        _instance = this;
    }

    public void StartButtonClicked()
    {
        this.ChangeScene();
    }

    public void QuitButtonClicked()
    {
        Application.Quit();
    }

    public void ChangeScene()
    {
        var currentScene = SceneManager.GetActiveScene().name;
        if (currentScene.Equals("FlyingScene"))
        {
            Destroy(GameObject.Find("GameManager"));
            Destroy(this.ship);
            SceneManager.LoadScene("BuildingScene");
            return;
        }
        
        var gameManager = GameObject.Find("GameManager");
        var gameManagerScript = gameManager.GetComponent<GameManager>();
        gameManagerScript.startGame();
        DontDestroyOnLoad(gameManager);
        DontDestroyOnLoad(this.ship);
        // Activate Ship
        this.ship.transform.position = new Vector3(0, -10, 0);
        this.ship.AddComponent(typeof(Spaceship));
        this.ship.transform.localScale = Vector3.one * 2;
        foreach (var script in this.ship.GetComponentsInChildren<Weapon>())
        {
            script.Enable();
            Debug.Log("Enabling Weapon");
        }

        SceneManager.LoadScene("FlyingScene");
    }
}
