using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector3 eulerRotation;
    [SerializeField] float damper;

    // Start is called before the first frame update
    void Start() {
        transform.eulerAngles = eulerRotation;
    }

    // Update is called once per frame
    private void FixedUpdate() {
        FollowTarget();
    }

    private void FollowTarget() {
        if (target == null) return;

        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.fixedDeltaTime * damper);
    }
}
