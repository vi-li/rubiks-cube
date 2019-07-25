using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraRayCastGroup : MonoBehaviour
{
    public GameObject fadeCube;
    public float fadeAnimationLength = 2f; // in seconds
    public GameObject[] littleCollidersParent;  // size of this array = number of group cubes in scene
                                                // Assign the gameobjects named little colliders, not
                                                    // the little colliders themselves.
    private float MAX_RAY_DISTANCE = 100.0f;

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch[] myTouches = Input.touches;

            for (int i = 0; i < Input.touchCount; i++)
            {
                Ray ray = Camera.main.ScreenPointToRay(myTouches[i].position);
                RaycastHit hit;
                
                Physics.Raycast(ray, out hit, MAX_RAY_DISTANCE);

                // /////////////////
                // Switch to Master
                // /////////////////
                if (myTouches[i].phase == TouchPhase.Began && 
                    hit.transform.gameObject.tag == "switchScene")
                {
                    fadeOut();
                }

                if (myTouches[i].phase == TouchPhase.Began && 
                    hit.transform.gameObject.tag == "reloadScene")
                {
                    SceneManager.LoadScene("RubiksCubesGroup");
                }

                // /////////////////
                // Touch Side Rotate
                // /////////////////
                else if (hit.transform.gameObject.tag.StartsWith("smallColliders"))
                {
                    var tempIndex = hit.transform.gameObject.tag.Substring(14); // 14 is index after last "s"
                    int tempIntIndex = 0;

                    try 
                    {
                        tempIntIndex = Int32.Parse(tempIndex);

                    } catch (FormatException)
                    {
                        Debug.Log("Unable to find cube index of Small Colliders hit.");
                    }

                    littleCollidersParent[tempIntIndex].GetComponent<TouchSideRotateNew>().resolveFirstSecondTouch(hit, myTouches[i]);
                        
                }
            }
        }
    }
    
    void fadeOut()
    {
        fadeCube.GetComponent<Animator>().SetTrigger("fadeOutToMaster - Trigger");
    }
}
