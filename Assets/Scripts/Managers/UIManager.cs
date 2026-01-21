using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [Header("Event System & Default selections")]
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private GameObject  mainMenuDefaultSelection;
        [SerializeField] private GameObject  gameOverDefaultSelection;

        [Header("Main Menu")]
        [SerializeField] private GameObject mainMenuUI;

        [Header("Game UI")]
        [SerializeField] private GameObject gameUI;
        [SerializeField] private TMP_Text   gameScoreText;

        [Header("Game Over UI")]
        [SerializeField] private GameObject gameOverUI;
        [SerializeField] private TMP_Text   gameOverScoreText;

        public bool HasMainMenuUI => mainMenuUI != null;
        public bool HasGameUI => gameUI != null;
        public bool HasGameOverUI => gameOverUI != null;

        public void ToggleMainMenuUI(bool active) {
            if(!HasMainMenuUI) return;
            if (active) eventSystem.SetSelectedGameObject(mainMenuDefaultSelection);
            mainMenuUI.SetActive(active);
        }

        public void ToggleGameUI(bool active) {
            if(!HasGameUI) return;
            gameUI.SetActive(active);
        }

        public void UpdateGameScore(float score) {
            if(!HasGameUI) return;
            gameScoreText.text = score.ToString("F2") + "s";
        }

        public void ToggleGameOverUI(bool active) {
            if(!HasGameOverUI) return;
            if (active) eventSystem.SetSelectedGameObject(gameOverDefaultSelection);
            gameOverUI.SetActive(active);
        }

        public void UpdateGameOverScore(float score) {
            if(!HasGameOverUI) return;
            gameOverScoreText.text = score.ToString("F2") + "s";
        }
    }
}
