using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class DisableMasterCube : MonoBehaviour
{
    public float MAX_RAY_DISTANCE = 100.0f;
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

                if (hit.transform.gameObject.tag == "switchScene")
                {
                    unloadMasterCube();
                }
            }   
        }
    }

    void unloadMasterCube()
    {
        SceneManager.LoadScene("RubiksCubesGroup");
    }
}
