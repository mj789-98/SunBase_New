using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SunBase.Data;

namespace SunBase.Controllers
{
    public class PostController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI usernameText;
        [SerializeField] private TextMeshProUGUI contentText;
        [SerializeField] private Image profilePicture;
        [SerializeField] private Button likeButton;
        [SerializeField] private TextMeshProUGUI likeCountText;
        [SerializeField] private Image likeButtonImage;
        [SerializeField] private Button commentButton;
        [SerializeField] private TextMeshProUGUI commentCountText;

        [Header("Comment System")]
        [SerializeField] private CommentPanel commentPanel;

        [Header("Animation Settings")]
        [SerializeField] private ColorBlock likeButtonColors = ColorBlock.defaultColorBlock;

        private PostData postData;
        private Color likedColor = new Color(1f, 0.2f, 0.2f);
        private Color unlikedColor = Color.white;

        private void Start()
        {
            SetupButtons();
            SetupButtonColors();
        }

        private void SetupButtons()
        {
            if (likeButton != null)
            {
                likeButton.onClick.AddListener(OnLikeButtonClicked);
            }

            if (commentButton != null)
            {
                commentButton.onClick.AddListener(OnCommentButtonClicked);
            }
        }

        private void SetupButtonColors()
        {
            if (likeButton != null)
            {
                likeButtonColors.normalColor = Color.white;
                likeButtonColors.highlightedColor = new Color(0.95f, 0.95f, 0.95f);
                likeButtonColors.pressedColor = new Color(0.85f, 0.85f, 0.85f);
                likeButtonColors.selectedColor = Color.white;
                likeButtonColors.disabledColor = new Color(0.8f, 0.8f, 0.8f);
                likeButtonColors.colorMultiplier = 1f;
                likeButtonColors.fadeDuration = 0.1f;

                likeButton.colors = likeButtonColors;
            }
        }

        public void InitializePost(PostData data)
        {
            postData = data;

            if (!string.IsNullOrEmpty(postData.UniqueId))
            {
                postData.isLiked = LikeStateManager.GetLikeState(postData.UniqueId, postData.isLiked);
                postData.likes = LikeStateManager.GetLikeCount(postData.UniqueId, postData.likes);
            }

            if (likeCountText != null)
            {
                likeCountText.text = postData.likes.ToString();
            }

            UpdateUI();
        }

        private void UpdateUI()
        {
            if (postData == null) return;

            if (usernameText != null) usernameText.text = postData.username;
            if (contentText != null) contentText.text = postData.content;
            
            if (likeCountText != null)
            {
                likeCountText.text = postData.likes.ToString();
            }

            if (likeButtonImage != null)
            {
                likeButtonImage.color = postData.isLiked ? likedColor : unlikedColor;
            }
            
            if (commentCountText != null && postData.comments != null)
            {
                commentCountText.text = postData.comments.Length.ToString();
            }
        }

        private void OnLikeButtonClicked()
        {
            if (postData == null) return;

            StartCoroutine(UIAnimationManager.AnimateButtonClick(likeButton.transform));

            postData.isLiked = !postData.isLiked;
            postData.likes += postData.isLiked ? 1 : -1;

            if (!string.IsNullOrEmpty(postData.UniqueId))
            {
                LikeStateManager.SaveLikeState(postData.UniqueId, postData.isLiked, postData.likes);
            }

            UpdateUI();
        }

        private void OnCommentButtonClicked()
        {
            if (postData == null || commentPanel == null) return;

            StartCoroutine(UIAnimationManager.AnimateButtonClick(commentButton.transform));

            commentPanel.ShowComments(postData.content, postData.comments);
        }

        private void OnDestroy()
        {
            if (likeButton != null)
            {
                likeButton.onClick.RemoveAllListeners();
            }

            if (commentButton != null)
            {
                commentButton.onClick.RemoveAllListeners();
            }
        }
    }
} 