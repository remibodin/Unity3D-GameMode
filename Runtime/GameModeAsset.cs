using UnityEngine;

namespace Nrtx
{
    [CreateAssetMenu(fileName = "GameModeAsset", menuName = "GameMode profil", order = 0)]
    public class GameModeAsset : ScriptableObject 
    {
        /// <summary>
        /// Player prefab
        /// </summary>
        public GameObject player;

        /// <summary>
        /// Auto spawn player when scene start (GameMode Awake)
        /// if not, you must call GameMode.SpanwPlayer()
        /// </summary>
        public bool spawnPlayerOnAwake = true;

        public FadeOptions fadeIn;
        public FadeOptions fadeOut;


    }

    [System.Serializable]
    public class FadeOptions
    {
        public bool enable = true;
        public float time = 1.0f;
    }
}