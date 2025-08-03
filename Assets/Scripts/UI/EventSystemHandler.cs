using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemHandler : MonoBehaviour {

    [SerializeField] private GameObject firstSelectedObject;

    void OnEnable() {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectedObject);
    }
}
