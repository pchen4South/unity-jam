using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracerFlicker : MonoBehaviour
{
    [SerializeField]    Renderer tracer = null;
    public float floor = 0.01f;
    public float ceiling = 1.0f;
    public float pulseSpeed = 1f;
    Color origEmissionColor;

    void Awake(){
        var tracerMat = tracer.material;
        origEmissionColor = tracerMat.GetColor("_EmissionColor");
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        Flicker();
    }
    void Flicker(){
            var tracerMat = tracer.material;
            float emission = floor + Mathf.PingPong (Time.time * pulseSpeed, ceiling - floor);
            Color finalColor = origEmissionColor * Mathf.LinearToGammaSpace (emission);
            tracerMat.SetColor("_EmissionColor", finalColor);
    }
}
