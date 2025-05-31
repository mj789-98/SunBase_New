using UnityEngine;

namespace SunBase.Data
{
    public static class LikeStateManager
    {
        private const string LIKE_STATE_PREFIX = "post_like_";
        private const string LIKE_COUNT_PREFIX = "post_count_";

        public static void SaveLikeState(string postId, bool isLiked, int likeCount)
        {
            PlayerPrefs.SetInt(LIKE_STATE_PREFIX + postId, isLiked ? 1 : 0);
            PlayerPrefs.SetInt(LIKE_COUNT_PREFIX + postId, likeCount);
            PlayerPrefs.Save();
        }

        public static bool GetLikeState(string postId, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(LIKE_STATE_PREFIX + postId, defaultValue ? 1 : 0) == 1;
        }

        public static int GetLikeCount(string postId, int defaultCount)
        {
            return PlayerPrefs.GetInt(LIKE_COUNT_PREFIX + postId, defaultCount);
        }

        public static void ClearAllLikeStates()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        public static void ResetPost(string postId)
        {
            PlayerPrefs.DeleteKey(LIKE_STATE_PREFIX + postId);
            PlayerPrefs.DeleteKey(LIKE_COUNT_PREFIX + postId);
            PlayerPrefs.Save();
        }
    }
} 