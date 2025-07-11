using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Seyren.Utils
{
    /// <summary>
    /// Utility class for managing scene operations in Unity.
    /// This class provides methods for loading, unloading, and transitioning between scenes.
    /// </summary>
    public static class SceneUtils
    {
        // Event that fires when a scene begins loading
        public static event Action<string> OnSceneLoadStarted;
        
        // Event that fires when a scene has finished loading
        public static event Action<string> OnSceneLoadComplete;
        
        /// <summary>
        /// Load a scene by name
        /// </summary>
        /// <param name="sceneName">The name of the scene to load</param>
        /// <param name="loadMode">The scene loading mode (Single, Additive)</param>
        public static void LoadScene(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            OnSceneLoadStarted?.Invoke(sceneName);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, loadMode);
            OnSceneLoadComplete?.Invoke(sceneName);
        }
        
        /// <summary>
        /// Load a scene asynchronously with an optional loading screen and progress callback
        /// </summary>
        /// <param name="sceneName">The name of the scene to load</param>
        /// <param name="onProgressUpdate">Optional callback that reports loading progress (0-1)</param>
        /// <param name="onComplete">Optional callback when loading is complete</param>
        /// <param name="loadMode">The scene loading mode (Single, Additive)</param>
        /// <returns>Coroutine handle for the async operation</returns>
        public static IEnumerator LoadSceneAsync(
            string sceneName, 
            Action<float> onProgressUpdate = null, 
            Action onComplete = null,
            LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            OnSceneLoadStarted?.Invoke(sceneName);
            
            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, loadMode);
            
            // Don't allow the scene to activate until we're ready
            asyncLoad.allowSceneActivation = false;
            
            while (!asyncLoad.isDone)
            {
                // Progress is reported as 0-0.9 until the very end
                float progress = asyncLoad.progress / 0.9f;
                onProgressUpdate?.Invoke(progress);
                
                // When loading is at 90%, activate the scene
                if (asyncLoad.progress >= 0.9f)
                {
                    asyncLoad.allowSceneActivation = true;
                }
                
                yield return null;
            }
            
            OnSceneLoadComplete?.Invoke(sceneName);
            onComplete?.Invoke();
        }
        
        /// <summary>
        /// Unload a scene by name
        /// </summary>
        /// <param name="sceneName">The name of the scene to unload</param>
        /// <returns>True if the scene was found and unloaded</returns>
        public static bool UnloadScene(string sceneName)
        {
            return UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName).isDone;
        }
        
        /// <summary>
        /// Get the currently active scene
        /// </summary>
        /// <returns>The active scene</returns>
        public static Scene GetActiveScene()
        {
            return UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        }
        
        /// <summary>
        /// Check if a scene is loaded
        /// </summary>
        /// <param name="sceneName">The name of the scene to check</param>
        /// <returns>True if the scene is loaded</returns>
        public static bool IsSceneLoaded(string sceneName)
        {
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
            {
                Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
                if (scene.name == sceneName && scene.isLoaded)
                {
                    return true;
                }
            }
            return false;
        }
    }
}