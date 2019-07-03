using System.Collections;
using UnityEngine;

public class FinishFourDots : MonoBehaviour
{
    public GameObject entireCubeSetUp;
    public Transform target;
    public float DIST_EPSILON_FROM_MID = 0.6f;
    public bool readyToMove = false;

    private bool arrivedAtMiddle = false;
    private float speed = 10.0f;
    private bool inCheckingCubeProcess = false;
    private bool inFixingCubeProcess = false;
    private bool inFourDotsProcess = false;

    private GameObject cubeCase = null;
    private GameObject actualCube = null;
    private LayerRotate layerRotate = null;
    private LayerRotate[] outerLayerRotateScript = null;
    private GameObject[] outerCubes = null;
    private GameObject[] outerActualCubes = null;
    private GameObject[] middleMiniCubes = null;
    private GameObject[] resetMiniCubes = null;
    private GameObject[] threeStepsMiniCubes = null;
    private GameObject resetCube = null;
    private GameObject threeStepsCube = null;

    // Start is called before the first frame update
    void Start()
    {   
        //////////////////////
        // Cube Cases and Actual Cubes
        //////////////////////
        cubeCase = entireCubeSetUp.transform.Find("Cube Case").gameObject;
        outerCubes = cubeCase.GetComponent<MatchRotationOnStart>().outerCubes;
        outerActualCubes = new GameObject[outerCubes.Length];
        for (int i = 0; i < outerActualCubes.Length; i++)
        {
            outerActualCubes[i] = outerCubes[i].transform.Find("Actual Cube").gameObject;
        }

        //////////////////////
        // Layer Rotate
        //////////////////////
        layerRotate = GetComponent<LayerRotate>();
        outerLayerRotateScript = new LayerRotate[outerCubes.Length];
        for (int i = 0; i < outerLayerRotateScript.Length; i++)
        {
            outerLayerRotateScript[i] = outerActualCubes[i].GetComponent<LayerRotate>();
        }

        //////////////////////
        // Dummy Cubes
        //////////////////////
        resetCube = GameObject.Find("Dummy Cube Reset").gameObject;
        threeStepsCube = GameObject.Find("Dummy Cube 3Steps").gameObject;

        middleMiniCubes = new GameObject[GetComponent<LayerRotate>().Cubes.Length];
        for (int i = 0; i < middleMiniCubes.Length; ++i)
        {
            middleMiniCubes[i] = GetComponent<LayerRotate>().Cubes[i].gameObject;
        }

        resetMiniCubes = new GameObject[GetComponent<LayerRotate>().Cubes.Length];
        for (int i = 0; i < resetMiniCubes.Length; ++i)
        {
            resetMiniCubes[i] = resetCube.GetComponent<LayerRotateDummy>().Cubes[i].gameObject;
        }

        threeStepsMiniCubes = new GameObject[GetComponent<LayerRotate>().Cubes.Length];
        for (int i = 0; i < threeStepsMiniCubes.Length; ++i)
        {
            threeStepsMiniCubes[i] = threeStepsCube.GetComponent<LayerRotateDummy>().Cubes[i].gameObject;
        }
        
        Debug.Log("Should be done setting all vars");
    }

    // Update is called once per frame
    void Update()
    {
        // readyToMove is forcibly set to true if the "done" button is pressed

        if (inCheckingCubeProcess || inFixingCubeProcess || inFourDotsProcess)
        {
            return;
        }

        if (readyToMove && !arrivedAtMiddle)
        {
            Debug.Log("Starting to move to middle");
            moveToMiddle();
        }

        if (arrivedAtMiddle)
        {
            Debug.Log("Apple");
            Debug.Log(middleMiniCubes[0].name + " " + threeStepsMiniCubes[0].name);
            if (checkCubeStatus())
            {
                StartCoroutine(finishFourDots(false));

            } else
            {
                Debug.Log("Butter");
                Debug.Log(middleMiniCubes[0].name + " " + threeStepsMiniCubes[0].name);
                fixFourDots();
            }
        }

    }

    void moveToMiddle()
    {
        float step = speed * Time.deltaTime;
        entireCubeSetUp.transform.position = Vector3.MoveTowards(entireCubeSetUp.transform.position, target.position, step);

        if (Vector3.Distance(transform.position, target.transform.position) < DIST_EPSILON_FROM_MID)
        {
            arrivedAtMiddle = true;
            Debug.Log("we have arrived at middle :)");
        }
    }

    // CheckCubeStatus returns true if matching 3 steps cube configuration, false if not.
    bool checkCubeStatus()
    {
        inCheckingCubeProcess = true;

        Debug.Log("Checking cube status");
        for (int i = 0; i < middleMiniCubes.Length; ++i)
        {
            middleMiniCubes[i] = GetComponent<LayerRotate>().Cubes[i].gameObject;
            if (middleMiniCubes[i].name != threeStepsMiniCubes[i].name)
            {
                Debug.Log(middleMiniCubes[i].name + " " + threeStepsMiniCubes[i].name);
                Debug.Log("NOT the same as three steps!!!");
                return false;
            }
        }
        Debug.Log("same as 3 steps :)");

        inCheckingCubeProcess = false;
        return true;
    }

    IEnumerator finishFourDots(bool startFromBeginning)
    {
        inFourDotsProcess = true;

        if (startFromBeginning)
        {
            Debug.Log("Finishing four dots, starting from beginning");
            yield return StartCoroutine(rotateAll(ELayer.B, true));
            yield return StartCoroutine(rotateAll(ELayer.B, true));
            yield return StartCoroutine(rotateAll(ELayer.F, true));
        }

        // Completed turns so far: B, B, F
        Debug.Log("Finishing four dots, after 3 steps");
        yield return StartCoroutine(rotateAll(ELayer.F, true));
        yield return StartCoroutine (rotateAll(ELayer.F, true));
        yield return StartCoroutine (rotateAll(ELayer.U, true));
        yield return StartCoroutine (rotateAll(ELayer.U, true));
        yield return StartCoroutine (rotateAll(ELayer.U, true));
        yield return StartCoroutine (rotateAll(ELayer.R, true));
        yield return StartCoroutine (rotateAll(ELayer.R, true));
        yield return StartCoroutine (rotateAll(ELayer.L, true));
        yield return StartCoroutine (rotateAll(ELayer.L, true));
        yield return StartCoroutine (rotateAll(ELayer.D, true));
        yield return StartCoroutine (rotateAll(ELayer.U, true));
        yield return StartCoroutine (rotateAll(ELayer.U, true));
        yield return StartCoroutine (rotateAll(ELayer.U, true));

        inFourDotsProcess = false;
    }

    void fixFourDots()
    {
        inFixingCubeProcess = true;

        Debug.Log("Cube was not in 3 steps formation, fixing cube");
        for (int i = 0; i < outerActualCubes.Length; ++i)
        {
            Debug.Log("Current Cube to fix: " + i);
            for (int j = 0; j < 27; ++j)
            {
                //Debug.Log("" + j + " " + outerActualCubes[i].GetComponent<LayerRotate>().Cubes[j].gameObject.name + " " + j.ToString() + " " + outerActualCubes[i].transform.Find(j.ToString()).gameObject.name);
                outerLayerRotateScript[i].Cubes[j] = outerActualCubes[i].transform.Find(j.ToString());
                var currMiniCube = outerLayerRotateScript[i].Cubes[j];

                if (outerLayerRotateScript[i].Cubes[j] != null)
                {
                    currMiniCube.localPosition = resetMiniCubes[j].transform.localPosition;
                    currMiniCube.localRotation = resetMiniCubes[j].transform.localRotation;
                } else {
                    Debug.Log("Outer cube " + i + " child not found");
                }
                
            }
        }

        Debug.Log("Fixing master cube");
        for (int j = 0; j < 27; ++j)
            {
                layerRotate.Cubes[j] = transform.Find(j.ToString());
                var currMiniCube = layerRotate.Cubes[j];
                if (currMiniCube != null)
                {
                    currMiniCube.localPosition = resetMiniCubes[j].transform.localPosition;
                    currMiniCube.localRotation = resetMiniCubes[j].transform.localRotation;
                } else {
                    Debug.Log("Master cube child not found");
                }

            }

        inFixingCubeProcess = false;

        finishFourDots(true);
    }

    IEnumerator rotateAll(ELayer aLayer, bool clockwise)
    {
        Debug.Log("Rotating all... ");
        layerRotate.RotateLayer(aLayer, clockwise);
        for (int i = 0; i < outerActualCubes.Length; i++)
        {
            outerActualCubes[i].GetComponent<LayerRotate>().RotateLayer(aLayer, clockwise);
        }
        return null;
    }
}
