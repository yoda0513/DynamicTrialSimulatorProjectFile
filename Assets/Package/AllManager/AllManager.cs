using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllManager : MonoBehaviour
{
    public RandomManager randomManager;
    void Start()
    {
        SceneChange.resetScene();
        SceneManager.sceneLoaded += SceneLoaded;
    }

    void SceneLoaded(Scene nextScene, LoadSceneMode mode)
    {
        if(nextScene.name == "Animation")
        {
            Destroy(SceneChange.getSceneObject("SampleDice", "Animation")); //デバッグ用のオブジェクトを消しておく
            Destroy(SceneChange.getSceneObject("Samplecoin", "Animation")); //デバッグ用のオブジェクトを消しておく
        }else if(nextScene.name == "Histogram")
        {
            var his = SceneChange.getSceneComponent<Histogram>(nextScene.name); ;
            randomManager.hist = his;

            
        }else if(nextScene.name == "GraphPlot")
        {
            randomManager.graphs.Add(SceneChange.getSceneObject("GraphPlot1", nextScene.name).GetComponent<GraphPlot>());
            randomManager.graphs.Add(SceneChange.getSceneObject("GraphPlot2", nextScene.name).GetComponent<GraphPlot>());
        }
    }
   
}
