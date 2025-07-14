using UnityEngine.Events;
using System.Collections.Generic;

public abstract class LevelEvents {
    public static UnityAction onRestartLevel;

    public static UnityAction onLevelLoadStarted;
    public static UnityAction onLevelLoadCompleted;
    public static UnityAction<float> onLevelLoadProgress;

    public static UnityAction<int> onLoadLevel;
    public static UnityAction onLoadNextLevel;
    public static UnityAction onLoadMainMenu;

    public static UnityAction<System.Action<int>> onGetCurrentLevelIndex;
    public static UnityAction<System.Action<bool>> onGetIsLoading;
    public static UnityAction<System.Action<bool>> onGetIsInMainMenu;
    public static UnityAction<System.Action<bool>> onGetIsInLevel;
    public static UnityAction<System.Action<int>> onGetTotalLevels;
    public static UnityAction<System.Action<List<LevelData>>> onGetLevelData;
}