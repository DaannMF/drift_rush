using UnityEngine.Events;

public abstract class CarEvents {
    public static UnityAction<float> onCarThrottleInput;
    public static UnityAction<float> onCarSteerInput;
    public static UnityAction<bool> onCarDriftInput;

    public static UnityAction onResetCar;

    public static UnityAction onDriftStarted;
    public static UnityAction onDriftEnded;

    public static UnityAction<float> onCarSpeedChanged;
}