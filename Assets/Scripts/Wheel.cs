using UnityEngine;

public class Wheel : MonoBehaviour {
    [SerializeField] bool steer;
    [SerializeField] bool invertSteer;
    [SerializeField] bool power;

    public float SteerAngle { get; set; }
    public float MotorTorque { get; set; }

    private WheelCollider _wheelCollider;
    private Transform _wheelTransform;

    void Awake() {
        _wheelCollider = GetComponent<WheelCollider>();
        _wheelTransform = GetComponentInChildren<MeshRenderer>().GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        UpdateWheelPose();
    }

    void FixedUpdate() {
        if (steer)
            _wheelCollider.steerAngle = SteerAngle * (invertSteer ? -1 : 1);

        if (power)
            _wheelCollider.motorTorque = MotorTorque;
    }

    private void UpdateWheelPose() {
        _wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        _wheelTransform.position = pos;
        _wheelTransform.rotation = rot;
    }
}
