using System.Collections;
using UnityEngine;

public class CameraRayCast : MonoBehaviour
{
    public GameObject masterActualCube;
    public GameObject fadeCube;
    public GameObject doneButton;
    public float fadeAnimationLength = 2f; // in seconds
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

                if (myTouches[i].phase == TouchPhase.Began)
                {
                    if (hit.transform.gameObject.tag == "doneButton")
                    {
                        moveMasterToMiddle();
                    } 
                    else if (hit.transform.gameObject.tag == "switchScene") 
                    {
                        fadeOut();
                    }
                }
            }
        }
    }
    
    void moveMasterToMiddle()
    {
        masterActualCube.GetComponent<FinishFourDots>().readyToMove = true;
        doneButton.GetComponent<Animator>().SetTrigger("moveToMiddle");
    }
    
    void fadeOut()
    {
        fadeCube.GetComponent<Animator>().SetTrigger("fadeOutToGroup - Trigger");
    }
}
