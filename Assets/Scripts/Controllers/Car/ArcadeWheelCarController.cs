using UnityEngine;

public class ArcadeCarController : MonoBehaviour {
    [SerializeField] private Car_SO data;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.5f;
    [SerializeField] private Transform leftFrontWheel;
    [SerializeField] private Transform rightFrontWheel;

    private InputManager _input;
    private bool _isGrounded = true;

    private void Awake() {
        if (_input == null) _input = FindObjectOfType<InputManager>();
    }

    void Start() {
        rb.transform.parent = null;
    }

    void Update() {
        UpdatePositionAndRotation();
        RotateWheels();
    }

    private void FixedUpdate() {
        CheckIsGroundedAndRotate();

        if (_isGrounded)
            ApplyForceToRigidbody();
        else
            ApplyGravity();
    }

    private void UpdatePositionAndRotation() {
        Vector3 steerVector = new(0f, _input.Steer * data.maxSteerAngle * Time.deltaTime * _input.Throttle, 0f);
        transform.SetPositionAndRotation(rb.transform.position, Quaternion.Euler(transform.rotation.eulerAngles + steerVector));
    }

    private void RotateWheels() {
        float inputAngle = _input.Steer * data.maxWheelRotation;
        var leftWheelRotation = leftFrontWheel.localRotation.eulerAngles;
        var rightWheelRotation = rightFrontWheel.localRotation.eulerAngles;
        leftFrontWheel.localRotation = Quaternion.Euler(leftWheelRotation.x, inputAngle, leftWheelRotation.z);
        rightFrontWheel.localRotation = Quaternion.Euler(rightWheelRotation.x, inputAngle - 180, rightWheelRotation.z);
    }

    private void ApplyForceToRigidbody() {
        if (Mathf.Abs(_input.Throttle) > 0) {
            rb.drag = data.dragOnGround;
            rb.AddForce(_input.Throttle * data.motorTorque * transform.forward * 500f);
        }
    }

    private void ApplyGravity() {
        rb.drag = data.dragInAir;
        rb.AddForce(-data.gravityForce * transform.up);
    }

    private void CheckIsGroundedAndRotate() {
        _isGrounded = Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, groundCheckDistance, groundLayer);
        if (_isGrounded)
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
    }
}