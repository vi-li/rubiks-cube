using System.Collections;
using UnityEngine;
using System.Linq;

/*
Discontinued at the moment.
*/
public class MasterCube : MonoBehaviour
{
    public float seconds = 0.5f;
    private bool ready = true; // TODO: PUT BACK TO FALSE
    private GameObject[] outerCubes;

    LayerRotate layerRotateS;

    // Start is called before the first frame update
    void Start()
    {
        if (outerCubes == null) { outerCubes = GameObject.FindGameObjectsWithTag("outerCube"); }
        
        layerRotateS = gameObject.AddComponent( typeof (LayerRotate) ) as LayerRotate;
    }

    // Update is called once per frame
    void Update()
    {
        if (ready)
        {
            startFourDots();
            ready = false;
        }

        // if (!ready) 
        //     return;

        // if (Input.touchCount > 0)
        // {
        //     Touch[] myTouches = Input.touches;

        //     for (int i = 0; i < Input.touchCount; i++)
        //     {
        //         Ray ray = Camera.main.ScreenPointToRay(myTouches[i].position);
        //         RaycastHit hit;
                
        //         Physics.Raycast(ray, out hit, 100.0f);

        //         if (hit.transform.gameObject.tag == "masterCube")
        //         {
        //             startFourDots();
        //             ready = false;
        //         }
        //     }
        // }
    }

    void onClick() 
    {
        ready = !ready;
    }

    void startFourDots() 
    {
        layerRotateS = transform.Find("Cube Case").transform.Find("Actual Cube").GetComponent<LayerRotate>();
        StartCoroutine(outerFourDots());
        StartCoroutine(performFourDots(layerRotateS));
    }

    IEnumerator performFourDots(LayerRotate layerRotateS)
    {
        // Blue 2 times
        foreach (var i in Enumerable.Range(0, 2))
        {
            layerRotateS.RotateLayer(ELayer.R, true);
        }

        // Green 2 times
        foreach (var i in Enumerable.Range(0, 2))
        {
            layerRotateS.RotateLayer(ELayer.L, true);
        }

        // Yellow 1 time
        layerRotateS.RotateLayer(ELayer.D, true);

        // White 3 times
        foreach (var i in Enumerable.Range(0, 3))
        {
            layerRotateS.RotateLayer(ELayer.U, true);
        }

        // Red 2 times
        foreach (var i in Enumerable.Range(0, 2))
        {
            layerRotateS.RotateLayer(ELayer.F, true);
        }

        // Orange 2 times
        foreach (var i in Enumerable.Range(0, 2))
        {
            layerRotateS.RotateLayer(ELayer.B, true);
        }

        // Yellow 1 time
        layerRotateS.RotateLayer(ELayer.D, true);

        // White 3 times
        foreach (var i in Enumerable.Range(0, 3))
        {
            layerRotateS.RotateLayer(ELayer.U, true);
        }

        return null;
    }

    IEnumerator outerFourDots()
    {
        // Does this pause the operation of whole program? or just the outer cubes?
        yield return new WaitForSeconds(seconds);   
        foreach (GameObject outerCube in outerCubes)
        {
            Debug.Log(outerCube.transform.name);
            // TODO: TEST THIS, does it do all cubes at once? Or one at a time? Or idk this seems weird
            LayerRotate layerRotateSO = outerCube.transform.Find("Cube Case").transform.Find("Actual Cube").GetComponent<LayerRotate>();
            StartCoroutine(performFourDots(layerRotateS));
        }
    }

}
