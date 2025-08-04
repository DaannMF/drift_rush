using UnityEngine.Events;
using System;

public abstract class SaveEvents {
    // Save game creation and management
    public static UnityAction<Action<Guid>> onCreateNewGame;
    public static UnityAction onSaveCurrentGame;
    public static UnityAction<Guid> onLoadGame;
    public static UnityAction onLoadLastGame;
    public static UnityAction<Guid> onDeleteGame;

    // Save data queries
    public static UnityAction<Action<PlayerLevelSaveData>> onGetCurrentGameData;
    public static UnityAction<Action<System.Collections.Generic.List<PlayerLevelSaveData>>> onGetAllSaveGames;
    public static UnityAction<Action<bool>> onHasSavedGames;

    // Scene integration
    public static UnityAction onApplySaveDataToScene;
}