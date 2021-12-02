using UnityEngine;

namespace Control
{
    public class MainMenu : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Instance.InGameButtons.SetActive(false);
            GameManager.Instance.ItemInventory.SetActive(false);
        }

        public void PlayButtonClicked()
        {
            this.gameObject.SetActive(false);
            GameManager.Instance.InGameButtons.SetActive(true);
            GameManager.Instance.ItemInventory.SetActive(true);
            StatTracker.ResetTracker();
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