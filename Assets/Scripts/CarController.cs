using UnityEngine;

public class CarController : MonoBehaviour {
    [SerializeField] Transform centerofMass;
    [SerializeField] float motorTorque = 100f;
    [SerializeField] float maxSteer = 30f;

    public float Steer { get; set; }
    public float Throttle { get; set; }

    private Rigidbody _rigidbody;
    private Wheel[] _wheels;

    void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _wheels = GetComponentsInChildren<Wheel>();
    }

    void Start() {
        _rigidbody.centerOfMass = centerofMass.localPosition;
    }

    void Update() {
        HandleWheelInput();
    }

    private void HandleWheelInput() {
        Throttle = Input.GetAxis("Vertical") * -1;
        Steer = Input.GetAxis("Horizontal");

        foreach (var wheel in _wheels) {
            wheel.SteerAngle = Steer * maxSteer;
            wheel.MotorTorque = Throttle * motorTorque;
        }
    }
}

