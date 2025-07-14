using UnityEngine;

public class ArcadeCarController : MonoBehaviour {
    [SerializeField] private Car_SO data;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.5f;
    [SerializeField] private Transform leftFrontWheel;
    [SerializeField] private Transform rightFrontWheel;
    [SerializeField] private float upRightForce = 300f;
    [SerializeField] private float uprightTorque = 3000f;
    [SerializeField] private float stuckDetectionTime = 3f;
    [SerializeField] private float minVelocityThreshold = 0.1f;
    [SerializeField] private float maxVerticalVelocity = 25f;

    private float currentThrottle = 0f;
    private float currentSteer = 0f;
    private bool currentDrift = false;

    private bool _isGrounded = true;
    private Vector3[] raycastPoints;
    private float stuckTimer = 0f;
    private Vector3 lastPosition;
    private bool isDrifting = false;
    private bool isIdle = false;
    private bool isAccelerating = false;
    private bool isBraking = false;

    private void Awake() {
        SubscribeToEvents();
    }

    void Start() {
        rb.transform.parent = null;
        InitializeRaycastPoints();
        lastPosition = transform.position;
    }

    void Update() {
        UpdatePositionAndRotation();
        RotateWheels();
        UpdateSpeed();
    }

    private void FixedUpdate() {
        if (Time.timeScale == 0f) return;

        CheckIsGroundedAndRotate();
        CheckIfStuck();

        if (_isGrounded)
            ApplyForceToRigidbody();
        else
            ApplyGravity();

        if (Mathf.Abs(rb.velocity.y) > maxVerticalVelocity) {
            rb.velocity = new Vector3(rb.velocity.x,
                                     Mathf.Sign(rb.velocity.y) * maxVerticalVelocity,
                                     rb.velocity.z);
        }
    }

    private void OnDestroy() {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents() {
        CarEvents.onCarThrottleInput += OnThrottleInput;
        CarEvents.onCarSteerInput += OnSteerInput;
        CarEvents.onCarDriftInput += OnDriftInput;
        CarEvents.onResetCar += OnResetCar;
    }

    private void UnsubscribeFromEvents() {
        CarEvents.onCarThrottleInput -= OnThrottleInput;
        CarEvents.onCarSteerInput -= OnSteerInput;
        CarEvents.onCarDriftInput -= OnDriftInput;
        CarEvents.onResetCar -= OnResetCar;
    }

    private void OnThrottleInput(float throttle) {
        currentThrottle = throttle;
    }

    private void OnSteerInput(float steer) {
        currentSteer = steer;
    }

    private void OnDriftInput(bool drift) {
        currentDrift = drift;
    }

    private void InitializeRaycastPoints() {
        raycastPoints = new Vector3[4];
        Bounds bounds = GetComponentInChildren<Collider>().bounds;
        float xOffset = bounds.size.x * 0.4f;
        float zOffset = bounds.size.z * 0.4f;

        raycastPoints[0] = new Vector3(xOffset, 0, zOffset);
        raycastPoints[1] = new Vector3(-xOffset, 0, zOffset);
        raycastPoints[2] = new Vector3(xOffset, 0, -zOffset);
        raycastPoints[3] = new Vector3(-xOffset, 0, -zOffset);
    }

    private void UpdatePositionAndRotation() {
        if (Time.timeScale == 0f) return;

        float currentSteerAngle = currentDrift ? data.driftSteerAngle : data.normalSteerAngle;

        Vector3 steerVector = new(0f, currentSteer * currentSteerAngle * Time.deltaTime * currentThrottle, 0f);
        transform.SetPositionAndRotation(rb.transform.position, Quaternion.Euler(transform.rotation.eulerAngles + steerVector));
    }

    private void RotateWheels() {
        float inputAngle = currentSteer * data.maxWheelRotation;
        var leftWheelRotation = leftFrontWheel.localRotation.eulerAngles;
        var rightWheelRotation = rightFrontWheel.localRotation.eulerAngles;
        leftFrontWheel.localRotation = Quaternion.Euler(leftWheelRotation.x, inputAngle, leftWheelRotation.z);
        rightFrontWheel.localRotation = Quaternion.Euler(rightWheelRotation.x, inputAngle - 180, rightWheelRotation.z);
    }

    private void ApplyForceToRigidbody() {
        if (Mathf.Abs(currentThrottle) > 0) {
            rb.drag = data.dragOnGround;
            rb.AddForce(currentThrottle * data.motorTorque * transform.forward * 500f);

            if (isIdle) {
                isIdle = false;
                AudioEvents.onCarIdleStop?.Invoke();
            }

            if (currentThrottle > 0) {
                if (!isAccelerating) {
                    isAccelerating = true;
                    if (isBraking) {
                        isBraking = false;
                        AudioEvents.onCarBrakeStop?.Invoke();
                    }
                    AudioEvents.onCarAccelerate?.Invoke();
                }
            }
            else {
                if (!isBraking) {
                    isBraking = true;
                    if (isAccelerating) {
                        isAccelerating = false;
                        AudioEvents.onCarAccelerateStop?.Invoke();
                    }
                    AudioEvents.onCarBrake?.Invoke();
                }
            }
        }
        else {
            if (isAccelerating) {
                isAccelerating = false;
                AudioEvents.onCarAccelerateStop?.Invoke();
            }
            if (isBraking) {
                isBraking = false;
                AudioEvents.onCarBrakeStop?.Invoke();
            }

            if (!isIdle) {
                isIdle = true;
                AudioEvents.onCarIdle?.Invoke();
            }
        }

        CheckDriftSimple();
    }

    private void CheckDriftSimple() {
        bool shouldDrift = currentDrift && currentThrottle > 0;

        if (shouldDrift && !isDrifting) {
            isDrifting = true;
            AudioEvents.onCarDrift?.Invoke();
            CarEvents.onDriftStarted?.Invoke();
        }
        else if (!shouldDrift && isDrifting) {
            isDrifting = false;
            AudioEvents.onCarDriftStop?.Invoke();
            CarEvents.onDriftEnded?.Invoke();
        }
    }

    private void ApplyGravity() {
        rb.drag = data.dragInAir;
        rb.AddForce(-data.gravityForce * transform.up);
    }

    private void CheckIsGroundedAndRotate() {
        _isGrounded = false;
        Vector3 averageNormal = Vector3.zero;
        int hitCount = 0;

        for (int i = 0; i < raycastPoints.Length; i++) {
            Vector3 worldPoint = transform.TransformPoint(raycastPoints[i]);

            if (Physics.Raycast(worldPoint, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer)) {
                _isGrounded = true;
                averageNormal += hit.normal;
                hitCount++;
            }
        }

        if (_isGrounded && hitCount > 0) {
            averageNormal /= hitCount;
            transform.rotation = Quaternion.FromToRotation(transform.up, averageNormal) * transform.rotation;
        }
        else {
            float uprightDot = Vector3.Dot(transform.up, Vector3.up);
            if (uprightDot < 0.5f) {
                HandleUprightRecovery();
            }
        }
    }

    private void HandleUprightRecovery() {
        Vector3 up = Vector3.up;
        float uprightDot = Vector3.Dot(transform.up, up);

        if (uprightDot < 0.5f && rb.velocity.y > -15f) {
            rb.AddForce(up * upRightForce * 0.5f, ForceMode.Acceleration);

            Vector3 torque = Vector3.Cross(transform.up, up);
            rb.AddTorque(torque * uprightTorque * 0.5f, ForceMode.Acceleration);
        }
    }

    private void CheckIfStuck() {
        float currentSpeed = rb.velocity.magnitude;
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);

        bool tryingToMove = Mathf.Abs(currentThrottle) > 0.1f;
        bool isStuck = currentSpeed < minVelocityThreshold && distanceMoved < minVelocityThreshold;

        if (isStuck && tryingToMove) {
            stuckTimer += Time.fixedDeltaTime;

            if (stuckTimer >= stuckDetectionTime) {
                ForceUprightRecovery();
                stuckTimer = 0f;
            }
        }
        else {
            stuckTimer = 0f;
        }

        lastPosition = transform.position;
    }

    private void ForceUprightRecovery() {
        Vector3 up = Vector3.up;

        rb.AddForce(up * upRightForce * 1.2f, ForceMode.VelocityChange);

        Vector3 torque = Vector3.Cross(transform.up, up);
        rb.AddTorque(torque * uprightTorque * 1.2f, ForceMode.VelocityChange);

        rb.AddForce(transform.forward * upRightForce * 0.3f, ForceMode.Impulse);

        if (rb.velocity.y > 20f) {
            rb.velocity = new Vector3(rb.velocity.x, 20f, rb.velocity.z);
        }
    }

    private void OnResetCar() {
        if (isIdle) {
            isIdle = false;
            AudioEvents.onCarIdleStop?.Invoke();
        }
        if (isAccelerating) {
            isAccelerating = false;
            AudioEvents.onCarAccelerateStop?.Invoke();
        }
        if (isBraking) {
            isBraking = false;
            AudioEvents.onCarBrakeStop?.Invoke();
        }
        if (isDrifting) {
            isDrifting = false;
            AudioEvents.onCarDriftStop?.Invoke();
        }

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
        rb.transform.rotation = Quaternion.identity;

        stuckTimer = 0f;
        lastPosition = transform.position;

        isDrifting = false;
    }

    private void UpdateSpeed() {
        if (Time.timeScale == 0f) return;

        float currentSpeed = rb.velocity.magnitude;

        CarEvents.onCarSpeedChanged?.Invoke(currentSpeed);
    }
}