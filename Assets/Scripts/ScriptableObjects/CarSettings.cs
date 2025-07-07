using UnityEngine;

[CreateAssetMenu(fileName = "Drift Car Settings", menuName = "DriftRush/Car Settings")]
public class Car_SO : ScriptableObject {
    [Header("Car Physics")]
    public float motorTorque = 8f;
    public float maxSteerAngle = 180f;
    public float maxWheelRotation = 25f;
    public float gravityForce = 10f;
    public float dragOnGround = 3f;
    public float dragInAir = 0.1f;
}