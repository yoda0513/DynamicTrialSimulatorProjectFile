using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GraphPlot : MonoBehaviour
{
    public LayerMask layer;

    public GameObject dot;
    public GameObject line;

    public float widthPerValue = 0;
    public float heightPerValue = 0;
    int minValue = 0;
    int plotCount = 0;


    private void Start()
    {
        layer = this.gameObject.layer;

        var random = SceneChange.getSceneComponent<RandomManager>(SceneChange.mainSceneName);
        if (random.nowSimulationMode == RandomManager.SimulationMode.Coin)
        {
            if(this.gameObject.name == "GraphPlot1")
            {
                setGraph(1000, 1,2);
            }
            else if (this.gameObject.name == "GraphPlot2")
            {
                setGraph(1000, 0,2);
            }
            
        }
        else
        {
            if (this.gameObject.name == "GraphPlot1")
            {
                setGraph(1000, 1, 6);
            }
            else if (this.gameObject.name == "GraphPlot2")
            {
                setGraph(1000, 1, 2);
            }
        }


    }

    //横幅はx=-330からx=370までで描画する。
    //盾幅はy=-200からy=200まで描画する。
    public void setGraph(int horizonCount, int minValue, int maxValue)
    {
        int verticalCount = maxValue - minValue + 1;

        resetGraph();
        widthPerValue = (float)700 / (float)horizonCount;
        heightPerValue = 400 / (verticalCount - 1);
        this.minValue = minValue;

        //ラインと線を引く
        for(int i = 0; i<verticalCount; i++)
        {
            var x = rectInstantiate(
                line,
                new Vector2(100, 100),
                new Vector3(0, -200 + heightPerValue * i, 26));

            x.GetComponent<Line>().setText((minValue + i).ToString());
            x.layer = this.layer;
        }
    }

    public void plot(float value)
    {
        float delta = value - minValue + 1;


        float x = -330 + widthPerValue * plotCount;
        float y = -200 + heightPerValue * (delta-1);

        if(x <= 370)
        {
            var i =rectInstantiate(
                dot,
                new Vector2(15, 15),
                new Vector3(x, y, 0));

            i.layer = this.layer;
        }

        plotCount++;
    }

    [ContextMenu("reset")]
    void resetGraph()
    {
        plotCount = 0;
        widthPerValue = 0;
        heightPerValue = 0;
        minValue = 0;

        foreach (Transform n in this.transform)
        {
            GameObject.Destroy(n.gameObject);
        }
    }



    private GameObject rectInstantiate(GameObject x, Vector2 deltaSize , Vector3 localPosition, string text = "")
    {
        GameObject instance = Instantiate(x);
        SceneManager.MoveGameObjectToScene(instance, SceneManager.GetSceneByName("GraphPlot"));
        instance.transform.SetParent(this.transform);
        RectTransform barTransform = instance.GetComponent<RectTransform>();


        barTransform.sizeDelta = deltaSize;
        barTransform.localPosition = localPosition;
        barTransform.localScale = new Vector3(1, 1, 1);

        if (instance.GetComponent<Text>() != null)
        {
            instance.GetComponent<Text>().text = text;
        }

        return instance;
    }




    [ContextMenu("set")]
    void test()
    {
        setGraph(100, 1,3);
    }

    [ContextMenu("plot")]
    void test2()
    {
        plot(0.5f);
        
    }
}
