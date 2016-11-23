using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class Loading : MonoBehaviour
{

    public string sceneName;
    public float waitTime;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
