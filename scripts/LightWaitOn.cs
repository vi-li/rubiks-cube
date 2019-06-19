using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightWaitOn : MonoBehaviour
{
    public float Seconds = 15f;

    // Start is called before the first frame update
    void Start()
    {
        // if (gameObject.activeInHierarchy)
        //      gameObject.SetActive(false);
 
         StartCoroutine(WaitLight());
    }

    IEnumerator WaitLight()
     {
 
         yield return new WaitForSeconds(Seconds);
 
         gameObject.SetActive(true);
         GetComponent<Light>().enabled = true;
     }
    // Update is called once per frame
    void Update()
    {
        
    }
}
