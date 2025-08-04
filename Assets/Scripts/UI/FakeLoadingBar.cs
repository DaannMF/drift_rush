using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class FakeLoadingBar : MonoBehaviour
{
    [Header("Loading Bar References")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI loadingMessageText;
    [SerializeField] private GameObject loadingPanel;

    [Header("Loading Settings")]
    [SerializeField] private float minLoadingTime = 2f;
    [SerializeField] private float maxLoadingTime = 4f;
    [SerializeField] private AnimationCurve loadingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Loading Messages")]
    [SerializeField]
    private string[] loadingMessages = {
        "Loading world...",
        "Preparing environment...",
        "Starting engines...",
        "Loading game data...",
        "Almost ready..."
    };

    private void Awake()
    {
        InitializeLoadingBar();
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        LevelEvents.onLevelLoadStarted += ShowLoadingScreen;
        LevelEvents.onLevelLoadCompleted += HideLoadingScreen;
        LevelEvents.onLevelLoadProgress += UpdateProgressFromLevel;
    }

    private void UnsubscribeFromEvents()
    {
        LevelEvents.onLevelLoadStarted -= ShowLoadingScreen;
        LevelEvents.onLevelLoadCompleted -= HideLoadingScreen;
        LevelEvents.onLevelLoadProgress -= UpdateProgressFromLevel;
    }

    private void InitializeLoadingBar()
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }

        if (progressBar != null)
        {
            progressBar.value = 0f;
        }

        if (progressText != null)
        {
            progressText.text = "0%";
        }
    }

    public void ShowLoadingScreen()
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(true);
        }

        ResetProgressBar();
    }

    public void HideLoadingScreen()
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }
    }

    private void UpdateProgressFromLevel(float progress)
    {
        UpdateProgressBar(progress);
    }

    public void StartFakeLoading(System.Action onComplete = null)
    {
        StartCoroutine(FakeLoadingCoroutine(onComplete));
    }

    public void StartFakeLoadingWithRealProgress(AsyncOperation asyncOperation, System.Action onComplete = null)
    {
        StartCoroutine(LoadingWithRealProgressCoroutine(asyncOperation, onComplete));
    }

    private IEnumerator FakeLoadingCoroutine(System.Action onComplete)
    {
        float loadingTime = Random.Range(minLoadingTime, maxLoadingTime);
        float elapsedTime = 0f;

        // Show random loading messages
        int currentMessageIndex = 0;
        float messageChangeInterval = loadingTime / loadingMessages.Length;
        float nextMessageTime = messageChangeInterval;

        while (elapsedTime < loadingTime)
        {
            elapsedTime += Time.unscaledDeltaTime;

            // Update progress using curve
            float normalizedTime = elapsedTime / loadingTime;
            float progress = loadingCurve.Evaluate(normalizedTime);

            UpdateProgressBar(progress);

            // Change loading message
            if (elapsedTime >= nextMessageTime && currentMessageIndex < loadingMessages.Length)
            {
                UpdateLoadingMessage(loadingMessages[currentMessageIndex]);
                currentMessageIndex++;
                nextMessageTime += messageChangeInterval;
            }

            yield return null;
        }

        // Ensure we reach 100%
        UpdateProgressBar(1f);
        UpdateLoadingMessage("Complete!");

        yield return new WaitForSecondsRealtime(0.5f); // Brief pause at 100%

        onComplete?.Invoke();
    }

    private IEnumerator LoadingWithRealProgressCoroutine(AsyncOperation asyncOperation, System.Action onComplete)
    {
        float fakeProgress = 0f;
        float realProgress = 0f;

        int currentMessageIndex = 0;
        float messageTimer = 0f;
        float messageChangeInterval = 1.5f;

        while (!asyncOperation.isDone || fakeProgress < 1f)
        {
            // Get real loading progress (clamped to 0.9 until done)
            realProgress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            // Smoothly interpolate fake progress toward real progress
            fakeProgress = Mathf.MoveTowards(fakeProgress, realProgress, Time.unscaledDeltaTime * 0.5f);

            // If real loading is done but fake isn't at 100%, speed up fake progress
            if (asyncOperation.isDone && fakeProgress < 1f)
            {
                fakeProgress = Mathf.MoveTowards(fakeProgress, 1f, Time.unscaledDeltaTime * 2f);
            }

            UpdateProgressBar(fakeProgress);

            // Update loading messages
            messageTimer += Time.unscaledDeltaTime;
            if (messageTimer >= messageChangeInterval && currentMessageIndex < loadingMessages.Length)
            {
                UpdateLoadingMessage(loadingMessages[currentMessageIndex]);
                currentMessageIndex = (currentMessageIndex + 1) % loadingMessages.Length;
                messageTimer = 0f;
            }

            yield return null;
        }

        // Ensure we reach 100%
        UpdateProgressBar(1f);
        UpdateLoadingMessage("Complete!");

        yield return new WaitForSecondsRealtime(0.3f);

        onComplete?.Invoke();
    }

    private void UpdateProgressBar(float progress)
    {
        progress = Mathf.Clamp01(progress);

        if (progressBar != null)
        {
            progressBar.value = progress;
        }

        if (progressText != null)
        {
            progressText.text = $"{Mathf.RoundToInt(progress * 100)}%";
        }
    }

    private void UpdateLoadingMessage(string message)
    {
        if (loadingMessageText != null)
        {
            loadingMessageText.text = message;
        }
    }

    private void ResetProgressBar()
    {
        UpdateProgressBar(0f);
        if (loadingMessages.Length > 0)
        {
            UpdateLoadingMessage(loadingMessages[0]);
        }
    }

    public bool IsLoading()
    {
        return loadingPanel != null && loadingPanel.activeInHierarchy;
    }
}