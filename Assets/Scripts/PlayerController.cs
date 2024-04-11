using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Rigidbody of the player.
    private Rigidbody rb; 
    private int count;

    // Movement along X and Y axes.
    private float movementX;
    private float movementY;

    public float speed = 0; // Speed at which the player moves.
    public float jumpForce = 5.0f; // Force applied for jumps
    private int jumpCount = 0; // Tracks how many times the player has jumped
    private bool isGrounded; // Tracks whether the player is touching the ground


    public TextMeshProUGUI countText;

    public GameObject winTextObject;


    // Start is called before the first frame update.
    void Start()
    {
        count = 0;
        rb = GetComponent<Rigidbody>(); // Get and store the Rigidbody component attached to the player.
        SetCountText();
        winTextObject.SetActive(false);


    }
    
    // This function is called when a move input is detected.
    void OnMove(InputValue movementValue)
    {
    // Convert the input value into a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>();

    // Store the X and Y components of the movement.
        movementX = movementVector.x; 
        movementY = movementVector.y; 
    }

    // Add a method for jump input
    void OnJump(InputValue value)
    {
        // Check if grounded or if double jump is available
        if (isGrounded || jumpCount < 2)
        {
            rb.AddForce(new Vector3(0.0f, jumpForce, 0.0f), ForceMode.Impulse);
            isGrounded = false;
            jumpCount++;
        }
    }

    void SetCountText() 
   {
       countText.text =  "Count: " + count.ToString();
       if (count >= 8)
       {
           winTextObject.SetActive(true);
       }
   }

    // FixedUpdate is called once per fixed frame-rate frame.
    private void FixedUpdate() 
    {
    // Create a 3D movement vector using the X and Y inputs.
        Vector3 movement = new Vector3 (movementX, 0.0f, movementY);

    // Apply force to the Rigidbody to move the player.
        rb.AddForce(movement * speed); 
    }


    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("PickUp")) 
        {

            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();

        }
    }

    // Reset jump count on collision with the ground
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }
}
