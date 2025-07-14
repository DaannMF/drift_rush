using UnityEngine.Events;

/// <summary>
/// Events related to UI panels, menus, and user interface interactions
/// </summary>
public abstract class UIEvents {
    public static UnityAction onShowMainMenu;
    public static UnityAction onShowMainMenuPanel;
    public static UnityAction onShowLevelSelectionPanel;
    public static UnityAction onHideLevelSelectionPanel;
    public static UnityAction onShowGameUI;
    public static UnityAction onShowPauseMenu;
    public static UnityAction onShowEndGamePanel;
    public static UnityAction onHideEndGamePanel;
    public static UnityAction onHideAllPanels;
    public static UnityAction onForceUIUpdate;

    public static UnityAction onShowAudioSettings;
    public static UnityAction onHideAudioSettings;
    public static UnityAction onToggleAudioSettings;

    public static UnityAction onSetupMainMenuUI;
    public static UnityAction onSetupGameUI;

    public static UnityAction onButtonHover;
    public static UnityAction onButtonClick;
    public static UnityAction onMenuOpen;
    public static UnityAction onMenuClose;
}