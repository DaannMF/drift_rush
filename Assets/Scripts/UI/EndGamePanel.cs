using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePanel : MonoBehaviour {
    [Header("UI Components")]
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private GameObject winTitle;
    [SerializeField] private GameObject loseTitle;

    [Header("Transition Settings")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float transitionDuration = 1f;
    [SerializeField] private float victoryDelayBeforeShow = 2f;
    [SerializeField] private float defeatDelayBeforeShow = 1.5f;

    [Header("Animation Settings")]
    [SerializeField] private Transform panelTransform;
    [SerializeField] private float scaleAnimationDuration = 0.5f;
    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private bool isGameWon;
    private bool hasShownTransition = false;

    void Awake() {
        InitializeComponents();
        SetupButtonListeners();
    }

    void OnEnable() {
        hasShownTransition = false;
        GameEvents.onGetIsGameWon?.Invoke(OnGameWonReceived);
    }

    private void InitializeComponents() {
        // Ensure we have a CanvasGroup for fade transitions
        if (canvasGroup == null) {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null) {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        // Use panel transform or this transform for scale animations
        if (panelTransform == null) {
            panelTransform = transform;
        }

        // Start invisible and scaled down
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        panelTransform.localScale = Vector3.zero;
    }

    private void SetupButtonListeners() {
        if (playAgainButton != null) {
            playAgainButton.onClick.AddListener(OnPlayAgainButtonClicked);
        }
        if (nextLevelButton != null) {
            nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
        }
        if (mainMenuButton != null) {
            mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        }
        if (exitButton != null) {
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }
    }

    private void OnGameWonReceived(bool isGameWon) {
        this.isGameWon = isGameWon;

        if (isGameWon) {
            winTitle.SetActive(true);
            loseTitle.SetActive(false);

            if (nextLevelButton != null) {
                LevelEvents.onGetCurrentLevelIndex?.Invoke(currentLevel => {
                    LevelEvents.onGetTotalLevels?.Invoke(totalLevels => {
                        bool hasNextLevel = currentLevel + 1 < totalLevels;
                        nextLevelButton.gameObject.SetActive(hasNextLevel);
                    });
                });
            }
        }
        else {
            winTitle.SetActive(false);
            loseTitle.SetActive(true);

            if (nextLevelButton != null) {
                nextLevelButton.gameObject.SetActive(false);
            }
        }

        // Start the transition sequence with appropriate delay
        if (!hasShownTransition) {
            hasShownTransition = true;
            float delay = isGameWon ? victoryDelayBeforeShow : defeatDelayBeforeShow;
            StartCoroutine(ShowPanelWithDelay(delay));
        }
    }

    private IEnumerator ShowPanelWithDelay(float delay) {
        // Wait for the specified delay
        yield return new WaitForSecondsRealtime(delay);

        // Start both fade and scale animations simultaneously
        StartCoroutine(FadeIn());
        StartCoroutine(ScaleIn());
    }

    private IEnumerator FadeIn() {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration) {
            elapsedTime += Time.unscaledDeltaTime;
            float progress = elapsedTime / transitionDuration;

            canvasGroup.alpha = Mathf.Lerp(0f, 1f, progress);

            yield return null;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private IEnumerator ScaleIn() {
        float elapsedTime = 0f;

        while (elapsedTime < scaleAnimationDuration) {
            elapsedTime += Time.unscaledDeltaTime;
            float progress = elapsedTime / scaleAnimationDuration;
            float scaleValue = scaleCurve.Evaluate(progress);

            panelTransform.localScale = Vector3.one * scaleValue;

            yield return null;
        }

        panelTransform.localScale = Vector3.one;
    }

    private IEnumerator FadeOut() {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration) {
            elapsedTime += Time.unscaledDeltaTime;
            float progress = elapsedTime / transitionDuration;

            canvasGroup.alpha = Mathf.Lerp(1f, 0f, progress);

            yield return null;
        }

        canvasGroup.alpha = 0f;
    }

    private void OnPlayAgainButtonClicked() {
        StartCoroutine(FadeOutAndRestart());
    }

    private void OnNextLevelButtonClicked() {
        StartCoroutine(FadeOutAndLoadNextLevel());
    }

    private void OnMainMenuButtonClicked() {
        StartCoroutine(FadeOutAndLoadMainMenu());
    }

    private IEnumerator FadeOutAndRestart() {
        yield return StartCoroutine(FadeOut());
        LevelEvents.onRestartLevel?.Invoke();
    }

    private IEnumerator FadeOutAndLoadNextLevel() {
        yield return StartCoroutine(FadeOut());

        // Use the new auto-save method that saves AFTER loading the next level
        if (isGameWon) {
            Debug.Log("Victory: Loading next level with auto-save...");
            LevelEvents.onLoadNextLevelWithAutoSave?.Invoke();
        }
        else {
            LevelEvents.onLoadNextLevel?.Invoke();
        }
    }

    private IEnumerator FadeOutAndLoadMainMenu() {
        yield return StartCoroutine(FadeOut());
        LevelEvents.onLoadMainMenu?.Invoke();
    }

    private void OnExitButtonClicked() {
        StartCoroutine(FadeOutAndExit());
    }

    private IEnumerator FadeOutAndExit() {
        yield return StartCoroutine(FadeOut());
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        Application.Quit();
#endif
    }
}
