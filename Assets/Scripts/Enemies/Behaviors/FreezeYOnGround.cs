using UnityEngine;

namespace Enemies.Behaviors
{
    public class FreezeYOnGround : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Rigidbody2D   rb;
        [SerializeField] private BoxCollider2D groundCollider;

        private void Awake() {
            // Add a flag: constraints |= FreezePositionY
            // Remove a flag: constraints &= ~FreezePositionY
            // Toggle a flag: constraints ^= FreezePositionY
            // Check a flag: (constraints & FreezePositionY) != 0
            rb.constraints  &= ~RigidbodyConstraints2D.FreezePositionY;
            rb.gravityScale =  1f;
            rb.simulated    =  true;
        }

        private void OnCollisionStay2D(Collision2D other) {
            if (!groundCollider.enabled) return;
            if (!other.gameObject.CompareTag("Ground")) return;

            rb.constraints  |= RigidbodyConstraints2D.FreezePositionY;
            rb.gravityScale =  0;

            groundCollider.enabled = false;

            enabled = false;
        }

        private void OnDrawGizmos() {
            if (groundCollider == null) return;

            var center      = groundCollider.bounds.center;
            var radius      = groundCollider.bounds.extents.y;
            var groundLayer = LayerMask.NameToLayer("Ground");

            var hit = Physics2D.Raycast(center, Vector2.down, radius, 1 << groundLayer);
            Gizmos.color = hit ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);

            Gizmos.DrawSphere(center, radius);
        }
    }
}
