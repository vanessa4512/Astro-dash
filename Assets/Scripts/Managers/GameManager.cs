using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton

        public static GameManager Instance { get; private set; }

        private void Awake() {
            if (Instance != null && Instance != this)
            {
                DestroyImmediate(gameObject);

                return;
            }

            Instance = this;
        }

        #endregion

        [Header("Components")]
        [SerializeField] private UIManager uiManager;

        [Header("Difficulty Settings")]
        [SerializeField] private float initialSpeed;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float speedIncrement;
        [SerializeField] private float speedIncrementInterval;

        public float Speed { get; private set; }

        public float GameTime { get; private set; }

        public bool IsPlaying  { get; private set; }
        public bool IsGameOver { get; private set; }

        private float _speedIncrementTimer;

        private void Start() {
            Speed = initialSpeed;

            _speedIncrementTimer = 0;
            GameTime             = 0;
            if (uiManager != null) uiManager.UpdateGameScore(GameTime);

            IsPlaying  = false;
            IsGameOver = false;

            if (uiManager != null) uiManager.ToggleMainMenuUI(true);
            if (uiManager != null) uiManager.ToggleGameUI(false);
            if (uiManager != null) uiManager.ToggleGameOverUI(false);

            FindAnyObjectByType<PlayerInput>().enabled = false;

            if (Speed < 1)
            {
                Debug.LogWarning("Initial speed must be at least 1");
            }

            Speed = 1;

            if (uiManager == null || !uiManager.HasMainMenuUI) StartGame();
        }

        private void Update() {
            if (!IsPlaying) return;

            GameTime += Time.deltaTime;

            if (uiManager != null)
            {
                uiManager.UpdateGameScore(GameTime);
            }
            else
            {
                Debug.Log($"Score: {GameTime:F2}s");
            }

            _speedIncrementTimer += Time.deltaTime;

            if (_speedIncrementTimer < speedIncrementInterval) return;

            Speed                = Math.Min(Speed + speedIncrement, maxSpeed);
            _speedIncrementTimer = 0;
        }

        public void StartGame() {
            IsPlaying  = true;
            IsGameOver = false;

            FindAnyObjectByType<PlayerInput>().enabled = true;

            GameTime = 0;
            if (uiManager != null)
            {
                uiManager.UpdateGameScore(GameTime);
            }
            else
            {
                Debug.Log($"Score: {GameTime:F2}s");
            }

            if (uiManager != null) uiManager.ToggleMainMenuUI(false);
            if (uiManager != null) uiManager.ToggleGameUI(true);
            if (uiManager != null) uiManager.ToggleGameOverUI(false);
        }

        public void GameOver() {
            IsPlaying  = false;
            IsGameOver = true;

            if (uiManager != null) uiManager.ToggleMainMenuUI(false);
            if (uiManager != null) uiManager.ToggleGameUI(false);
            if (uiManager != null) uiManager.ToggleGameOverUI(true);
            if (uiManager != null) uiManager.UpdateGameOverScore(GameTime);
            if (uiManager == null) Debug.LogWarning("Game Over! (No UI Manager)");
        }

        public void RestartGame() {
            var currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }

        public void Quit() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}
