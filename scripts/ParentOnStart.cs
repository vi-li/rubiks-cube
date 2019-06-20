using System.Collections;
using UnityEngine;

public class ParentOnStart : MonoBehaviour
{
    public GameObject[] outerCubes;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject outerCube in outerCubes)
        {
            outerCube.transform.parent = transform;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
