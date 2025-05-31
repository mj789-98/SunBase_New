using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace SunBase.Controllers
{
    public class CommentPanel : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Transform commentContainer;
        [SerializeField] private GameObject commentPrefab;
        [SerializeField] private Button closeButton;
        [SerializeField] private TextMeshProUGUI postContentPreview;
        [SerializeField] private CanvasGroup canvasGroup;

        [Header("Animation Settings")]
        [SerializeField] private float commentSpawnDelay = 0.1f;
        [SerializeField] private float commentFadeDuration = 0.2f;

        private void Awake()
        {
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(OnCloseButtonClicked);
            }

            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            
            // Start hidden
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            gameObject.SetActive(true);
        }

        public void ShowComments(string postContent, string[] comments)
        {
            StopAllCoroutines();
            StartCoroutine(ShowCommentsRoutine(postContent, comments));
        }

        private IEnumerator ShowCommentsRoutine(string postContent, string[] comments)
        {
            // Clear existing comments
            foreach (Transform child in commentContainer)
            {
                Destroy(child.gameObject);
            }

            // Show post content preview with fade
            if (postContentPreview != null)
            {
                postContentPreview.text = postContent;
                postContentPreview.alpha = 0f;
                yield return StartCoroutine(FadeTextMeshPro(postContentPreview, 1f));
            }

            // Show panel
            yield return StartCoroutine(UIAnimationManager.FadePanel(canvasGroup, true));

            // Spawn comments with animation
            if (comments != null && comments.Length > 0)
            {
                foreach (string comment in comments)
                {
                    yield return StartCoroutine(SpawnCommentWithAnimation(comment));
                    yield return new WaitForSeconds(commentSpawnDelay);
                }
            }
            else
            {
                yield return StartCoroutine(SpawnCommentWithAnimation("No comments yet."));
            }
        }

        private void OnCloseButtonClicked()
        {
            StartCoroutine(HideCommentsRoutine());
        }

        private IEnumerator HideCommentsRoutine()
        {
            yield return StartCoroutine(UIAnimationManager.FadePanel(canvasGroup, false));
        }

        private IEnumerator SpawnCommentWithAnimation(string commentText)
        {
            GameObject commentObj = Instantiate(commentPrefab, commentContainer);
            TextMeshProUGUI textComponent = commentObj.GetComponent<TextMeshProUGUI>();
            
            if (textComponent != null)
            {
                textComponent.text = commentText;
                textComponent.alpha = 0f;
                
                // Scale animation
                Vector3 originalScale = commentObj.transform.localScale;
                commentObj.transform.localScale = Vector3.zero;
                
                float elapsedTime = 0f;
                float duration = 0.3f;
                
                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    float normalizedTime = elapsedTime / duration;
                    
                    // Animate scale and alpha
                    commentObj.transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, normalizedTime);
                    textComponent.alpha = Mathf.Lerp(0f, 1f, normalizedTime);
                    
                    yield return null;
                }
                
                commentObj.transform.localScale = originalScale;
                textComponent.alpha = 1f;
            }
        }

        private IEnumerator FadeTextMeshPro(TextMeshProUGUI text, float targetAlpha)
        {
            float startAlpha = text.alpha;
            float elapsedTime = 0f;
            
            while (elapsedTime < commentFadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float normalizedTime = elapsedTime / commentFadeDuration;
                text.alpha = Mathf.Lerp(startAlpha, targetAlpha, normalizedTime);
                yield return null;
            }
            
            text.alpha = targetAlpha;
        }

        private void OnDestroy()
        {
            if (closeButton != null)
            {
                closeButton.onClick.RemoveAllListeners();
            }
        }
    }
} 