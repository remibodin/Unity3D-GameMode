using UnityEngine;

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
    public bool spawnPlayerOnSceneLoad = true;
}
