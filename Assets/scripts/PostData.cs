using UnityEngine;
using System.Security.Cryptography;
using System.Text;

namespace SunBase.Data
{
    [System.Serializable]
    public class PostData
    {
        // Required fields
        public string username;
        public string profilePic;
        public string content;
        public int likes;
        public bool isLiked;

        // Optional fields for bonus features
        public string[] comments;  // Optional for comment system
        public string timestamp;   // Optional enhancement for post timing

        // Unique identifier for persistence
        private string _uniqueId;
        public string UniqueId
        {
            get
            {
                if (string.IsNullOrEmpty(_uniqueId))
                {
                    _uniqueId = GenerateUniqueId();
                }
                return _uniqueId;
            }
            set { _uniqueId = value; }
        }

        // Generate a unique ID based on username and content
        private string GenerateUniqueId()
        {
            string combined = username + content;
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(combined);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                
                return sb.ToString();
            }
        }
    }
} 