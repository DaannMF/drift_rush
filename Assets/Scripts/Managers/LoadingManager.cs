using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    private static LoadingManager instance;
    public static LoadingManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("LoadingManager");
                instance = go.AddComponent<LoadingManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    [Header("Loading Settings")]
    [SerializeField] private bool useRealLoadingProgress = true;
    [SerializeField] private float minimumLoadingTime = 1f;

    private bool isLoading = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void LoadSceneWithLoading(string sceneName)
    {
        if (isLoading)
        {
            Debug.LogWarning("Already loading a scene!");
            return;
        }

        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    public void LoadSceneWithLoading(int sceneIndex)
    {
        if (isLoading)
        {
            Debug.LogWarning("Already loading a scene!");
            return;
        }

        StartCoroutine(LoadSceneCoroutine(sceneIndex));
    }

    public void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        LoadSceneWithLoading(currentSceneName);
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        isLoading = true;

        // Show loading screen
        FakeLoadingBar.Instance.ShowLoadingScreen();

        yield return new WaitForSecondsRealtime(0.1f); // Brief delay to ensure UI is visible

        if (useRealLoadingProgress)
        {
            // Load scene asynchronously
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            // Start loading with real progress
            bool loadingComplete = false;
            FakeLoadingBar.Instance.StartFakeLoadingWithRealProgress(asyncLoad, () =>
            {
                loadingComplete = true;
            });

            // Wait for loading to complete
            yield return new WaitUntil(() => loadingComplete);

            // Activate the scene
            asyncLoad.allowSceneActivation = true;

            // Wait for scene to actually load
            yield return new WaitUntil(() => asyncLoad.isDone);
        }
        else
        {
            // Use fake loading
            bool loadingComplete = false;
            FakeLoadingBar.Instance.StartFakeLoading(() =>
            {
                loadingComplete = true;
            });

            // Wait for fake loading to complete
            yield return new WaitUntil(() => loadingComplete);

            // Load scene normally
            SceneManager.LoadScene(sceneName);
        }

        // Brief delay before hiding loading screen
        yield return new WaitForSecondsRealtime(0.2f);

        // Hide loading screen
        FakeLoadingBar.Instance.HideLoadingScreen();

        // Apply loaded game data if we have any
        if (SaveGameManager.Instance.GetCurrentGameData() != null)
        {
            // Wait one frame for scene to initialize
            yield return null;
            SaveGameManager.Instance.ApplyGameDataToScene();
        }

        isLoading = false;

        Debug.Log($"Scene loaded: {sceneName}");
    }

    private IEnumerator LoadSceneCoroutine(int sceneIndex)
    {
        isLoading = true;

        // Show loading screen
        FakeLoadingBar.Instance.ShowLoadingScreen();

        yield return new WaitForSecondsRealtime(0.1f);

        if (useRealLoadingProgress)
        {
            // Load scene asynchronously
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
            asyncLoad.allowSceneActivation = false;

            // Start loading with real progress
            bool loadingComplete = false;
            FakeLoadingBar.Instance.StartFakeLoadingWithRealProgress(asyncLoad, () =>
            {
                loadingComplete = true;
            });

            // Wait for loading to complete
            yield return new WaitUntil(() => loadingComplete);

            // Activate the scene
            asyncLoad.allowSceneActivation = true;

            // Wait for scene to actually load
            yield return new WaitUntil(() => asyncLoad.isDone);
        }
        else
        {
            // Use fake loading
            bool loadingComplete = false;
            FakeLoadingBar.Instance.StartFakeLoading(() =>
            {
                loadingComplete = true;
            });

            // Wait for fake loading to complete
            yield return new WaitUntil(() => loadingComplete);

            // Load scene normally
            SceneManager.LoadScene(sceneIndex);
        }

        // Brief delay before hiding loading screen
        yield return new WaitForSecondsRealtime(0.2f);

        // Hide loading screen
        FakeLoadingBar.Instance.HideLoadingScreen();

        // Apply loaded game data if we have any
        if (SaveGameManager.Instance.GetCurrentGameData() != null)
        {
            // Wait one frame for scene to initialize
            yield return null;
            SaveGameManager.Instance.ApplyGameDataToScene();
        }

        isLoading = false;

        Debug.Log($"Scene loaded with index: {sceneIndex}");
    }

    public bool IsLoading()
    {
        return isLoading;
    }

    public void SetUseRealLoadingProgress(bool useReal)
    {
        useRealLoadingProgress = useReal;
    }
}