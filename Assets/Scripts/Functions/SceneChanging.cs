using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanging : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScene(int sceneID)
    {
        SceneHandler sceneHandler = GameObject.Find("SceneHandler").GetComponent<SceneHandler>();

        sceneHandler.loadNextScene(sceneID);
    }
}