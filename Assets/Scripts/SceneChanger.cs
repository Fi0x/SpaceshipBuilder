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
        SceneManager.LoadScene("FlyingScene");
    }
}
