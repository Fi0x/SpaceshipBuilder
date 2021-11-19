using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private GameObject ship;

    public static GameObject Ship;
    public void StartButtonClicked()
    {
        this.SaveShip();
        this.ChangeScene();
    }

    public void SaveShip()
    {
        Ship = this.ship;
    }

    public void ChangeScene()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        GameManager gameManagerScript = gameManager.GetComponent<GameManager>();
        gameManagerScript.startGame();
        DontDestroyOnLoad(gameManager);
        DontDestroyOnLoad(ship);
        // Activate Ship
        ship.transform.position = new Vector3(0, -10, 0);
        ship.AddComponent(typeof(Spaceship));
        ship.transform.localScale = Vector3.one * 2;
        foreach (Weapon script in ship.GetComponentsInChildren<Weapon>())
        {
            script.Enable();
            Debug.Log("Enabling Weapon");
        }

        SceneManager.LoadScene("FlyingScene");
    }
}
