using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RGBGradient : MonoBehaviour
{
    public Material gradientMaterial;
    public float redMax;
    public float redMin;
    public float greenMax;
    public float greenMin;
    public float blueMax;
    public float blueMin;
    private float shiftSpeed = 1.35f;
    private float shiftMaxDistance = 1.0f; // A value between 0 and 1. Percentage of maxIntensity you want it to pingpong to.
    private int RGBSelect = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartShifting(shiftSpeed, shiftMaxDistance);
    }
 
    void StartShifting(float shiftSpeed, float shiftMaxDistance)
    {
        float redValue = Mathf.Lerp (redMin, redMax, Mathf.PingPong (Time.time * shiftSpeed, shiftMaxDistance));
        float greenValue = Mathf.Lerp (greenMin, greenMax, Mathf.PingPong (Time.time * shiftSpeed, shiftMaxDistance));
        float blueValue = Mathf.Lerp (blueMin, blueMax, Mathf.PingPong (Time.time * shiftSpeed, shiftMaxDistance));
        // Color baseColor = gradientMaterial.color;
        // Color finalColor = baseColor * Mathf.LinearToGammaSpace (emission);
        Color finalColor = new Color(redValue, greenValue, blueValue);
        gradientMaterial.SetColor("_EmissionColor", finalColor);
    }
}
