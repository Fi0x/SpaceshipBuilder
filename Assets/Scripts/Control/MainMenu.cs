using System;
using UnityEngine;

namespace Control
{
    public class MainMenu : MonoBehaviour
    {
        public static event EventHandler MenuButtonClickedEvent;
        
        private void Start()
        {
            GameManager.Instance.InGameButtons.SetActive(false);
        }

        public void PlayButtonClicked()
        {
            MenuButtonClickedEvent?.Invoke(null, null);
            this.gameObject.SetActive(false);
            GameManager.Instance.InGameButtons.SetActive(true);
            StatTracker.ResetTracker();
        }

        public void SettingsButtonClicked()
        {
            MenuButtonClickedEvent?.Invoke(null, null);
        }

        public void QuitButtonClicked()
        {
            MenuButtonClickedEvent?.Invoke(null, null);
            Application.Quit();
        }
    }
}