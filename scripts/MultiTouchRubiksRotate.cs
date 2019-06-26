using System.Collections;
using UnityEngine;

public class MultiTouchRubiksRotate : MonoBehaviour
{
  public GameObject cubeItself;
  public int cubeIndex;
  public float rotSpeed = 10f;
  public float MAX_RAY_DISTANCE = 100.0f;
  //public bool startedOnCube = false;
  public bool m_largeRotating = false;        // Exists so that rotation can continue over cube

  void Start()
  {

  }

  void Update()
  {

    if (Input.touchCount > 0)
    {
      Touch[] myTouches = Input.touches;

      for (int i = 0; i < Input.touchCount; i++)
      {
        Ray ray = Camera.main.ScreenPointToRay(myTouches[i].position);
        RaycastHit hit;

        // TODO: remove when done
        Debug.DrawRay(ray.origin, ray.direction*MAX_RAY_DISTANCE, Color.red, 2);
        
        Physics.Raycast(ray, out hit, MAX_RAY_DISTANCE);

        if ((hit.transform.gameObject.tag == ("rubiksCubesLColl" + cubeIndex)
            && myTouches[i].phase == TouchPhase.Began)

           || (m_largeRotating &&
              (hit.transform.gameObject.tag == ("rubiksCubesLColl" + cubeIndex) || hit.transform.gameObject.tag == ("smallColliders" + cubeIndex))))
        {
          if (!m_largeRotating && myTouches[i].phase == TouchPhase.Began) 
          { 
            m_largeRotating = true; 
          }

          cubeItself.transform.Rotate(myTouches[i].deltaPosition.y * rotSpeed * Time.deltaTime, 
                                      myTouches[i].deltaPosition.x * rotSpeed * Time.deltaTime, 
                                      0f, 
                                      Space.World);

          if (myTouches[i].phase == TouchPhase.Ended)
          {
            m_largeRotating = false;
          }
        }

/*         // Begin rotation
        if (!m_largeRotating && 
            myTouches[i].phase == TouchPhase.Began &&
            hit.transform.gameObject.tag == ("rubiksCubesLColl" + cubeIndex))
        {
          m_largeRotating = true;
        }

        if (!m_largeRotating &&
            myTouches[i].phase == TouchPhase.Began &&
            hit.transform.gameObject.tag == ("smallColliders" + cubeIndex))
        {
          startedOnCube = true;
        }

        // Continue rotating even if finger moves over cube
        if ((m_largeRotating || myTouches[i].phase == TouchPhase.Moved) &&
            (hit.transform.gameObject.tag == ("rubiksCubesLColl" + cubeIndex) || hit.transform.gameObject.tag == ("smallColliders" + cubeIndex)) &&
            !startedOnCube)
        {
          cubeItself.transform.Rotate(myTouches[i].deltaPosition.y * rotSpeed * Time.deltaTime, 
                                      myTouches[i].deltaPosition.x * rotSpeed * Time.deltaTime, 
                                      0f, 
                                      Space.World);
        }

        // End rotation
        if (myTouches[i].phase == TouchPhase.Ended)
        {
          m_largeRotating = false;
          startedOnCube = false;
        } */
        
      }   
    }
  }

  // Debugging purposes
  void OnMouseDrag()
  {
    float rotX = Input.GetAxis("Mouse X")*rotSpeed*Mathf.Deg2Rad;
    float rotY = Input.GetAxis("Mouse Y")*rotSpeed*Mathf.Deg2Rad;

    cubeItself.transform.RotateAround(Vector3.forward, -rotX);
    cubeItself.transform.RotateAround(Vector3.right, rotY);
  }

}


