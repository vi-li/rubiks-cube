using System.Collections;
using UnityEngine;

public class MatchRotationOnStart : MonoBehaviour
{
    public GameObject[] outerCubes;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        foreach (GameObject outerCube in outerCubes)
        {
            outerCube.transform.rotation = transform.rotation;
        }
    }
}
