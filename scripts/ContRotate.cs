using System.Collections;
using UnityEngine;

public class ContRotate : MonoBehaviour
{
    public float xAngle;
    public float yAngle;
    public float zAngle;

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(xAngle, yAngle, zAngle, Space.World);
    }
}
