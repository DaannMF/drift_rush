using UnityEngine;

[CreateAssetMenu(fileName = "Drift Car Settings", menuName = "DriftRush/Car Settings")]
public class Car_SO : ScriptableObject {
    [Header("Car Physics")]
    public float motorTorque = 8f;

    [Header("Steering")]
    public float normalSteerAngle = 90f;
    public float driftSteerAngle = 180f;
    public float maxWheelRotation = 25f;

    [Header("Physics")]
    public float gravityForce = 10f;
    public float dragOnGround = 3f;
    public float dragInAir = 0.1f;
}