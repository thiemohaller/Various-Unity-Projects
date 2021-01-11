using UnityEngine;

public class PlayerLook : MonoBehaviour {
    public float mouseSensitivity = 500f;
    public Transform playerbody;

    float rotationX = 0f;

    // Start is called before the first frame update
    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        rotationX -= mouseY;
        // clamp
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        playerbody.Rotate(Vector3.up * mouseX);
        transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }
}
