using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used by animator to access functions in other objects for animation events
public class FunctionReferences : MonoBehaviour
{
    public void interactable_moveToExit() {
        GetComponent<PlayerController>().interactable.moveToExit();
    }
    public void interactable_arriveAtExit() {
        GetComponent<PlayerController>().interactable.arriveAtExit();
    }
}
