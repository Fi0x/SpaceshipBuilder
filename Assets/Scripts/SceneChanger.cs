using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private GameObject ship;

    public static GameObject Ship;
    public void StartButtonClicked()
    {
        this.SaveShip();
    }

    public void SaveShip()
    {
        Ship = this.ship;
    }
}
