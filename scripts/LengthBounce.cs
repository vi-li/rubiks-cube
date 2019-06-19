using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LengthBounce : MonoBehaviour
{
    public float minLength = 0.5f;
    public float maxLength = 1f;
    public float incLength = 0.03f;
    public float lengthenFactor = 1.0f;
    public float shortenFactor = 2.0f;
    private bool increasing = true;

    // Start is called before the first frame update
    void Start()
    {
        // Check that current x length is between bounds. If not, set to avg.
        if (transform.localScale.x >= maxLength || transform.localScale.x <= minLength) {
            transform.localScale = new Vector3((maxLength + minLength) / 2.0f,
                                                transform.localScale.y,
                                                transform.localScale.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If out of bounds, redirect
        if (transform.localScale.x >= maxLength || transform.localScale.x <= minLength) {
            increasing = !increasing;
        }

        // Perform scaling
        if (increasing) {
            // Lengthen the object by incLength
            transform.localScale += new Vector3(incLength * lengthenFactor, 0, 0);
        }
        else {
            // Shorten the object by factor * incLength
            transform.localScale -= new Vector3(incLength * shortenFactor, 0, 0);
        }

    }
}
