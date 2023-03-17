using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugScreen : MonoBehaviour {
    private TextMeshProUGUI underlay;
    private TextMeshProUGUI textMeshPro;

    private float timer;
    private float frameRate;

    private void Awake() {
        textMeshPro = transform.Find("Debug Text").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        underlay = transform.Find("Debug Text Underlay").gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }
    
    private void Start() {
        
    }

    private void Update() {
        underlay.text = textMeshPro.text;

        DebugText();
    }

    private void DebugText() {
        textMeshPro.text = (
            "0.0.2a" + "\n" +
            FpsUpdate()
        );
    }

    private string FpsUpdate() {
        if(timer > 1.0f) {
            frameRate = (int)(1.0f / Time.unscaledDeltaTime);
            timer = 0;
        }
        else {
            timer += Time.deltaTime;
        }

        string text = (
            frameRate + " fps"
        );

        return text;
    }
}
