using UnityEngine;

public class Coin : MonoBehaviour {
    [SerializeField] private float rotationSpeed = 100f;

    // Update is called once per frame
    void Update() {
        Rotate();
    }

    void OnTriggerEnter(Collider other) {
        GameManager.Instance.AddCoin();
        Destroy(gameObject);
    }

    private void Rotate() {
        // Rotate the coin around its x-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }
}
