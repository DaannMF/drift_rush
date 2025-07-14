using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Automatically adds UIAudioHandler to all buttons in the scene
/// </summary>
public class AutoUIAudioSetup : MonoBehaviour {
    [SerializeField] private bool includeInactive = true;

    private void Awake() {
        SetupUIAudio();
    }

    private void SetupUIAudio() {
        foreach (Button button in GetComponentsInChildren<Button>(includeInactive))
            if (button.GetComponent<UIAudioHandler>() == null)
                button.gameObject.AddComponent<UIAudioHandler>();

    }
}