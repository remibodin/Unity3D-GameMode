using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour 
{
    public static GameMode Instance { get; private set; }

    /// <summary>
    /// fire when player spawn in scene
    /// </summary>
    public event System.Action<GameMode> onPlayerSpawn;

    [SerializeField] private GameModeAsset _gameMode;

    private List<GameObject> _spawnPoints = null;
    private GameObject _playerInstance = null;
    private GameObject _playerTagInstance = null;
    private float _playerSpawnTime = 0f;

    private Material _fadeMaterial;
    private Color _fadeColor = Color.clear;
    
    /// <summary>
    /// check if GameMode is valide
    /// </summary>
    /// <returns>True if all require reference are set</returns>
    public bool IsValide
    {
        get
        {
            return _gameMode != null && _gameMode.player != null;
        }
    }

    /// <summary>
    /// Object with Player tag or MainCamera tag in player prefab.
    /// Or root object spawned if no tag found
    /// </summary>
    /// <returns>GameObject player</returns>
    public GameObject Player
    {
        get
        {
            if (_playerTagInstance != null)
            {
                return _playerTagInstance;
            }
            return _playerInstance;
        }
    }

    /// <summary>
    /// Object spawn
    /// </summary>
    /// <returns>Object spawn</returns>
    public GameObject PlayerRoot
    {
        get
        {
            return _playerInstance;
        }
    }

    /// <summary>
    /// Check if player already spawn in scene
    /// </summary>
    /// <returns>Player is present in scene</returns>
    public bool PlayerIsPresent
    {
        get
        {
            return _playerInstance != null;
        }
    }

    /// <summary>
    /// Secondes since player spawn
    /// </summary>
    /// <returns>0 if player not present</returns>
    public float TimeSincePlayerSpawn
    {
        get
        {
            if (PlayerIsPresent == false)
            {
                return 0;
            }
            return Time.time - _playerSpawnTime;
        }
    }

    public float MasterVolume
    {
        get 
        {
            return AudioListener.volume;
        }

        set
        {
            AudioListener.volume = Mathf.Clamp01(value);
        }
    }

    /// <summary>
    /// Spawn player prefab in scene
    /// </summary>
    public void SpawnPlayer()
    {
        if (_playerInstance != null)
        {
            Debug.LogWarning("Player already present in scene");
            return;
        }
        GameObject spawnPoint = _spawnPoints[0];
        _playerInstance = GameObject.Instantiate(_gameMode.player, spawnPoint.transform.position, spawnPoint.transform.rotation);
        _playerTagInstance = FindPlayerTag(_playerInstance);
        if (_playerTagInstance == null)
        {
            _playerTagInstance = FindMainCameraTag(_playerInstance);
        }
        _playerSpawnTime = Time.time;
        if (onPlayerSpawn != null)
        {
            onPlayerSpawn(this);
        }
    }

    private bool _loading = false;
    public void LoadScene(string sceneName)
    {
        if (_loading == true)
        {
            return;
        }
        _loading = true;
        StartCoroutine(CO_LoadScene(sceneName));
    }

    private IEnumerator CO_LoadScene(string sceneName)
    {
        if (_gameMode.fadeOut.enable)
        {
            yield return CO_FadeOut(_gameMode.fadeOut);
        }
        yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        _loading = false;
    }

    private void SearchAllSpawnPointsInScene()
    {
        _spawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("Respawn"));
    }

    private static GameObject FindTagInChild(GameObject go, string tag)
    {
        if (go.transform.CompareTag(tag) == true)
        {
            return go;
        }
        int childCount = go.transform.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            Transform child = go.transform.GetChild(i);
            if (child.CompareTag(tag) == true)
            {
                return child.gameObject;
            }
            if (child.childCount > 0)
            {
                GameObject tagInChild = FindTagInChild(child.gameObject, tag);
                if (tagInChild != null)
                {
                    return tagInChild;
                }
            }
        }
        return null;
    }

    public static GameObject FindPlayerTag(GameObject go)
    {
        return FindTagInChild(go, "Player");
    }

    public static GameObject FindMainCameraTag(GameObject go)
    {
        return FindTagInChild(go, "MainCamera");
    }

    private GameObject CreateSpawnPoint(Vector3 position, Quaternion rotation)
    {
        GameObject spanwPoint = new GameObject("PlayerStart");
        Transform spanwPointTransform = spanwPoint.transform;
        spanwPointTransform.position = position;
        spanwPointTransform.rotation = rotation;
        spanwPoint.tag = "Respawn";
        return spanwPoint;
    }

    private void Awake() 
    {
        // au cas ou il y a un fadeout dans la scene precedente
        // et pas de fadein dan la nouvelle
        MasterVolume = 1;

        Instance = this;
        if (IsValide == false)
        {
            Debug.LogError("GameMode is invalide. Assigne a correct GameMode profil", this);
            return;
        }
        SearchAllSpawnPointsInScene();
        if (_spawnPoints == null)
        {
            _spawnPoints = new List<GameObject>();
        }
        if (_spawnPoints.Count <= 0)
        {
            _spawnPoints.Add(CreateSpawnPoint(Vector3.zero, Quaternion.identity));
            Debug.LogWarning("Objects with tag Respawn not found. One SpawnPoint auto-created");
        }
        if (_gameMode.spawnPlayerOnSceneLoad)
        {
            SpawnPlayer();
        }

        Shader fadeShader = Shader.Find("Hidden/Apperture/Fade");
        _fadeMaterial = new Material(fadeShader);
        CreateCameraComponent();
        if (_gameMode.fadeIn.enable)
        {
            StartCoroutine(CO_FadeIn(_gameMode.fadeIn));
        }
    }

    private IEnumerator CO_FadeIn(FadeOptions fadeOptions)
    {
        float t = 0;
        if (fadeOptions.globalScreen)
        {
            _fadeColor = Color.black;
            _fadeColor.a = 1;
        }
        if (fadeOptions.globalSound)
        {
            MasterVolume = 0;
        }
        while(t <= 1)
        {
            if (fadeOptions.globalScreen)
            {
                _fadeColor.a = Mathf.Lerp(1f, 0f, t);    
            }
            if (fadeOptions.globalSound)
            {
                MasterVolume = Mathf.Lerp(0f, 1f, t);
            }
            t += Time.deltaTime / fadeOptions.time;
            yield return null;
        }
        if (fadeOptions.globalScreen)
        {
            _fadeColor.a = 0;
        }
        if (fadeOptions.globalSound)
        {
            MasterVolume = 1;
        }
    }

    private IEnumerator CO_FadeOut(FadeOptions fadeOptions)
    {
        float t = 0;
        if (fadeOptions.globalScreen)
        {
            _fadeColor = Color.black;
            _fadeColor.a = 0;
        }
        if (fadeOptions.globalSound)
        {
            MasterVolume = 1;
        }
        while(t <= 1)
        {
            if (fadeOptions.globalScreen)
            {
                _fadeColor.a = Mathf.Lerp(0f, 1f, t);    
            }
            if (fadeOptions.globalSound)
            {
                MasterVolume = Mathf.Lerp(1f, 0f, t);
            }
            t += Time.deltaTime / fadeOptions.time;
            yield return null;
        }
        if (fadeOptions.globalScreen)
        {
            _fadeColor.a = 1;
        }
        if (fadeOptions.globalSound)
        {
            MasterVolume = 0;
        }
    }

    private void CreateCameraComponent()
    {
        Camera camera = gameObject.AddComponent<Camera>();
        camera.clearFlags = CameraClearFlags.Nothing;
        camera.cullingMask = 0;
        camera.nearClipPlane = 0.01f;
        camera.farClipPlane = 0.02f;
        camera.depth = 100;
        camera.useOcclusionCulling = false;
        camera.allowMSAA = false;
        camera.allowHDR = false;
        camera.hideFlags = HideFlags.HideInInspector;
    }

    private void OnPostRender()
    {
        if (_fadeColor.a <= 0)
        {
            return;
        }
        _fadeMaterial.SetColor("_Color", _fadeColor);
        _fadeMaterial.SetPass(0);
        GL.Begin(GL.QUADS);
        GL.Vertex3(-1, -1, 0);
        GL.Vertex3( 1, -1, 0);
        GL.Vertex3(1, 1, 0);
        GL.Vertex3(-1, 1, 0);
        GL.End(); 
    }
}
