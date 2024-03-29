﻿using System.Collections;
using UnityEngine;

public class TouchSideRotateNew : MonoBehaviour
{
    /*  Array layout
     *       c24  c25  c26
     *     c21  c22  c23|
     *   c18  c19  c20  |
     *   |              |
     *   |   c15  c16  c17
     *   | c12  c13  c14|
     *   c9   c10  c11  |
     *   |              |
     *   |   c6   c7   c8
     *   | c3   c4   c5
     *   c0---c1---c2       Front in this POV is GREEN (previously red). Up is WHITE.
     *
     * */

    private const int TOUCH_ARR_SIZE = 15;
    private const int SM_COLL_ARR_SIZE = 54;
    private Transform firstTouchTrans;
    private Transform secondTouchTrans;
    private bool readyToDetectSecond = true;
    private TextMeshProCounter textMeshPro = null;


    public float MAX_SWIPE_LENGTH = 30f;
    public float MIN_SWIPE_LENGTH = 0.5f;
    //public float MAX_RAY_DISTANCE = 100.0f;
    public GameObject actualCube;
    public int cubeIndex; // 0-6, number of rubik's cubes
    public Transform[] Colliders = new Transform[SM_COLL_ARR_SIZE];


    // Start is called before the first frame update
    void Start()
    {
        // **************************************************************************************
        // IMPORTANT!: Ensure that all collider children are in correct order in the hierarchy.
        // Correct order is listed below in "Small Colliders Array Setup". 
        // IF YOU HAVE THE SMALL COLLIDERS IN THE RIGHT ORDER IN HIERARCHY, 
        // YOU DO NOT NEED TO MANUALLY ASSIGN SMALL COLLIDERS IN INSPECTOR.
        // (If you come up with a better way to do this, feel free to restructure.)
        // **************************************************************************************
        for (int i = 0; i < Colliders.Length; ++i)
        {
            Colliders[i] = transform.GetChild(i);
        }

        // Assign counter text script and reset
        textMeshPro = GameObject.Find("Canvas/Counter").gameObject.GetComponent<TextMeshProCounter>();
        textMeshPro.setCounter(0);
    }

    // Update is called once per frame
    void Update()
    {
        // OLD RAYCASTING CODE - THIS HAS BEEN MOVED INTO CameraRayCastGroup.cs and the below resolve func
        // OLD RAYCASTING CODE - THIS HAS BEEN MOVED INTO CameraRayCastGroup.cs and the below resolve func
        // OLD RAYCASTING CODE - THIS HAS BEEN MOVED INTO CameraRayCastGroup.cs and the below resolve func

        // if (Input.touchCount > 0)
        // {
        //     Touch[] myTouches = Input.touches;

        //     for (int i = 0; i < Input.touchCount; i++)
        //     {
        //         Ray ray = Camera.main.ScreenPointToRay(myTouches[i].position);
        //         RaycastHit hit;
        //         Physics.Raycast(ray, out hit, MAX_RAY_DISTANCE);

        //         if (hit.collider.tag == ("smallColliders" + cubeIndex))
        //         {
        //             // Detect swipe start
        //             if (myTouches[i].phase == TouchPhase.Began)
        //             {
        //                 firstTouchTrans = hit.transform;

        //                 secondTouchTrans = null;
        //                 readyToDetectSecond = true;
        //             }
        //             //Detects swipe while finger is still moving
        //             if (myTouches[i].phase == TouchPhase.Moved)
        //             {   // Insert anything to do while moving here <---
        //                 if (readyToDetectSecond && hit.transform != firstTouchTrans)
        //                 {
        //                     secondTouchTrans = hit.transform;
        //                     readyToDetectSecond = false;
        //                 }

        //                 if (secondTouchTrans != null) 
        //                 {
        //                     checkSwipe(firstTouchTrans, secondTouchTrans, true);
        //                     secondTouchTrans = null;
        //                 }
        //             }
        //             //Detects swipe after finger is released
        //             if (myTouches[i].phase == TouchPhase.Ended)
        //             {
        //                 if (secondTouchTrans != null) 
        //                 {
        //                     checkSwipe(firstTouchTrans, secondTouchTrans, true);
        //                     secondTouchTrans = null;
        //                 }                    
        //             }
        //         }
        //     }      
        // }
    }

    public void resolveFirstSecondTouch(RaycastHit hit, Touch currTouch)
    {
        if (currTouch.phase == TouchPhase.Began)
        {
            firstTouchTrans = hit.transform;

            secondTouchTrans = null;
            readyToDetectSecond = true;
        }
        //Detects swipe while finger is still moving
        if (currTouch.phase == TouchPhase.Moved)
        {   // Insert anything to do while moving here <---
            if (readyToDetectSecond && hit.transform != firstTouchTrans)
            {
                secondTouchTrans = hit.transform;
                readyToDetectSecond = false;
            }

            if (secondTouchTrans != null) 
            {
                checkSwipe(firstTouchTrans, secondTouchTrans, true);
                secondTouchTrans = null;
            }
        }
        // Detects swipe after finger is released
        if (currTouch.phase == TouchPhase.Ended)
        {
            if (secondTouchTrans != null) 
            {
                checkSwipe(firstTouchTrans, secondTouchTrans, true);
                secondTouchTrans = null;
            }                    
        }
    }

    /*
    SMALL COLLIDERS ARRAY SETUP (not the most elegant method, but it works?)
    Elem:      Collider:
    0           0F
    1           1F
    2           2F
    3           9F
    4           10F
    5           11F
    6           18F
    7           19F
    8           20F
    9           20R
    10          23R
    11          26R
    12          11R
    13          14R
    14          17R
    15          2R
    16          5R
    17          8R
    18          26B
    19          25B
    20          24B
    21          17B
    22          16B
    23          15B
    24          8B
    25          7B
    26          6B
    27          24L
    28          21L
    29          18L
    30          15L
    31          12L
    32          9L
    33          6L
    34          3L
    35          0L
    36          26U
    37          23U
    38          20U
    39          25U
    40          22U
    41          19U
    42          24U
    43          21U
    44          18U
    45          8D
    46          5D
    47          2D
    48          7D
    49          4D
    50          1D
    51          6D
    52          3D
    53          0D
    */

    void checkSwipe(Transform fT, Transform sT, bool clockwise)
    {
        // FRONT
        if ((fT == Colliders[29] && sT == Colliders[44])  ||
            (fT == Colliders[44] && sT == Colliders[41])  ||
            (fT == Colliders[41] && sT == Colliders[38])  ||
            (fT == Colliders[38] && sT == Colliders[9])   ||
            (fT == Colliders[9] && sT == Colliders[12])   ||
            (fT == Colliders[12] && sT == Colliders[15])  ||
            (fT == Colliders[15] && sT == Colliders[47])  ||
            (fT == Colliders[47] && sT == Colliders[50])  ||
            (fT == Colliders[50] && sT == Colliders[53])  ||
            (fT == Colliders[53] && sT == Colliders[35])  ||
            (fT == Colliders[35] && sT == Colliders[32])  ||
            (fT == Colliders[32] && sT == Colliders[29]))
        {
            performRotation(ELayer.F, clockwise);
        }

        // BACK
        else
        if ((fT == Colliders[36] && sT == Colliders[39])   ||
            (fT == Colliders[39] && sT == Colliders[42])   ||
            (fT == Colliders[42] && sT == Colliders[27])   ||
            (fT == Colliders[27] && sT == Colliders[30])   ||
            (fT == Colliders[30] && sT == Colliders[33])   ||
            (fT == Colliders[33] && sT == Colliders[51])   ||
            (fT == Colliders[51] && sT == Colliders[48])   ||
            (fT == Colliders[48] && sT == Colliders[45])   ||
            (fT == Colliders[45] && sT == Colliders[17])   ||
            (fT == Colliders[17] && sT == Colliders[14])   ||
            (fT == Colliders[14] && sT == Colliders[11])   ||
            (fT == Colliders[11] && sT == Colliders[36]))
        {
            performRotation(ELayer.B, clockwise);
        }

        // LEFT
        else
        if ((fT == Colliders[6] && sT == Colliders[3])   ||
            (fT == Colliders[3] && sT == Colliders[0])   ||
            (fT == Colliders[0] && sT == Colliders[53])  ||
            (fT == Colliders[53] && sT == Colliders[52]) ||
            (fT == Colliders[52] && sT == Colliders[51]) ||
            (fT == Colliders[51] && sT == Colliders[26]) ||
            (fT == Colliders[26] && sT == Colliders[23]) ||
            (fT == Colliders[23] && sT == Colliders[20]) ||
            (fT == Colliders[20] && sT == Colliders[42]) ||
            (fT == Colliders[42] && sT == Colliders[43]) ||
            (fT == Colliders[43] && sT == Colliders[44]) ||
            (fT == Colliders[44] && sT == Colliders[6]))
        {
            performRotation(ELayer.L, clockwise);
        }

        // RIGHT
        else 
        if ((fT == Colliders[37] && sT == Colliders[36])  ||
            (fT == Colliders[38] && sT == Colliders[37])  ||
            (fT == Colliders[8] && sT == Colliders[38])   ||
            (fT == Colliders[5] && sT == Colliders[8])    ||
            (fT == Colliders[2] && sT == Colliders[5])    ||
            (fT == Colliders[47] && sT == Colliders[2])   ||
            (fT == Colliders[46] && sT == Colliders[47])  ||
            (fT == Colliders[45] && sT == Colliders[46])  ||
            (fT == Colliders[24] && sT == Colliders[45])  ||
            (fT == Colliders[21] && sT == Colliders[24])  ||
            (fT == Colliders[18] && sT == Colliders[21])  ||
            (fT == Colliders[36] && sT == Colliders[18]))
        {
            performRotation(ELayer.R, clockwise);
        }

        // UP
        else
        if ((fT == Colliders[29] && sT == Colliders[28])   ||
            (fT == Colliders[28] && sT == Colliders[27])   ||
            (fT == Colliders[27] && sT == Colliders[20])   ||
            (fT == Colliders[20] && sT == Colliders[19])   ||
            (fT == Colliders[19] && sT == Colliders[18])   ||
            (fT == Colliders[18] && sT == Colliders[11])   ||
            (fT == Colliders[11] && sT == Colliders[10])   ||
            (fT == Colliders[10] && sT == Colliders[9])    ||
            (fT == Colliders[9] && sT == Colliders[8])     ||
            (fT == Colliders[8] && sT == Colliders[7])     ||
            (fT == Colliders[7] && sT == Colliders[6])     ||
            (fT == Colliders[6] && sT == Colliders[29]))
        {
            performRotation(ELayer.U, clockwise);
        }

        // DOWN
        else 
        if ((fT == Colliders[0] && sT == Colliders[1])   ||
            (fT == Colliders[1] && sT == Colliders[2])   ||
            (fT == Colliders[2] && sT == Colliders[15])  ||
            (fT == Colliders[15] && sT == Colliders[16]) ||
            (fT == Colliders[16] && sT == Colliders[17]) ||
            (fT == Colliders[17] && sT == Colliders[24]) ||
            (fT == Colliders[24] && sT == Colliders[25]) ||
            (fT == Colliders[25] && sT == Colliders[26]) ||
            (fT == Colliders[26] && sT == Colliders[33]) ||
            (fT == Colliders[33] && sT == Colliders[34]) ||
            (fT == Colliders[34] && sT == Colliders[35]) ||
            (fT == Colliders[35] && sT == Colliders[0]))
        {
            performRotation(ELayer.D, clockwise);
        }

        // EQUATORIAL
        else
        if ((fT == Colliders[3] && sT == Colliders[4])  ||
            (fT == Colliders[4] && sT == Colliders[5])   ||
            (fT == Colliders[5] && sT == Colliders[12])  ||
            (fT == Colliders[12] && sT == Colliders[13]) ||
            (fT == Colliders[13] && sT == Colliders[14]) ||
            (fT == Colliders[14] && sT == Colliders[21]) ||
            (fT == Colliders[21] && sT == Colliders[22]) ||
            (fT == Colliders[22] && sT == Colliders[23]) ||
            (fT == Colliders[23] && sT == Colliders[30]) ||
            (fT == Colliders[30] && sT == Colliders[31]) ||
            (fT == Colliders[31] && sT == Colliders[32]) ||
            (fT == Colliders[32] && sT == Colliders[3]))
        {
            performRotation(ELayer.E, clockwise);
        }

        // MIDDLE
        else 
        if ((fT == Colliders[7] && sT == Colliders[4])   ||
            (fT == Colliders[4] && sT == Colliders[1])   ||
            (fT == Colliders[1] && sT == Colliders[50])  ||
            (fT == Colliders[50] && sT == Colliders[49]) ||
            (fT == Colliders[49] && sT == Colliders[48]) ||
            (fT == Colliders[48] && sT == Colliders[25]) ||
            (fT == Colliders[25] && sT == Colliders[22]) ||
            (fT == Colliders[22] && sT == Colliders[19]) ||
            (fT == Colliders[19] && sT == Colliders[39]) ||
            (fT == Colliders[39] && sT == Colliders[40]) ||
            (fT == Colliders[40] && sT == Colliders[41]) ||
            (fT == Colliders[41] && sT == Colliders[7]))
        {
            performRotation(ELayer.M, clockwise);        
        }

        // S(ide middle?) (Standing)
        else 
        if ((fT == Colliders[43] && sT == Colliders[40])   ||
            (fT == Colliders[40] && sT == Colliders[37])   ||
            (fT == Colliders[37] && sT == Colliders[10])   ||
            (fT == Colliders[10] && sT == Colliders[13])   ||
            (fT == Colliders[13] && sT == Colliders[16])   ||
            (fT == Colliders[16] && sT == Colliders[46])   ||
            (fT == Colliders[46] && sT == Colliders[49])   ||
            (fT == Colliders[49] && sT == Colliders[52])   ||
            (fT == Colliders[52] && sT == Colliders[34])   ||
            (fT == Colliders[34] && sT == Colliders[31])   ||
            (fT == Colliders[31] && sT == Colliders[28])   ||
            (fT == Colliders[28] && sT == Colliders[43]))
        {
            performRotation(ELayer.S, clockwise);
        }

        // If reached, NOT CLOCKWISE! SWAP DIRECTIONS
        else if (clockwise)
        {
            checkSwipe(sT, fT, false);
        }
    }

    void performRotation(ELayer aLayer, bool clockwise)
    {
        incMoveCounter();
        actualCube.GetComponent<LayerRotate>().RotateLayer(aLayer, clockwise);
    }

    void incMoveCounter()
    {
        textMeshPro.incCounter();
    }

    void setMoveCounter(int numMoves)
    {
        textMeshPro.setCounter(numMoves);
    }
}
