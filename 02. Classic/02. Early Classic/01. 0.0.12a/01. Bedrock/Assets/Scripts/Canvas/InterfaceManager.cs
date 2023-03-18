using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour {
    private bool isFullScreen = false;
    private bool isPaused = false;
    
    private void Start() {
        isFullScreen = Screen.fullScreen;
    }

    private void Update() {
        FullScreenInput();
        QuitGameInput();
        PauseInput();
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

    private void PauseInput() {
        if(Input.GetKey(KeyCode.F10)) {
            if(Input.GetKey(KeyCode.Escape)) {
                if(Input.GetKeyDown(KeyCode.LeftAlt)) {
                    isPaused = !isPaused;
                }
            }
        } 
    }

    public bool IsPaused() {
        return isPaused;
    }
}
