using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public CharacterController characterController;
    [Range(1, 256)]
    public int lateralMovementSpeed = 10;
    [Range(1, 256)]
    public int longitudinalMovementSpeed = 12;
    public float gravity = 20f;
    public float runningMultiplier = 1.5f;
    public bool enableJumping = false;
    public float jumpHeight = 3f;

    Vector3 velocity;
    Vector3 move;

    private void Start() {
        characterController = GetComponent<CharacterController>();
    }

    void Update() {
        if (!characterController.isGrounded) {
            velocity.y = -4f;
        }

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isJumping = Input.GetButtonDown("Jump");
        float axisX = Input.GetAxis("Horizontal");
        float axisZ = Input.GetAxis("Vertical");

        if (isRunning) {
            move = (transform.right * axisX * lateralMovementSpeed * runningMultiplier) + (transform.forward * axisZ * longitudinalMovementSpeed * runningMultiplier);
        } else {
            move = (transform.right * axisX * lateralMovementSpeed) + (transform.forward * axisZ * longitudinalMovementSpeed);
        }

        characterController.Move(move * Time.deltaTime); // deltatime has to be included to make it frame independent

        if (enableJumping && isJumping && characterController.isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * (-2f * gravity));
        }

        velocity.y += gravity * Time.deltaTime;
        // gravity: 1/2 * g * t^2 -> times deltaTime TWICE!
        characterController.Move(velocity * Time.deltaTime);

    }
}
