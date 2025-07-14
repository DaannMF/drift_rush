using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputManager : MonoBehaviour {
    private Int16 throttle;
    private Int16 steer;
    private bool drift;

    public Int16 Throttle { get { return throttle; } }
    public Int16 Steer { get { return steer; } }
    public bool Drift { get { return drift; } }

    private void OnThrottle(InputValue value) {
        throttle = (Int16)value.Get<float>();
        CarEvents.onCarThrottleInput?.Invoke(throttle);
    }

    private void OnSteer(InputValue value) {
        steer = (Int16)value.Get<float>();
        CarEvents.onCarSteerInput?.Invoke(steer);
    }

    private void OnDrift(InputValue value) {
        drift = value.Get<float>() > 0.5f;
        CarEvents.onCarDriftInput?.Invoke(drift);
    }

    private void OnRestart(InputValue _) {
        CarEvents.onResetCar?.Invoke();
    }
}