using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using SunBase.Data;

namespace SunBase.Controllers
{
    public class SocialFeedManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject postPrefab;
        [SerializeField] private Transform scrollViewContent;
        [SerializeField] private VerticalLayoutGroup layoutGroup;

        [Header("Configuration")]
        [SerializeField] private float spacingBetweenPosts = 10f;
        
        private List<PostData> posts;

        private void Start()
        {
            InitializeScrollView();
            LoadPostsFromJSON();
            GeneratePosts();
        }

        private void InitializeScrollView()
        {
            if (layoutGroup != null)
            {
                layoutGroup.spacing = spacingBetweenPosts;
                layoutGroup.childAlignment = TextAnchor.UpperCenter;
                layoutGroup.childForceExpandWidth = true;
                layoutGroup.childForceExpandHeight = false;
            }
        }

        private void LoadPostsFromJSON()
        {
            posts = JSONLoader.LoadPosts();
            
            if (posts.Count == 0)
            {
                Debug.LogWarning("No posts loaded from JSON. Using fallback data...");
                // Create a single fallback post if JSON loading fails
                posts = new List<PostData>
                {
                    new PostData
                    {
                        username = "System Message",
                        content = "Failed to load posts. Please check your data file.",
                        likes = 0,
                        isLiked = false,
                        timestamp = "Just now"
                    }
                };
            }
        }

        private void GeneratePosts()
        {
            foreach (var postData in posts)
            {
                GameObject postInstance = Instantiate(postPrefab, scrollViewContent);
                PostController postController = postInstance.GetComponent<PostController>();
                
                if (postController != null)
                {
                    postController.InitializePost(postData);
                }
            }
        }

        // Method to add a new post at runtime
        public void AddNewPost(PostData newPost)
        {
            posts.Insert(0, newPost); // Add to beginning of list
            GameObject postInstance = Instantiate(postPrefab, scrollViewContent);
            postInstance.transform.SetSiblingIndex(0); // Move to top of scroll view
            
            PostController postController = postInstance.GetComponent<PostController>();
            if (postController != null)
            {
                postController.InitializePost(newPost);
            }
        }

        // Method to refresh posts from JSON (can be called to reload data)
        public void RefreshPosts()
        {
            // Clear existing posts from the UI
            foreach (Transform child in scrollViewContent)
            {
                Destroy(child.gameObject);
            }

            // Reload and regenerate posts
            LoadPostsFromJSON();
            GeneratePosts();
        }
    }
} 