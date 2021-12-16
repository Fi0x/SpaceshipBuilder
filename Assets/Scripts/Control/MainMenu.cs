using System;
using UnityEngine;
using UnityEngine.UI;

namespace Control
{
    public class MainMenu : MonoBehaviour
    {
        public static event EventHandler<SceneChanger.SceneChangedEventArgs> MenuButtonClickedEvent;
        public static event EventHandler<VolumeChangedEventArgs> VolumeChangedEvent;
        
        private void Start()
        {
            GameManager.Instance.InGameButtons.SetActive(false);
            GameManager.Instance.ItemInventory.SetActive(false);
        }

        public void PlayButtonClicked()
        {
            var args = new SceneChanger.SceneChangedEventArgs() { NewScene = SceneChanger.Scene.Build};
            MenuButtonClickedEvent?.Invoke(null, args);
            this.gameObject.SetActive(false);
            GameManager.Instance.InGameButtons.SetActive(true);
            GameManager.Instance.ItemInventory.SetActive(true);
            GameManager.Instance.ResetShip();
            InventoryTracker.Instance.Init();
            StatTracker.ResetTracker();
        }

        public void EffectVolumeChanged(Slider sender)
        {
            var args = new VolumeChangedEventArgs
            {
                Effects = true,
                NewVolume = sender.value
            };
            
            VolumeChangedEvent?.Invoke(null, args);
        }

        public void MusicVolumeChanged(Slider sender)
        {
            var args = new VolumeChangedEventArgs
            {
                Effects = false,
                NewVolume = sender.value
            };
            
            VolumeChangedEvent?.Invoke(null, args);
        }

        public void QuitButtonClicked()
        {
            MenuButtonClickedEvent?.Invoke(null, null);
            Application.Quit();
        }
        
        public class VolumeChangedEventArgs : EventArgs
        {
            public bool Effects;
            public float NewVolume;
        }
    }
}