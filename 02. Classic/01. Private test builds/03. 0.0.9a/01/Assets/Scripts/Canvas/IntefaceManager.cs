using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntefaceManager : MonoBehaviour {
    private bool isFullScreen = false;
    
    private void Start() {
        isFullScreen = Screen.fullScreen;
    }

    private void Update() {
        FullScreenInput();
        QuitGameInput();
    }

    private void FullScreenInput() {
        if(Input.GetKeyDown(KeyCode.F11)) {
            isFullScreen = !isFullScreen;
            Screen.fullScreen = isFullScreen;
        }
    }

    private void QuitGameInput() {
        if(Input.GetButtonDown("Escape")) {
            Application.Quit();
        }
    }
}
