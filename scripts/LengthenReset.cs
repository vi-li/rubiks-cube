using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LengthenReset : MonoBehaviour
{
    //private bool increasing = true;
    public float minLength = 0.5f;
    public float maxLength = 1f;
    public float incLength = 0.03f;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.localScale.x >= maxLength || transform.localScale.x <= minLength) {
        transform.localScale = new Vector3((maxLength + minLength) / 2.0f,
                                            transform.localScale.y,
                                            transform.localScale.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x >= maxLength) {
            transform.localScale = new Vector3(minLength, transform.localScale.y, transform.localScale.z);
        }

        transform.localScale += new Vector3(incLength, 0, 0);

    }
}
