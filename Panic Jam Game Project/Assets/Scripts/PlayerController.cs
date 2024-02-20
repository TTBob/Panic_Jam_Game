using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour 
{
    [Header("Walking")]
    [Tooltip("The speed of walking (meters per second)")] public float walkSpeed;
    [Tooltip("The acceleration if velocity < walkspeed or no horizontal input is recieved")] public float walkAcc;
    [HideInInspector] public bool movementEnabled;

    [Header("Sprinting")]
    [Tooltip("Applied when shift is pressed")] public float sprintSpeed;
    [Tooltip("Applied when velocity > walkSpeed and horizontal input is recieved")] public float sprintAcc;
    
    [Header("Stamina")]
    [Tooltip("Time until stamina runs out (seconds)")] public float sprintTime;
    private float stamina;
    private bool staminaState; // True = usable, False = locked for refill
    [Tooltip("Time for stamina to refill (seconds)")] public float refillTime;

    private float targetVel;
    
    [Header("Jump")]
    [Tooltip("The force applied vertically to make the character jump, always applied once per jump")] public float jumpForce;

    [Header("Objects")]
    public GameSettings gameSettings;
    public Transform staminaUI;
    private Rigidbody2D rb;
    [HideInInspector] public Interactable interactable;
    
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        stamina = 1;
        movementEnabled = true;
    }
    
    void FixedUpdate() {
        bool sprinting = Input.GetButton("Sprint") && staminaState;
        if (movementEnabled) {
            targetVel = Input.GetAxisRaw("Horizontal") * (sprinting ? sprintSpeed : walkSpeed);
        } else {
            targetVel = 0;
        }

        // Calculate acceleration and stamina change
        float acc;
        // If sprinting, velocity > walkspeed and input in same direction as velocity (check input to make stopping completely and turning around after running faster)
        if (sprinting && ((rb.velocity.x > walkSpeed && Input.GetAxisRaw("Horizontal") == 1) || (rb.velocity.x < -walkSpeed && Input.GetAxisRaw("Horizontal") == -1))) {
            // Using stamina:
            acc = sprintAcc;
            stamina = Mathf.MoveTowards(stamina, 0, Time.deltaTime / sprintTime);
            if (stamina == 0) {
                staminaState = false;
            }
        } else {
            // Not using stamina:
            acc = walkAcc;
            stamina = Mathf.MoveTowards(stamina, 1, Time.deltaTime / refillTime);
            if (stamina == 1) {
                staminaState = true;
            }
        }

        // Calculate velocity and apply to rigidbody
        float vel = Mathf.MoveTowards(rb.velocity.x, targetVel, acc * Time.deltaTime);
        rb.velocity = new Vector2(vel, rb.velocity.y);

        // Update the stamina UI element (Temporary)
        float UIPos = Mathf.Lerp(-1, 0, stamina);
        float UIWidth = Mathf.Lerp(0, 2, stamina);
        staminaUI.localPosition = new Vector3(UIPos, staminaUI.localPosition.y, 0);
        staminaUI.localScale = new Vector3(UIWidth, staminaUI.localScale.y, 1);
        staminaUI.gameObject.GetComponent<SpriteRenderer>().color = staminaState ? Color.yellow : Color.red;

        // Jump and ground check
        if (Input.GetButton("Jump")) {
            RaycastHit2D hit;
            hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.9f, 0, -Vector2.up, 0.6f);
            if (hit) {
                if (rb.velocity.y == 0 && movementEnabled) {
                    rb.AddForce(Vector2.up * jumpForce);
                }
            }
        }

        // Interacting
        if (Input.GetButton("Interact") && movementEnabled) {
            if (interactable) {
                interactable.interact();
            } else {
                print("No interactable!");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Interactable") && movementEnabled) {
            interactable = col.gameObject.GetComponent<Interactable>();
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (interactable) {
            if (col.gameObject == interactable.gameObject && movementEnabled) {
                interactable = null;
            }
        }
    }
}
