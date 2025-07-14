using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
    private void OnThrottle(InputValue value) {
        CarEvents.onCarThrottleInput?.Invoke(value.Get<float>());
    }

    private void OnSteer(InputValue value) {
        CarEvents.onCarSteerInput?.Invoke(value.Get<float>());
    }

    private void OnDrift(InputValue value) {
        CarEvents.onCarDriftInput?.Invoke(value.Get<float>() > 0.5f);
    }

    private void OnRestart(InputValue _) {
        CarEvents.onResetCar?.Invoke();
    }

    private void OnPause(InputValue _) {
        LevelEvents.onGetIsInLevel?.Invoke(isInLevel => {
            if (isInLevel) {
                if (Time.timeScale == 0f) {
                    GameEvents.onResumeGame?.Invoke();
                }
                else {
                    GameEvents.onPauseGame?.Invoke();
                    UIEvents.onShowPauseMenu?.Invoke();
                }
            }
        });
    }
}