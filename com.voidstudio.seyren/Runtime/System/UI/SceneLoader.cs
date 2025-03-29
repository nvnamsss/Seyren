using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Seyren.Utils;

namespace Seyren.System.UI
{
    /// <summary>
    /// A scene loader MonoBehaviour that can be attached to a GameObject to manage transitions between scenes.
    /// Features a loading screen with progress bar.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        // Singleton instance
        public static SceneLoader Instance { get; private set; }

        [Header("Loading Screen UI")]
        [SerializeField] private GameObject loadingScreenPanel;
        [SerializeField] private Slider progressBar;
        [SerializeField] private Text progressText;
        [SerializeField] private Text sceneNameText;
        
        [Header("Transition Settings")]
        [SerializeField] private float minimumLoadingTime = 0.5f; // Minimum time to show loading screen
        [SerializeField] private float fadeInTime = 0.5f;
        [SerializeField] private float fadeOutTime = 0.5f;
        
        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                
                // Hide loading screen at start
                if (loadingScreenPanel != null)
                    loadingScreenPanel.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        /// <summary>
        /// Load a scene with a loading screen
        /// </summary>
        /// <param name="sceneName">The name of the scene to load</param>
        public void LoadSceneWithLoadingScreen(string sceneName)
        {
            StartCoroutine(LoadSceneWithProgressRoutine(sceneName));
        }
        
        private IEnumerator LoadSceneWithProgressRoutine(string sceneName)
        {
            // Show loading screen
            loadingScreenPanel.SetActive(true);
            
            // Update scene name text
            if (sceneNameText != null)
                sceneNameText.text = $"Loading {sceneName}...";
            
            // Fade in the loading screen
            yield return StartCoroutine(FadeLoadingScreen(true, fadeInTime));
            
            // Track loading start time to ensure minimum display duration
            float startTime = Time.time;
            
            // Start the async loading operation
            yield return StartCoroutine(SceneUtils.LoadSceneAsync(
                sceneName,
                // Update progress bar and text
                (progress) => {
                    if (progressBar != null)
                        progressBar.value = progress;
                    if (progressText != null)
                        progressText.text = $"{Mathf.Round(progress * 100)}%";
                }
            ));
            
            // Ensure loading screen shows for at least the minimum time
            float elapsedTime = Time.time - startTime;
            if (elapsedTime < minimumLoadingTime)
            {
                yield return new WaitForSeconds(minimumLoadingTime - elapsedTime);
            }
            
            // Fade out the loading screen
            yield return StartCoroutine(FadeLoadingScreen(false, fadeOutTime));
            
            // Hide loading screen
            loadingScreenPanel.SetActive(false);
        }
        
        private IEnumerator FadeLoadingScreen(bool fadeIn, float duration)
        {
            // This is a simple implementation. For a proper fade, you would use a CanvasGroup
            // or modify the alpha of UI elements directly. This is a placeholder.
            float startTime = Time.time;
            while (Time.time - startTime < duration)
            {
                float t = (Time.time - startTime) / duration;
                if (!fadeIn) t = 1 - t;
                
                // Apply fade to UI elements here
                // For example, if using a CanvasGroup: canvasGroup.alpha = t;
                
                yield return null;
            }
        }
    }
}