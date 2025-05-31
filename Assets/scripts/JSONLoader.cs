using UnityEngine;
using System.Collections.Generic;

namespace SunBase.Data
{
    [System.Serializable]
    public class PostDataList
    {
        public List<PostData> posts;
    }

    public static class JSONLoader
    {
        private const string POST_DATA_FILE = "Data/posts";

        public static List<PostData> LoadPosts()
        {
            try
            {
                TextAsset jsonFile = Resources.Load<TextAsset>(POST_DATA_FILE);
                
                if (jsonFile == null)
                {
                    Debug.LogError($"Failed to load posts.json from Resources/{POST_DATA_FILE}");
                    return new List<PostData>();
                }

                PostDataList postDataList = JsonUtility.FromJson<PostDataList>(jsonFile.text);
                
                if (postDataList == null || postDataList.posts == null)
                {
                    Debug.LogError("Failed to parse posts.json or posts array is null");
                    return new List<PostData>();
                }

                Debug.Log($"Successfully loaded {postDataList.posts.Count} posts from JSON");
                return postDataList.posts;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error loading posts from JSON: {e.Message}");
                return new List<PostData>();
            }
        }
    }
} 