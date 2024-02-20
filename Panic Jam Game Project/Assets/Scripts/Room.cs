using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Tooltip("Constant or dynamic camera movement")] public bool dynamicCam;
    [Tooltip("x position range for the camera, local space")] public Vector2 posRange;
}
