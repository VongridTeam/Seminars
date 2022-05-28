using UnityEngine;

namespace Vongrid.DemoFPS.Demo
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float jumpingForce;
        [SerializeField]
        private float mouseSensitivity = 300f;
        [SerializeField]
        private float moveSpeed = 10;

        [SerializeField]
        private Camera playerCamera;
        [SerializeField]
        private GameObject playerFeet;
        [SerializeField]
        private GameObject hand;
        [SerializeField]
        private LayerMask groundMask;

        private Vector2 mouseInputAxis;
        private Vector2 inputAxis;
        private bool isSpacePressed;
        private float mouseXRotation;

        private Rigidbody playerBody;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            playerBody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            UpdateInput();
            RotatePlayer();
        }

        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void UpdateInput()
        {
            mouseInputAxis.x = Input.GetAxis("Mouse X");
            mouseInputAxis.y = Input.GetAxis("Mouse Y");

            inputAxis.x = Input.GetAxis("Horizontal");
            inputAxis.y = Input.GetAxis("Vertical");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isSpacePressed = true;
            }
        }

        private void MovePlayer()
        {
            Vector3 movementVectorHorizontal = transform.right * inputAxis.x;
            Vector3 movementVectorVertical = transform.forward * inputAxis.y;
            Vector3 movementVector = (movementVectorVertical + movementVectorHorizontal) * moveSpeed * Time.deltaTime;

            playerBody.MovePosition(transform.position + movementVector);

            if (isSpacePressed && IsGrounded())
            {
                playerBody.AddForce(Vector3.up * jumpingForce, ForceMode.Impulse);
            }
            isSpacePressed = false;
        }

        private void RotatePlayer()
        {
            mouseInputAxis *= mouseSensitivity * Time.deltaTime;

            transform.Rotate(new Vector3(0, mouseInputAxis.x, 0));

            mouseXRotation -= mouseInputAxis.y;
            mouseXRotation = Mathf.Clamp(mouseXRotation, -90f, 90f);
            playerCamera.transform.localRotation = Quaternion.Euler(mouseXRotation, 0f, 0f);
            hand.transform.localRotation = Quaternion.Euler(-90 + mouseXRotation, 0, 90);
        }

        private bool IsGrounded()
        {
            return Physics.OverlapSphere(playerFeet.transform.position, 0.25f, groundMask).Length > 0;
        }
    }
}
