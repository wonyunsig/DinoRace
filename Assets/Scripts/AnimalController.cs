using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class AnimalController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 60f;

    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;
    private PhotonView view;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Disable physics-driven rotation
        view = GetComponent<PhotonView>();
    }

    private
        void Update()
    {
        if (view.IsMine)
        {
            // Get input for movement
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }
    }

    private void FixedUpdate()
    {
        // Calculate movement direction
        Vector3 movementDirection = transform.forward * verticalInput;
        rb.velocity = movementDirection * moveSpeed;

        // Calculate rotation
        Quaternion rotation = Quaternion.Euler(0, horizontalInput * rotationSpeed * Time.fixedDeltaTime, 0);
        rb.MoveRotation(rb.rotation * rotation);
    }
}