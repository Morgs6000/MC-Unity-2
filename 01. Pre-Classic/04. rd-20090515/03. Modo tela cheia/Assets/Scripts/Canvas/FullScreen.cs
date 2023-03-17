using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreen : MonoBehaviour {
    private bool isFullScreen = false;
    
    private void Start() {
        isFullScreen = Screen.fullScreen;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.F11)) {
            isFullScreen = !isFullScreen;
            Screen.fullScreen = isFullScreen;
        }
    }
}
