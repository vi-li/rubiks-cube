using System.Collections;
using UnityEngine;

public class MasterCube : MonoBehaviour
{
    public float seconds = 0.5;
    private bool ready = false;
    private GameObject[] outerCubes;

    // Start is called before the first frame update
    void Start()
    {
        if (outerCubes == null) { outerCubes = GameObject.FindGameObjectsWithTag("outerCubes"); }
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
                
                Physics.Raycast(ray, out hit, 100.0f);

                if (ready && hit.transform.gameObject.tag == "masterCube")
                {
                    startFourDots();
                }
            }
        }
    }

    void onClick() 
    {
        ready = !ready;
    }

    void startFourDots() 
    {
        Script masterLayerRotate = transform.Find("Cube Case").transform.Find("Actual Cube").GetComponent<LayerRotate>;

        StartCoroutine(outerFourDots);
        
        StartCoroutine(performFourDots(masterLayerRotate));
    }

    IEnumerable performFourDots(Script layerRotateS)
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
    }

    IEnumerator outerFourDots()
    {
        yield return new WaitForSeconds(seconds);
        foreach (GameObject outerCube in outerCubes)
        {
            // TODO: TEST THIS, does it do all cubes at once? Or one at a time? Or idk this seems weird
            Script layerRotateS = outerCube.GetComponent<LayerRotate>();
            StartCoroutine(performFourDots(layerRotateS));
        }
    }

}
