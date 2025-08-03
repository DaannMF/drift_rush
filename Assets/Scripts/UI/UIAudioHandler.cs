using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIAudioHandler : MonoBehaviour, IPointerEnterHandler, ISelectHandler {
    [Header("Audio Settings")]
    [SerializeField] private bool playHoverSound = true;
    [SerializeField] private bool playClickSound = true;

    private Button button;

    private void Awake() {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (playHoverSound && button != null && button.interactable) {
            UIEvents.onButtonHover?.Invoke();
        }
    }

    public void OnButtonClick() {
        if (playClickSound && button != null && button.interactable) {
            UIEvents.onButtonClick?.Invoke();
        }
    }

    public void OnSelect(BaseEventData eventData) {
        if (playHoverSound && button != null && button.interactable) {
            UIEvents.onButtonHover?.Invoke();
        }
    }
}