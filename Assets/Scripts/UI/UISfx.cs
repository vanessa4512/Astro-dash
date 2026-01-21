using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class UISfx : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        // [Header("Components")]
        [SerializeField] private AudioSource audioSource;

        // [Header("Audio Clips")]
        [SerializeField] private AudioClip hoverSfx;
        [SerializeField] private AudioClip clickSfx;

        private void Awake() {
            if (audioSource == null)
            {
                Debug.LogError($"Audio Source is null in {gameObject.name}");
            }
        }

        public void OnPointerEnter(PointerEventData eventData) {
            audioSource.PlayOneShot(hoverSfx);
        }

        public void OnPointerExit(PointerEventData eventData) {
            audioSource.Stop();
        }

        public void OnPointerClick(PointerEventData eventData) {
            audioSource.PlayOneShot(clickSfx);
        }
    }
}
