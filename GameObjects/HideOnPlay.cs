using UnityEngine;

namespace Grimity.GameObjects {
public class HideOnPlay : MonoBehaviour {
    private void Start() {
        gameObject.SetActive(false);
    }
}
}