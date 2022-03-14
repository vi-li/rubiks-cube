using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOnStart : MonoBehaviour
{
    [SerializeField] private float fadePerSecond = 2.5f;
    public Material fadeMaterial;

    // Start is called before the first frame update
    void Start()
    {
        Color color = fadeMaterial.color;
        fadeMaterial.color = new Color(color.r, color.g, color.b, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
       Color color = fadeMaterial.color;
        if (color.a > 0) {
            fadeMaterial.color = new Color(color.r, color.g, color.b, color.a - (fadePerSecond * Time.deltaTime));
        }
        else {
            fadeMaterial.color = new Color(color.r, color.g, color.b, 0.0f);
        }
    }

}
