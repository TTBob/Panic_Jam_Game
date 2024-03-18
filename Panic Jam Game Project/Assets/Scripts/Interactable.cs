using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum types
    {
        door,
        ladder
    }
    [Tooltip("Type of interactable object")] public types type;
    [Tooltip("Player position when interacting, local space")] public Vector2 pos;
    [Header("Passage")]
    [Tooltip("Where the player will be moved when interacting with the passage")] public Interactable exit;
    [Header("Objects")]
    private Transform player;
    private Animator anim;
    private CameraController cam;
    public void Start()
    {
        player = GameObject.Find("Player").transform;
        anim = player.GetComponent<Animator>();
        cam = CameraController.instance;
    }
    public void interact()
    {
        // Called by player when interacting with object
        print("Interacted with " + gameObject.name);
        if (type == types.door)
        {
            interactStart();
            anim.SetTrigger("door");
        }
        else if (type == types.ladder)
        {
            interactStart();
            anim.SetTrigger("ladder");
        }
    }
    private void interactStart()
    {
        // Moves player to object and blocks movement
        player.position = transform.position + (Vector3)pos;
        PlayerController.instance.movementEnabled = false;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    // Functions called by animator
    public void moveToExit()
    {
        player.position = exit.transform.position + (Vector3)exit.pos;
        cam.room = exit.GetComponentInParent<Room>();
        float xPos;
        if (cam.room.dynamicCam)
        {
            xPos = Mathf.Clamp(player.position.x, cam.room.transform.position.x + cam.room.posRange.x, cam.room.transform.position.x + cam.room.posRange.y);
        }
        else
        {
            xPos = cam.room.transform.position.x;
        }
        cam.transform.position = new Vector3(xPos, cam.room.transform.position.y, -10);
        print("Moving!");
    }
    public void arriveAtExit()
    {
        PlayerController.instance.movementEnabled = true;
        PlayerController.instance.interactable = exit;
        TextBoxMaster.instance.ShowTextBox(new TextBoxClass
        {
            message = "You have transitioned from one room to the other"
        }, true);
    }
}
