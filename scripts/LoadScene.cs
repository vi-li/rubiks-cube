using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadMasterCube()
    {
        SceneManager.LoadScene("RubiksCubesMaster");
    }

    public void loadGroupCube()
    {
        SceneManager.LoadScene("RubiksCubesGroup");
    }
}
