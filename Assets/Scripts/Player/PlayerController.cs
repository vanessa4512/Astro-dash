using System;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private static readonly int VerticalSpeedAnimHash = Animator.StringToHash("VerticalSpeed");
        private static readonly int IsJumpingAnimHash     = Animator.StringToHash("IsJumping");
        private static readonly int DieAnimHash           = Animator.StringToHash("Die");

        [Header("Components")]
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Animator    animator;
        [SerializeField] private AudioSource audioSource;

        [Header("Settings")]
        [SerializeField] private float jumpForce;

        [Header("Audio")]
        [SerializeField] private AudioClip jumpClip;
        [SerializeField] private AudioClip dieClip;


        private bool _isGrounded;
        private bool _isDead;

        private void Awake() {
            if (rb == null)
            {
                Debug.LogError($"Rigidbody is null in {gameObject.name}");
            }
        }

        private void Start() {
            _isDead = false;
        }

        private void Update() {
            if (_isDead) return;

            if (animator != null) animator.SetFloat(VerticalSpeedAnimHash, rb.linearVelocityY);
        }

        public void OnJump(InputAction.CallbackContext context) {
            if (_isDead) return;

            if (!context.performed || !_isGrounded) return;

            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _isGrounded = false;

            if (animator != null) animator.SetBool(IsJumpingAnimHash, true);
            if (audioSource != null) audioSource.PlayOneShot(jumpClip);
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            if (_isDead) return;

            if (collision.gameObject.CompareTag("Ground"))
            {
                _isGrounded = true;
                if (animator != null) animator.SetBool(IsJumpingAnimHash, false);

                return;
            }

            if (!_isDead)
            {
                Debug.LogWarning($"Player collided with {collision.gameObject.name}, but not with the ground");
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (_isDead) return;

            if (!other.gameObject.transform.root.CompareTag("Enemy"))
            {
                Debug.LogWarning($"Player collided with trigger: {other.gameObject.name}, but not with an enemy");

                return;
            }

            _isDead = true;

            if (audioSource != null) audioSource.PlayOneShot(dieClip);
            if (animator != null) animator.SetTrigger(DieAnimHash);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
            else
            {
                Debug.LogWarning("Player is dead! (No Game Manager)");
            }
        }
    }
}
