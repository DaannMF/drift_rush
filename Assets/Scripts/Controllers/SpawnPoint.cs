using UnityEngine;

public class SpawnPoint : MonoBehaviour {
    [Header("Spawn Settings")]
    [SerializeField] private float rotationOffset = 0f;

    [Header("Debug")]
    [SerializeField] private bool showGizmos = true;
    [SerializeField] private Color gizmoColor = Color.green;
    [SerializeField] private Color directionColor = Color.red;
    [SerializeField] private float gizmoSize = 1f;
    [SerializeField] private float arrowLength = 2f;

    private void OnDrawGizmos() {
        if (!showGizmos) return;

        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(transform.position, Vector3.one * gizmoSize);

        Gizmos.color = directionColor;
        Vector3 forward = transform.forward * arrowLength;
        Gizmos.DrawRay(transform.position, forward);

        Vector3 arrowTip = transform.position + forward;
        Vector3 arrowRight = transform.right * (gizmoSize * 0.3f);
        Vector3 arrowUp = transform.up * (gizmoSize * 0.3f);

        Gizmos.DrawLine(arrowTip, arrowTip - forward * 0.3f + arrowRight);
        Gizmos.DrawLine(arrowTip, arrowTip - forward * 0.3f - arrowRight);
        Gizmos.DrawLine(arrowTip, arrowTip - forward * 0.3f + arrowUp);
        Gizmos.DrawLine(arrowTip, arrowTip - forward * 0.3f - arrowUp);

        if (Mathf.Abs(rotationOffset) > 0.1f) {
            Gizmos.color = Color.yellow;
            Quaternion offsetRotation = transform.rotation * Quaternion.Euler(0, rotationOffset, 0);
            Vector3 finalForward = offsetRotation * Vector3.forward * arrowLength * 0.8f;
            Gizmos.DrawRay(transform.position, finalForward);

            Vector3 finalArrowTip = transform.position + finalForward;
            Vector3 finalArrowRight = offsetRotation * Vector3.right * (gizmoSize * 0.25f);
            Vector3 finalArrowUp = offsetRotation * Vector3.up * (gizmoSize * 0.25f);

            Gizmos.DrawLine(finalArrowTip, finalArrowTip - finalForward * 0.3f + finalArrowRight);
            Gizmos.DrawLine(finalArrowTip, finalArrowTip - finalForward * 0.3f - finalArrowRight);
            Gizmos.DrawLine(finalArrowTip, finalArrowTip - finalForward * 0.3f + finalArrowUp);
            Gizmos.DrawLine(finalArrowTip, finalArrowTip - finalForward * 0.3f - finalArrowUp);
        }
    }

    public void SpawnObject(GameObject obj) {
        if (obj == null) return;

        obj.transform.position = transform.position;

        Quaternion finalRotation = transform.rotation * Quaternion.Euler(0, rotationOffset, 0);
        obj.transform.rotation = finalRotation;

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null) {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}