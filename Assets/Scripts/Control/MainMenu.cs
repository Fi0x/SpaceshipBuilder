using System;
using UnityEngine;

namespace Control
{
    public class MainMenu : MonoBehaviour
    {
        private GameObject _inGameButtons;
        private void Start()
        {
            this._inGameButtons = GameObject.Find("InGameButtons");
            this._inGameButtons.SetActive(false);
        }

        public void PlayButtonClicked()
        {
            this.gameObject.SetActive(false);
            this._inGameButtons.SetActive(true);
        }

        public void SettingsButtonClicked()
        {
        
        }

        public void QuitButtonClicked()
        {
            Application.Quit();
        }
    }
}