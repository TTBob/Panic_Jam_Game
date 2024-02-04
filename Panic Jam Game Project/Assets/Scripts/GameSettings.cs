using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [Range(30,120)]
    public int targetFPS;
    void Awake() {
        DontDestroyOnLoad(transform.gameObject);
    }
    void Start() {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFPS;
    }
}
