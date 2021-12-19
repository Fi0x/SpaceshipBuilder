using System;
using UnityEngine;
using UnityEngine.UI;

namespace Control
{
    public class StatMenu : MonoBehaviour
    {
        [SerializeField] private Text title;
        [SerializeField] private Text destroyedAsteroidsValue;
        [SerializeField] private Text lostShipPartsValue;
        [SerializeField] private Text destroyedEnemiesValue;
        [SerializeField] private Text collectedResourcesValue;
        [SerializeField] private Text timeInLevelValue;
        [SerializeField] private Text timeInCurrentRunValue;
        [SerializeField] private Text completedLevelsValue;
        [SerializeField] private Text usedShipPartsValue;
        [SerializeField] private Text totalScoreValueValue;
        [SerializeField] private Text buttonText;

        public static event EventHandler StatMenuClosedEvent;

        private void Start()
        {
            this.destroyedAsteroidsValue.text = StatTracker.Instance.DestroyedAsteroids.ToString();
            this.lostShipPartsValue.text = StatTracker.Instance.LostShipParts.ToString();
            this.destroyedEnemiesValue.text = StatTracker.Instance.DestroyedEnemies.ToString();
            this.collectedResourcesValue.text = StatTracker.Instance.CollectedResources.ToString();
            this.timeInLevelValue.text = $"{(double)StatTracker.Instance.TimeInLevel / 1000}s";
            this.timeInCurrentRunValue.text = $"{(double)StatTracker.Instance.TotalTime / 1000}s";
            this.completedLevelsValue.text = StatTracker.Instance.CompletedLevels.ToString();
            this.usedShipPartsValue.text = StatTracker.Instance.UsedShipParts.ToString();
            this.totalScoreValueValue.text = StatTracker.Instance.TotalScore.ToString();
            this.buttonText.text = StatTracker.Instance.PlayerWon ? "Next Level" : "Return to Menu";
        }

        public void ContinueButtonClicked()
        {
            Time.timeScale = 1;
            StatMenuClosedEvent?.Invoke(null, null);
            SceneChanger.LoadBuildingScene();
            GameManager.Instance.Menu.SetActive(!StatTracker.Instance.PlayerWon);
            GameManager.Instance.InGameButtons.SetActive(StatTracker.Instance.PlayerWon);
            GameManager.Instance.ItemInventory.SetActive(StatTracker.Instance.PlayerWon);
        }
    }
}