using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class EnableMasterCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void loadMasterCube()
    {
        SceneManager.LoadScene("RubiksCube - MasterCube");
    }
}
