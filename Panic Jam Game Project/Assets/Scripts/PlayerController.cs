using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour 
{
    public float topSpeed;
    [Range(0, 1)]
    public float acc;
    private float targetVel;
    public float jumpForce;
    private Rigidbody2D rb;
    public GameSettings gameSettings;
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate() {
        targetVel = Input.GetAxisRaw("Horizontal") * topSpeed;
        float vel = Mathf.Lerp(rb.velocity.x, targetVel, 1 - Mathf.Pow(1 - acc, Time.deltaTime * gameSettings.targetFPS));
        rb.velocity = new Vector2(vel, rb.velocity.y);

        if (Input.GetButton("Jump")) {
            RaycastHit2D hit;
            hit = Physics2D.BoxCast(transform.position, Vector2.one, 0, -Vector2.up, 0.51f);
            if (hit) {
                if (hit.transform.CompareTag("Ground") && rb.velocity.y == 0) {
                    rb.AddForce(Vector2.up * jumpForce);
                }
            }
        }
    }
}
