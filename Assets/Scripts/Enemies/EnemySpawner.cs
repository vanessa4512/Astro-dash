using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Enemies")]
        [SerializeField] private List<Enemy> enemies;

        [Header("Spawners")]
        [SerializeField] private List<Transform> spawners;

        [Header("Settings")]
        [SerializeField] private float startingSpawnRate;
        [SerializeField] private float minSpawnRate;
        [SerializeField] private float decreaseSpawnRateBy;
        [SerializeField] private float increaseSpawnRateAt;

        private float _currentSpawnRate;
        private float _increaseSpawnRateAt;
        private float _spawnTimer;

        private void Awake() {
            _currentSpawnRate    = startingSpawnRate;
            _spawnTimer          = _currentSpawnRate + 1f;
            _increaseSpawnRateAt = increaseSpawnRateAt;
        }

        private void Update() {
            if (!(GameManager.Instance?.IsPlaying ?? true)) return;

            _spawnTimer += Time.deltaTime;

            IncreaseSpawnRateIfNecessary();

            if (_spawnTimer < _currentSpawnRate) return;

            SpawnEnemy();

            _spawnTimer = 0;
        }

        private void IncreaseSpawnRateIfNecessary() {
            if (Mathf.Approximately(_currentSpawnRate, minSpawnRate)) return;

            if (GameManager.Instance == null) return;
            if (!GameManager.Instance.IsPlaying) return;
            if (GameManager.Instance.GameTime < _increaseSpawnRateAt) return;

            _increaseSpawnRateAt += increaseSpawnRateAt;
            var newSpawnRate = _currentSpawnRate - decreaseSpawnRateBy;
            _currentSpawnRate = Mathf.Max(newSpawnRate, minSpawnRate);
        }

        private void SpawnEnemy() {
            var spawnPointIdx = Random.Range(0, spawners.Count);
            var spawnPoint    = spawners[spawnPointIdx];

            var enemyIdx = Random.Range(0, enemies.Count);
            var enemy    = enemies[enemyIdx];

            Instantiate(enemy, spawnPoint.position, Quaternion.identity);
        }
    }
}
