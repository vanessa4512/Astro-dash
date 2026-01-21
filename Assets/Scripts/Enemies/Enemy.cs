using Managers;
using UnityEngine;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Rigidbody2D rb;

        [Header("Settings")]
        [SerializeField] private float speed;
        [SerializeField] private float xLimit;

        private void Start() {
            rb.linearVelocityX = speed * (GameManager.Instance?.Speed ?? 1);
        }

        private void Update() {
            if (transform.position.x < xLimit)
            {
                Destroy(gameObject);
                return;
            }

            if (!(GameManager.Instance?.IsGameOver ?? false)) return;

            rb.linearVelocityX = 0;
            rb.simulated       = false;
            enabled            = false;
        }
    }
}
