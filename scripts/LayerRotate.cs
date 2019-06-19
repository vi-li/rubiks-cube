using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ELayer
{
    F, // front face
    R, // right face
    U, // upper face
    L, // left face
    B, // back face
    D, // down face
    M, // middle layer
    E, // middle layer
    S // middle layer
}
 
 
public struct CubeLayer
{
    public Transform center;
    public Transform c0, c1, c2, c3, c4, c5, c6, c7;
    public Vector3 normal;
    public CubeLayer(Transform aCenter, Transform aC0, Transform aC1, Transform aC2, Transform aC3, 
                     Transform aC4, Transform aC5, Transform aC6, Transform aC7, Vector3 aNormal)
    {
        m_Index = -1;
        center = aCenter;
        normal = aNormal;
        c0 = aC0; c1 = aC1; c2 = aC2; c3 = aC3;
        c4 = aC4; c5 = aC5; c6 = aC6; c7 = aC7;
    }
    public CubeLayer RotateCW()                                                 // c0 c1 c2 --> c6 c7 c0
    {                                                                           // c7 C  c3 --> c5 C  c1
        return new CubeLayer(center, c6, c7, c0, c1, c2, c3, c4, c5, normal);   // c6 c5 c4 --> c4 c3 c2
    }
    public CubeLayer RotateCCW()                                                // c0 c1 c2 --> c2 c3 c4
    {                                                                           // c7 C  c3 --> c1 C  c5
        return new CubeLayer(center, c2, c3, c4, c5, c6, c7, c0, c1, normal);   // c6 c5 c4 --> c0 c7 c6
    }
    public Transform this[int index]
    {
        get
        {
            switch (index)
            {
                case 0: return center;
                case 1: return c0;
                case 2: return c1;
                case 3: return c2;
                case 4: return c3;
                case 5: return c4;
                case 6: return c5;
                case 7: return c6;
                case 8: return c7;
                default: return null;
            }
        }
        set
        {
            switch (index)
            {
                case 0: center = value; return;
                case 1: c0 = value; return;
                case 2: c1 = value; return;
                case 3: c2 = value; return;
                case 4: c3 = value; return;
                case 5: c4 = value; return;
                case 6: c5 = value; return;
                case 7: c6 = value; return;
                case 8: c7 = value; return;
            }
        }
    }
 
    // enumerable implementation
    private int m_Index;
    public CubeLayer GetEnumerator()
    {
        Reset();
        return this;
    }
    public bool MoveNext()
    {
        return ++m_Index < 9;
    }
    public Transform Current
    {
        get { return this[m_Index]; }
    }
    public void Reset()
    {
        m_Index = -1;
    }
}
 
 
public class LayerRotate : MonoBehaviour
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
     *   c0---c1---c2       Front in this POV is RED.
     *
     * */
 
    public Transform[] Cubes;
    public Transform rotatePivot;
    public GameObject cubeCase;
    public GameObject largeCollider;
    private bool m_Rotating = false;
 
    public CubeLayer this[ELayer layer]
    {
        get {
            var c = Cubes;
            switch (layer)
            {
                default:                                                                                                            // Old non-functional directions:
                case ELayer.F: return new CubeLayer(c[10], c[ 0], c[ 9], c[18], c[19], c[20], c[11], c[ 2], c[ 1], Vector3.right);      //Vector3.forward
                case ELayer.R: return new CubeLayer(c[14], c[ 2], c[11], c[20], c[23], c[26], c[17], c[ 8], c[ 5], Vector3.forward);    //-Vector3.right
                case ELayer.U: return new CubeLayer(c[22], c[18], c[21], c[24], c[25], c[26], c[23], c[20], c[19], -Vector3.up);
                case ELayer.L: return new CubeLayer(c[12], c[ 6], c[15], c[24], c[21], c[18], c[ 9], c[ 0], c[ 3], -Vector3.forward);   //Vector3.right
                case ELayer.B: return new CubeLayer(c[16], c[ 6], c[ 7], c[ 8], c[17], c[26], c[25], c[24], c[15], -Vector3.right);     //-Vector3.forward
                case ELayer.D: return new CubeLayer(c[ 4], c[ 0], c[ 1], c[ 2], c[ 5], c[ 8], c[ 7], c[ 6], c[ 3], Vector3.up);
                case ELayer.M: return new CubeLayer(c[13], c[19], c[10], c[ 1], c[ 4], c[ 7], c[16], c[25], c[22],  -Vector3.forward);  //Vector3.right
                case ELayer.E: return new CubeLayer(c[13], c[ 9], c[10], c[11], c[14], c[17], c[16], c[15], c[12], Vector3.up);
                case ELayer.S: return new CubeLayer(c[13], c[ 3], c[12], c[21], c[22], c[23], c[14], c[ 5], c[ 4], Vector3.right);      //Vector3.forward
            }
        }
        set
        {
            var c = Cubes;
            var v = value;
            switch(layer)
            {
                case ELayer.F:
                    c[10] = v.center;
                    c[0] = v.c0; c[9] = v.c1; c[18] = v.c2; c[19] = v.c3;
                    c[20] = v.c4; c[11] = v.c5; c[2] = v.c6; c[1] = v.c7;
                    break;
                case ELayer.R:
                    c[14] = v.center;
                    c[ 2] = v.c0; c[11] = v.c1; c[20] = v.c2; c[23] = v.c3;
                    c[26] = v.c4; c[17] = v.c5; c[8] = v.c6; c[5] = v.c7;
                    break;
                case ELayer.U:
                    c[22] = v.center;
                    c[18] = v.c0; c[21] = v.c1; c[24] = v.c2; c[25] = v.c3;
                    c[26] = v.c4; c[23] = v.c5; c[20] = v.c6; c[19] = v.c7;
                    break;
                case ELayer.L:
                    c[12] = v.center;
                    c[ 6] = v.c0; c[15] = v.c1; c[24] = v.c2; c[21] = v.c3;
                    c[18] = v.c4; c[9] = v.c5; c[0] = v.c6; c[3] = v.c7;
                    break;
                case ELayer.B:
                    c[16] = v.center;
                    c[ 6] = v.c0; c[7] = v.c1; c[8] = v.c2; c[17] = v.c3;
                    c[26] = v.c4; c[25] = v.c5; c[24] = v.c6; c[15] = v.c7;
                    break;
                case ELayer.D:
                    c[ 4] = v.center;
                    c[ 0] = v.c0; c[1] = v.c1; c[2] = v.c2; c[5] = v.c3;
                    c[8] = v.c4; c[7] = v.c5; c[6] = v.c6; c[3] = v.c7;
                    break;
                case ELayer.M:
                    c[13] = v.center;
                    c[19] = v.c0; c[10] = v.c1; c[1] = v.c2; c[4] = v.c3;
                    c[7] = v.c4; c[16] = v.c5; c[25] = v.c6; c[22] = v.c7;
                    break;
                case ELayer.E:
                    c[13] = v.center;
                    c[ 9] = v.c0; c[10] = v.c1; c[11] = v.c2; c[14] = v.c3;
                    c[17] = v.c4; c[16] = v.c5; c[15] = v.c6; c[12] = v.c7;
                    break;
                case ELayer.S:
                    c[13] = v.center;
                    c[ 3] = v.c0; c[12] = v.c1; c[21] = v.c2; c[22] = v.c3;
                    c[23] = v.c4; c[14] = v.c5; c[5] = v.c6; c[4] = v.c7;
                    break;
            }
        }
    }
 
    private void Start()
    {
        if (rotatePivot == null)
        {
            rotatePivot = new GameObject("rotate pivot").transform;
            rotatePivot.SetParent(transform, true);
        }
    }
 
    private void Update()
    {
        // if (m_Rotating)  // Commented due to duplicate below
        //     return;
        bool counterclockwise = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        if (Input.GetKeyDown(KeyCode.F)) RotateLayer(ELayer.F, !counterclockwise);
        else if (Input.GetKeyDown(KeyCode.B)) RotateLayer(ELayer.B, !counterclockwise);
        else if (Input.GetKeyDown(KeyCode.L)) RotateLayer(ELayer.L, !counterclockwise);
        else if (Input.GetKeyDown(KeyCode.R)) RotateLayer(ELayer.R, !counterclockwise);
        else if (Input.GetKeyDown(KeyCode.U)) RotateLayer(ELayer.U, !counterclockwise);
        else if (Input.GetKeyDown(KeyCode.D)) RotateLayer(ELayer.D, !counterclockwise);
        else if (Input.GetKeyDown(KeyCode.M)) RotateLayer(ELayer.M, !counterclockwise);
        else if (Input.GetKeyDown(KeyCode.E)) RotateLayer(ELayer.E, !counterclockwise);
        else if (Input.GetKeyDown(KeyCode.S)) RotateLayer(ELayer.S, !counterclockwise);
    }
 
    public void RotateLayerCW(ELayer aLayer)
    {
        var layer = this[aLayer];
        this[aLayer] = layer.RotateCW();
        StartCoroutine(RotateLayer(layer, -90f, 4f));
    }
    public void RotateLayerCCW(ELayer aLayer)
    {
        var layer = this[aLayer];
        this[aLayer] = layer.RotateCCW();
        StartCoroutine(RotateLayer(layer, 90f, 4f));
    }
    public void RotateLayer(ELayer aLayer, bool aClockwise)
    {
        if (m_Rotating || 
            largeCollider.GetComponent<MultiTouchRubiksRotate>().m_largeRotating ||
            largeCollider.GetComponent<MultiTouchRubiksRotate>().m_startedOnTable)
        {
            //Debug.Log("Cancelled!");
            return;         // TODO: FOR TOUCH INTERFERENCE
        }

        if (aClockwise)
            RotateLayerCW(aLayer);
        else
            RotateLayerCCW(aLayer);
    }
 
    IEnumerator RotateLayer(CubeLayer aLayer, float aDegree, float aSpeed)
    {
        // mutex of sorts
        m_Rotating = true;

        //rotatePivot.localPosition = Vector3.zero;
        rotatePivot.localRotation = Quaternion.identity;

        // Set each layer's cube parent to rotation pivot
        foreach (Transform t in aLayer)
            t.parent = rotatePivot;
        
        // Set angle to rotate and do rotation
        Quaternion target = Quaternion.AngleAxis(aDegree, aLayer.normal);
        for (float t = 0f; t <= 1f; t += aSpeed * Time.deltaTime)
        {
            rotatePivot.localRotation = Quaternion.Slerp(Quaternion.identity, target, t);
            yield return null;
        }
        // Chain rotation
        rotatePivot.localRotation = target;
        // Reset parent back to whole cube
        foreach (Transform t in aLayer)
            t.parent = transform;
        m_Rotating = false;
    }
}