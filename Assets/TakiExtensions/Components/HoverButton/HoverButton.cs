using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace UI
{
    public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Animation Settings")]
        [SerializeField] private float hoverScaleMultiplier = 1.1f;
        [SerializeField] private float hoverRotationZ = 15f;
        [SerializeField] private float clickScaleMultiplier = 0.9f;
        [SerializeField] private float animationDuration = 0.1f;

        [Header("Events")]
        public UnityEvent onClick;

        private Vector3 _originalScale;
        private Quaternion _originalRotation;
        private Coroutine _animationCoroutine;
        private bool _isHovering;

        private void Awake()
        {
            _originalScale = transform.localScale;
            _originalRotation = transform.localRotation;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isHovering = true;
            UpdateVisualState();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isHovering = false;
            UpdateVisualState();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            StopAnimation();
            _animationCoroutine = StartCoroutine(AnimateTo(
                _originalScale * clickScaleMultiplier,
                transform.localRotation // Keep current rotation
            ));
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            UpdateVisualState();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke();
        }

        private void UpdateVisualState()
        {
            StopAnimation();
            if (_isHovering)
            {
                _animationCoroutine = StartCoroutine(AnimateTo(
                    _originalScale * hoverScaleMultiplier,
                    _originalRotation * Quaternion.Euler(0, 0, hoverRotationZ)
                ));
            }
            else
            {
                _animationCoroutine = StartCoroutine(AnimateTo(_originalScale, _originalRotation));
            }
        }

        private void StopAnimation()
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
                _animationCoroutine = null;
            }
        }

        private System.Collections.IEnumerator AnimateTo(Vector3 targetScale, Quaternion targetRotation)
        {
            Vector3 startScale = transform.localScale;
            Quaternion startRotation = transform.localRotation;
            float elapsed = 0f;

            while (elapsed < animationDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / animationDuration;
                
                // Lerp for smooth animation
                transform.localScale = Vector3.Lerp(startScale, targetScale, t);
                transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);
                
                yield return null;
            }

            transform.localScale = targetScale;
            transform.localRotation = targetRotation;
            _animationCoroutine = null;
        }
    }
}
