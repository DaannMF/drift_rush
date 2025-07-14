using UnityEngine;

public class Coin : MonoBehaviour {
    [SerializeField] private float rotationSpeed = 100f;

    void Update() {
        Rotate();
    }

    void OnTriggerEnter(Collider other) {
        AudioEvents.onCoinCollected?.Invoke();
        GameEvents.onAddCoin?.Invoke();
        Destroy(gameObject);
    }

    private void Rotate() {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }
}
