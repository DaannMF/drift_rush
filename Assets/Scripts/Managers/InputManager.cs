using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputManager : MonoBehaviour {
    private Int16 throttle;
    private Int16 steer;
    private Int16 brake;

    public Int16 Throttle { get { return throttle; } }
    public Int16 Steer { get { return steer; } }
    public Int16 Brake { get { return brake; } }

    private void OnThrottle(InputValue value) {
        throttle = (Int16)value.Get<float>();
    }

    private void OnSteer(InputValue value) {
        steer = (Int16)value.Get<float>();
    }

    private void OnBrake(InputValue value) {
        brake = (Int16)value.Get<float>();
    }
}