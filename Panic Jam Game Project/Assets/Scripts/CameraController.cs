using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float lerpSpeed;
    public Room room;
    
    void Update() {
        float xPos;
        if (room.dynamicCam) {
            xPos = Mathf.Lerp(transform.position.x, Mathf.Clamp(player.position.x, room.transform.position.x + room.posRange.x, room.transform.position.x + room.posRange.y), lerpSpeed);
        } else {
            xPos = room.transform.position.x;
        }
        transform.position = new Vector3(xPos, room.transform.position.y, -10);
    }
}
