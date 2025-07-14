using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIAudioHandler : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler {
    [Header("Audio Settings")]
    [SerializeField] private bool playHoverSound = true;
    [SerializeField] private bool playClickSound = true;

    private Button button;

    private void Awake() {
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (playHoverSound && button != null && button.interactable) {
            UIEvents.onButtonHover?.Invoke();
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (playClickSound && button != null && button.interactable) {
            UIEvents.onButtonClick?.Invoke();
        }
    }
}