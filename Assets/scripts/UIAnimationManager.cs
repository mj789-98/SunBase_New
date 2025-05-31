using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

namespace SunBase.Controllers
{
    public static class UIAnimationManager
    {
        // Animation durations
        private const float FADE_DURATION = 0.3f;
        private const float SCALE_DURATION = 0.1f;
        private const float BUTTON_SCALE = 0.95f;

        // Fade panel in/out
        public static IEnumerator FadePanel(CanvasGroup panel, bool fadeIn, System.Action onComplete = null)
        {
            float startAlpha = panel.alpha;
            float targetAlpha = fadeIn ? 1f : 0f;
            float elapsedTime = 0f;

            while (elapsedTime < FADE_DURATION)
            {
                elapsedTime += Time.deltaTime;
                float normalizedTime = elapsedTime / FADE_DURATION;
                panel.alpha = Mathf.Lerp(startAlpha, targetAlpha, normalizedTime);
                yield return null;
            }

            panel.alpha = targetAlpha;
            panel.blocksRaycasts = fadeIn;
            panel.interactable = fadeIn;

            onComplete?.Invoke();
        }

        // Button click animation
        public static IEnumerator AnimateButtonClick(Transform buttonTransform)
        {
            Vector3 originalScale = buttonTransform.localScale;
            Vector3 targetScale = originalScale * BUTTON_SCALE;

            // Scale down
            float elapsedTime = 0f;
            while (elapsedTime < SCALE_DURATION)
            {
                elapsedTime += Time.deltaTime;
                float normalizedTime = elapsedTime / SCALE_DURATION;
                buttonTransform.localScale = Vector3.Lerp(originalScale, targetScale, normalizedTime);
                yield return null;
            }

            // Scale back up
            elapsedTime = 0f;
            while (elapsedTime < SCALE_DURATION)
            {
                elapsedTime += Time.deltaTime;
                float normalizedTime = elapsedTime / SCALE_DURATION;
                buttonTransform.localScale = Vector3.Lerp(targetScale, originalScale, normalizedTime);
                yield return null;
            }

            buttonTransform.localScale = originalScale;
        }

        // Like count animation
        public static IEnumerator AnimateLikeCount(TextMeshProUGUI textComponent, int fromValue, int toValue)
        {
            float elapsedTime = 0f;
            float duration = 0.5f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float normalizedTime = elapsedTime / duration;
                int currentValue = Mathf.RoundToInt(Mathf.Lerp(fromValue, toValue, normalizedTime));
                textComponent.text = currentValue.ToString();
                yield return null;
            }

            textComponent.text = toValue.ToString();
        }

        // Color transition animation
        public static IEnumerator TransitionColor(Image image, Color targetColor, float duration = 0.2f)
        {
            Color startColor = image.color;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float normalizedTime = elapsedTime / duration;
                image.color = Color.Lerp(startColor, targetColor, normalizedTime);
                yield return null;
            }

            image.color = targetColor;
        }
    }
} 