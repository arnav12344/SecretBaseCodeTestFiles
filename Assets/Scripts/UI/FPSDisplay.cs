using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    [Header("UI Text for FPS Display")]
    [SerializeField] private Text fpsText;

    private float deltaTime;

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        float milliseconds = deltaTime * 1000f;
        float fps = 1f / deltaTime;
        fpsText.text = string.Format("{0:0.0} ms ({1:0.} fps)", milliseconds, fps);
    }
}
