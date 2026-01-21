using System;
using Managers;
using UnityEngine;

namespace Environment
{
    public class ParallaxBackground : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private SpriteRenderer spriteRenderer;

        [Header("Settings")]
        [SerializeField] private float speed;

        private float _width;

        private void Awake() {
            var sprite = spriteRenderer.sprite;
            var width  = sprite.texture.width;
            var ppu    = sprite.pixelsPerUnit;

            _width = width / ppu;
        }


        private void Update() {
            if (!(GameManager.Instance?.IsPlaying ?? true)) return;

            var deltaMovement = speed * Time.deltaTime * (GameManager.Instance?.Speed ?? 1);
            var translation   = Vector3.right * deltaMovement;
            transform.Translate(translation);

            var currentX = transform.position.x;

            if (Math.Abs(currentX) < _width) return;

            var resetPosition = new Vector3(0, transform.position.y, transform.position.z);
            transform.position = resetPosition;
        }
    }
}
