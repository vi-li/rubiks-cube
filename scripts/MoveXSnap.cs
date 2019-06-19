using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveXSnap : MonoBehaviour
{
    public float distTravel = 2.0f;
    public float waitTimeInSecs = 1.0f;
    public float distInterval = 0.03f;

    private float startX;
    private float startY;
    private float startZ;
    //private Vector3 startMarker = transform.position;
    private Vector3 endMarker;
    

    // Start is called before the first frame update
    void Start()
    {
        startX = transform.position.x;
        startY = transform.position.y;
        startZ = transform.position.z;

        endMarker = new Vector3(transform.position.x + distInterval, 
                                transform.position.y, 
                                transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        // If reached end, fade and return to start
        if (transform.position.x >= endMarker.x) {
            //WaitandFade(waitTimeInSecs);      //temp

            transform.position = new Vector3(startX, startY, startZ);
        }

        // Move
        transform.position += new Vector3(distInterval, 0, 0);

    }

    // void WaitandFade(float waitSecs) 
    // {
    //     //yield return WaitForSeconds(waitSecs); //temp
    //     return;    // TODO: REMOVE


    // }
}
