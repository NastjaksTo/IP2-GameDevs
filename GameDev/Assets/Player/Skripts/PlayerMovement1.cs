using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Describes the movements of the player
/// </summary>
public class PlayerMovement1 : MonoBehaviour
{
    public CharacterController playerController;        //referenz set in editor
    public Transform cam;                               //referenz set in editor

    public float movementSpeed = 6;
    public float turnSmoothVelocity; 
    public float turnSmoothTime = 0.1f;

    /// <summary>
    /// Start is called before the first frame update. It locked the cursor.
    /// </summary>
    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }


    /// <summary>
    /// Called once per frame, make the character walk.
    /// </summary>
    void Update() {

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized; 

        if (direction.magnitude >= 0.1f) {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            playerController.Move(moveDir.normalized * movementSpeed * Time.deltaTime);
        }
    }
}
