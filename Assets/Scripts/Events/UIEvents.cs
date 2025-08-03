using UnityEngine.Events;

public abstract class UIEvents {
    public static UnityAction onShowMainMenu;
    public static UnityAction onShowMainMenuPanel;
    public static UnityAction onShowGameUI;
    public static UnityAction onShowPauseMenu;
    public static UnityAction onShowEndGamePanel;
    public static UnityAction onShowPlayPanel;
    public static UnityAction onHideAllPanels;
    public static UnityAction onForceUIUpdate;

    public static UnityAction onButtonHover;
    public static UnityAction onButtonClick;
    public static UnityAction onMenuOpen;
    public static UnityAction onMenuClose;
}