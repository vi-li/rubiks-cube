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
    private bool isDone = false;

    private GameObject cubeCase = null;
    private GameObject actualCube = null;
    private LayerRotate layerRotate = null;
    private LayerRotate[] outerLayerRotateScript = null;
    private GameObject[] outerCubes = null;
    private GameObject[] outerActualCubes = null;
    private GameObject[] outerLargeColliders = null;
    private GameObject[] middleMiniCubes = null;
    private GameObject[] resetMiniCubes = null;
    private GameObject[] threeStepsMiniCubes = null;
    private GameObject resetCube = null;
    private GameObject threeStepsCube = null;
    private TextMeshProCounter textMeshPro = null;

    // Start is called before the first frame update
    void Start()
    {   
        /////////////////////////////////
        // Cube Cases, Actual Cubes, Large Colls
        /////////////////////////////////
        cubeCase = entireCubeSetUp.transform.Find("Cube Case").gameObject;

        outerCubes = cubeCase.GetComponent<MatchRotationOnStart>().outerCubes;
        outerActualCubes = new GameObject[outerCubes.Length];
        outerLargeColliders = new GameObject[outerCubes.Length];
        
        for (int i = 0; i < outerActualCubes.Length; i++)
        {
            outerActualCubes[i] = outerCubes[i].transform.Find("Actual Cube").gameObject;
        }

        for (int i = 0; i < outerCubes.Length; i++)
        {
            outerLargeColliders[i] = GameObject.Find("Table/Side Cube " + i + "/Large Collider").gameObject;
        }

        /////////////////////////////////
        // Layer Rotate
        /////////////////////////////////
        layerRotate = GetComponent<LayerRotate>();
        outerLayerRotateScript = new LayerRotate[outerCubes.Length];
        for (int i = 0; i < outerLayerRotateScript.Length; i++)
        {
            outerLayerRotateScript[i] = outerActualCubes[i].GetComponent<LayerRotate>();
        }

        /////////////////////////////////
        // Dummy Cubes
        /////////////////////////////////
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

        ///////////////////
        // Text Mesh Pro
        ///////////////////
        textMeshPro = GameObject.Find("Canvas/Counter").gameObject.GetComponent<TextMeshProCounter>();
    }

    // Update is called once per frame
    void Update()
    {
        // readyToMove is forcibly set to true if the "done" button is pressed

        if (inCheckingCubeProcess || inFixingCubeProcess || inFourDotsProcess || isDone)
        {
            return;
        }

        if (readyToMove && !arrivedAtMiddle)
        {
            moveToMiddle();
        }

        if (arrivedAtMiddle)
        {
            if (checkCubeStatus())
            {
                StartCoroutine(finishFourDots(false));

            } else
            {
                fixFourDots();
            }
        }

    }

    void moveToMiddle()
    {
        // Uncomment if re-enabling side motion.
        /*
        float step = speed * Time.deltaTime;
        entireCubeSetUp.transform.position = Vector3.MoveTowards(entireCubeSetUp.transform.position, target.position, step);

        if (Vector3.Distance(entireCubeSetUp.transform.position, target.transform.position) < DIST_EPSILON_FROM_MID)
        {
            arrivedAtMiddle = true;
        }
        */

        // Placeholder, since we aren't moving to the side anymore. Delete if re-enabling side motion.
        arrivedAtMiddle = true;
    }

    // CheckCubeStatus returns true if matching 3 steps cube configuration, false if not.
    bool checkCubeStatus()
    {
        inCheckingCubeProcess = true;

        for (int i = 0; i < middleMiniCubes.Length; ++i)
        {
            middleMiniCubes[i] = GetComponent<LayerRotate>().Cubes[i].gameObject;
            if (middleMiniCubes[i].name != threeStepsMiniCubes[i].name)
            {
                inCheckingCubeProcess = false;
                return false;
            }
        }

        inCheckingCubeProcess = false;
        return true;
    }

    IEnumerator finishFourDots(bool startFromBeginning)
    {
        // Enable constant rotation motion, disable large rotation
        cubeCase.GetComponent<ContRotate>().enabled = true;
        entireCubeSetUp.transform.Find("Large Collider").GetComponent<MultiTouchRubiksRotate>().enabled = false;
        
        // Disable rotation during four dots process
        if (outerLargeColliders != null)
        {        
            for (int i = 0; i < outerActualCubes.Length; i++)
            {
                outerLargeColliders[i].GetComponent<MultiTouchRubiksRotate>().enabled = false;
            }
        } else {
            Debug.Log("outerLargeColliders is NULL!");
        }

        // Reset Counter
        textMeshPro.setCounter(0);
        inFourDotsProcess = true;

        if (startFromBeginning)
        {
            yield return StartCoroutine (rotateAll(ELayer.B, true));
            yield return StartCoroutine (rotateAll(ELayer.B, true));
            yield return StartCoroutine (rotateAll(ELayer.F, true));
        }

        // Completed turns so far: B, B, F
        yield return StartCoroutine (rotateAll(ELayer.F, true));
        yield return StartCoroutine (rotateAll(ELayer.D, true));
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

        entireCubeSetUp.transform.Find("Large Collider").GetComponent<MultiTouchRubiksRotate>().enabled = true;
        if (outerLargeColliders != null)
        {        
            for (int i = 0; i < outerActualCubes.Length; i++)
            {
                outerLargeColliders[i].GetComponent<MultiTouchRubiksRotate>().enabled = true;
            }
        } else {
            Debug.Log("outerLargeColliders is NULL!");
        }

        inFourDotsProcess = false; 
        isDone = true;
    }

    void fixFourDots()
    {
        inFixingCubeProcess = true;

        for (int i = 0; i < outerActualCubes.Length; ++i)
        {
            for (int j = 0; j < 27; ++j)
            {
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

        StartCoroutine(finishFourDots(true));
    }

    private IEnumerator rotateAll(ELayer aLayer, bool clockwise)
    {
        layerRotate.RotateLayer(aLayer, clockwise);
        for (int i = 0; i < outerActualCubes.Length; i++)
        {
            outerActualCubes[i].GetComponent<LayerRotate>().RotateLayer(aLayer, clockwise);
        }

        textMeshPro.incCounter();
        yield return new WaitForSeconds(0.7f);
    }
}
