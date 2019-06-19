using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsateLight : MonoBehaviour
{
    public Material lightOnMaterial;
    private float minIntensity = 0.5f;
    private float maxIntensity = 2.0f;
    private float pulsateSpeed = 1.35f;
    private float pulsateMaxDistance = 1.0f; // A value between 0 and 1. Percentage of maxIntensity you want it to pingpong to.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartPulsating(minIntensity, maxIntensity, pulsateSpeed, pulsateMaxDistance);
    }
 
    void StartPulsating (float minIntensity, float maxIntensity, float pulsateSpeed, float pulsateMaxDistance)
    {
        float emission = Mathf.Lerp (minIntensity, maxIntensity, Mathf.PingPong (Time.time * pulsateSpeed, pulsateMaxDistance));
        Color baseColor = lightOnMaterial.color;
        Color finalColor = baseColor * Mathf.LinearToGammaSpace (emission);
        lightOnMaterial.SetColor("_EmissionColor", finalColor);
    }
}
