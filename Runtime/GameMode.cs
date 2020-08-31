using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nrtx
{
    public class GameMode : MonoBehaviour
    {
        [SerializeField] private GameModeAsset _gameMode;
        private readonly List<GameObject> _spawnPointCollection = new List<GameObject>();
        private Material _fadeMaterial;
        private Color _fadeColor;
        private Coroutine _fadeCoroutine = null;
        private float _fadeValue = 0;

        private static GameMode _instance;

        #region MonoBehaviour

        private void Awake()
        {
            _instance = this;
            
            // Add a camera component on game object to active OnPostRender callback
            CreateCameraComponent();

            _fadeMaterial = new Material(Shader.Find("Hidden/Nrtx/Fade"));
            _fadeColor = Color.clear;

            SearchAllSpawnPointsInScene();
            if (_gameMode != null && _gameMode.spawnPlayerOnAwake)
            {
                SpawnPlayerInternal();
            }

            if (_gameMode != null && _gameMode.fadeIn.enable)
            {
                FadeIn(_gameMode.fadeIn.time);
            }
        }

        private void FadeIn(float time)
        {
            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = StartCoroutine(FadeCoroutine(time, 0f));
        }

        private void FadeOut(float time)
        {
            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = StartCoroutine(FadeCoroutine(time, 1f));
        }

        private IEnumerator FadeCoroutine(float time, float endValue)
        {
            if (time <= 0)
            {
                _fadeValue = endValue;
                yield break;
            }
            
            float speed = 1f / time;
            yield return null;
        }

        private void OnPostRender()
        {
            _fadeMaterial.SetColor("_Color", _fadeColor);
            _fadeMaterial.SetPass(0);
            GL.Begin(GL.QUADS);
            GL.Vertex3(-1, -1, 0);
            GL.Vertex3( 1, -1, 0);
            GL.Vertex3(1, 1, 0);
            GL.Vertex3(-1, 1, 0);
            GL.End(); 
        }

        #endregion

        private static void CheckInstance()
        {
            if (_instance == null)
            {
                throw new System.Exception("GameMode instance is null");
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
            camera.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
        }

        private void SearchAllSpawnPointsInScene()
        {
            var sceneSpawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
            if (sceneSpawnPoints.Length > 0)
            {
                _spawnPointCollection.AddRange(sceneSpawnPoints);
            }
        }

        private void SpawnPlayerInternal()
        {
            if (_gameMode == null || _gameMode.player == null)
            {
                Debug.LogError("Player is not set in game mode or game mode asset is null");
                return;
            }

            if (_spawnPointCollection.Count <= 0)
            {
                CreateSpawnPoint(Vector3.zero);
                Debug.LogWarning("Objects with tag Respawn not found. One SpawnPoint auto-created");
            }

            GameObject spawnPoint = _spawnPointCollection[0];
            GameObject playerInstance = Instantiate(_gameMode.player, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }

        private GameObject CreateSpawnPointInternal(Vector3 position, Quaternion rotation)
        {
            GameObject spawnPoint = new GameObject("SpawnPoint");
            spawnPoint.transform.position = position;
            spawnPoint.transform.rotation = rotation;
            spawnPoint.tag = "Respawn";
            _spawnPointCollection.Add(spawnPoint);
            return spawnPoint;
        }

        #region Public static methods

        public static void SpawnPlayer()
        {
            CheckInstance();
            _instance.SpawnPlayerInternal();
        }

        public static GameObject CreateSpawnPoint(Vector3 position)
        {
            return CreateSpawnPoint(position, Quaternion.identity);
        }

        public static GameObject CreateSpawnPoint(Vector3 position, Quaternion rotation)
        {
            CheckInstance();
            return _instance.CreateSpawnPointInternal(position, rotation);
        }

        public static GameObject GetClosestSpawnPoint(Vector3 position)
        {
            CheckInstance();
            float closestDistance = float.MaxValue;
            GameObject closestObject = null;
            foreach (var spawn in _instance._spawnPointCollection)
            {
                var distance = Vector3.Distance(position, spawn.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = spawn;
                }
            }
            return closestObject;
        }

        #endregion
    }
}