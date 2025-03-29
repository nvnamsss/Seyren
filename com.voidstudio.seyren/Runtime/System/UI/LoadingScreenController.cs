using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Seyren.Utils;

namespace Seyren.System.UI
{
    /// <summary>
    /// Controller for the loading screen UI. Handles progress bar updates and animations.
    /// </summary>
    public class LoadingScreenController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Slider progressBar;
        [SerializeField] private Text progressText;
        [SerializeField] private Text tipText;
        [SerializeField] private Image backgroundImage;
        
        [Header("Loading Tips")]
        [SerializeField] private string[] loadingTips;
        
        [Header("Background Images")]
        [SerializeField] private Sprite[] backgroundImages;
        
        [Header("Animation Settings")]
        [SerializeField] private float tipChangeDuration = 5f;
        [SerializeField] private float tipFadeDuration = 0.5f;
        
        private float tipTimer;
        private int currentTipIndex;
        
        private void OnEnable()
        {
            // Reset progress bar
            if (progressBar != null)
                progressBar.value = 0;
                
            // Reset progress text
            if (progressText != null)
                progressText.text = "0%";
                
            // Select random tip and background
            SetRandomTip();
            SetRandomBackground();
            
            // Reset tip timer
            tipTimer = 0;
            
            // Subscribe to scene loading events
            SceneUtils.OnSceneLoadStarted += HandleSceneLoadStarted;
            SceneUtils.OnSceneLoadComplete += HandleSceneLoadComplete;
        }
        
        private void OnDisable()
        {
            // Unsubscribe from scene loading events
            SceneUtils.OnSceneLoadStarted -= HandleSceneLoadStarted;
            SceneUtils.OnSceneLoadComplete -= HandleSceneLoadComplete;
        }
        
        private void Update()
        {
            // Cycle through loading tips
            if (loadingTips.Length > 1 && tipText != null)
            {
                tipTimer += Time.deltaTime;
                if (tipTimer >= tipChangeDuration)
                {
                    StartCoroutine(FadeTipText());
                    tipTimer = 0;
                }
            }
        }
        
        private IEnumerator FadeTipText()
        {
            // Fade out
            float startTime = Time.time;
            while (Time.time - startTime < tipFadeDuration)
            {
                float t = (Time.time - startTime) / tipFadeDuration;
                tipText.color = new Color(tipText.color.r, tipText.color.g, tipText.color.b, 1 - t);
                yield return null;
            }
            
            // Change tip
            SetRandomTip();
            
            // Fade in
            startTime = Time.time;
            while (Time.time - startTime < tipFadeDuration)
            {
                float t = (Time.time - startTime) / tipFadeDuration;
                tipText.color = new Color(tipText.color.r, tipText.color.g, tipText.color.b, t);
                yield return null;
            }
        }
        
        private void SetRandomTip()
        {
            if (loadingTips.Length > 0 && tipText != null)
            {
                int newIndex;
                do
                {
                    newIndex = Random.Range(0, loadingTips.Length);
                } while (loadingTips.Length > 1 && newIndex == currentTipIndex);
                
                currentTipIndex = newIndex;
                tipText.text = loadingTips[currentTipIndex];
            }
        }
        
        private void SetRandomBackground()
        {
            if (backgroundImages.Length > 0 && backgroundImage != null)
            {
                backgroundImage.sprite = backgroundImages[Random.Range(0, backgroundImages.Length)];
            }
        }
        
        private void HandleSceneLoadStarted(string sceneName)
        {
            // You could add custom logic here when loading starts
            Debug.Log($"Started loading scene: {sceneName}");
        }
        
        private void HandleSceneLoadComplete(string sceneName)
        {
            // You could add custom logic here when loading completes
            Debug.Log($"Finished loading scene: {sceneName}");
        }
        
        // Public methods that can be called by the SceneLoader
        public void UpdateProgress(float progress)
        {
            if (progressBar != null)
                progressBar.value = progress;
                
            if (progressText != null)
                progressText.text = $"{Mathf.Round(progress * 100)}%";
        }
        
        public IEnumerator FadeIn(float duration)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0;
                float startTime = Time.time;
                while (Time.time - startTime < duration)
                {
                    float t = (Time.time - startTime) / duration;
                    canvasGroup.alpha = t;
                    yield return null;
                }
                canvasGroup.alpha = 1;
            }
        }
        
        public IEnumerator FadeOut(float duration)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1;
                float startTime = Time.time;
                while (Time.time - startTime < duration)
                {
                    float t = (Time.time - startTime) / duration;
                    canvasGroup.alpha = 1 - t;
                    yield return null;
                }
                canvasGroup.alpha = 0;
            }
        }
    }
}