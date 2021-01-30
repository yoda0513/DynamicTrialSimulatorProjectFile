using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/*
 * Histogramオブジェクト...このコンポーネントを取り付けたオブジェクトでCanvasコンポーネントを持っている。
 * 
 * 度数分布とラインを生成し、カメラで撮影したものをリアルタイムで800×500のRendererTextureに焼き付ける。
 * そのためCanvasの要素の座標範囲は中心０、右上が(400,250)になるのに対して、 LineRendererのようなグローバル座標を用いるものは、中心(0,0)
 * で、右上が(8,5)となる。
 * 
 * 
 * 関連クラス
 * HistogramFunction..まとめているだけ
 * Distribution...度数分布表オブジェクト
 * 
 */




[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
public class Histogram : MonoBehaviour
{
    public GameObject HistgramBar;
    public GameObject number;
    public GameObject BinomialBar;

    //描画するときの設定。描画範囲に影響する。
    float barWidth = 0;
    float barHeightPerValue = 0;

    List<GameObject> bars = new List<GameObject>();

    public int minHorizonValue = 1;


    private void Start()
    {
        resetHist();

        var random = SceneChange.getSceneComponent<RandomManager>(SceneChange.mainSceneName);
        if (random.nowSimulationMode == RandomManager.SimulationMode.Coin)
        {
            setHist(2, 600);
        }
        else if(random.nowSimulationMode == RandomManager.SimulationMode.Dice)
        {
            setHist(6, 200);
        }
    }

    public void resetHist()　//Histogramのリセット。全てのオブジェクトを削除する。
    {
        barWidth = 0;
        barHeightPerValue = 0;
        bars = new List<GameObject>();

        foreach (Transform n in this.transform)
        {
            GameObject.Destroy(n.gameObject);
        }
    }

    //ヒストグラムの縦棒の数と、一つのバーがとることができる最大数を決める（描画範囲が決まる。）
    public void setHist(int BarCount, float maxValue, bool binomial =false,int minHorizon = 1)
    {
        resetHist();

        

        //情報を計算するーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        barWidth = (float)650 / (float)BarCount;
        if (barWidth >= 100) barWidth = 100;

        barHeightPerValue = (float)420 / (float)maxValue; //一度あたりの描画する高さ

        float space = (650 - (barWidth*BarCount)) / (BarCount - 1);  //バーとバーの間隔の計算
        if (space >= 300) space = 300;

        minHorizonValue = minHorizon;

        //コンテンツを設定するーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        for (int i = 0; i < BarCount; i++)
        {
            GameObject bar = HistgramBar;
            if (binomial) bar = BinomialBar;
            Vector2 size = new Vector2(barWidth, 0);
            Vector3 position = new Vector3((-300 + (barWidth / 2) + (barWidth + space)* i), -200, 0);
            GameObject x = rectInstantiate(bar, size, position);
            bars.Add(x);

            if(!binomial) x.GetComponent<HistogramBar>().setText(0.ToString());
        }

        //度数を表示ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        if (!binomial)
        {
            for (int i = 0; i < BarCount; i++)
            {
                rectInstantiate(number,
                    new Vector2(1, 1),
                    new Vector3(-300 + (barWidth / 2) + (barWidth + space) * i, -219, 1),
                    (minHorizonValue+ i).ToString());
            }
        }
        else
        {
            for (int i = 0; i < BarCount; i++)
            {
                if(i % 5 == 0)
                {
                    rectInstantiate(number,
                    new Vector2(1, 1),
                    new Vector3(-300 + (barWidth / 2) + (barWidth + space) * i, -219, 1),
                    (minHorizonValue + i).ToString());
                }
                
            }
        }
        



    }

    public void addValue(int value, bool bino = false) //value値ののバーの高さを１つ増やす
    {
        float maxhorizonvalue = minHorizonValue + bars.Count - 1;

        if((value >= minHorizonValue)&&(maxhorizonvalue >= value))
        {
            int index = value - minHorizonValue;

            var trans = bars[index].GetComponent<RectTransform>();
            trans.sizeDelta = new Vector3(trans.sizeDelta.x, (trans.sizeDelta.y) + (barHeightPerValue));

            var bar = bars[index].GetComponent<HistogramBar>();
            if(!bino) bar.setText((int.Parse(bar.UIText.text) + 1).ToString());

        }

       

    }




    private GameObject rectInstantiate(GameObject x, Vector2 deltaSize, Vector3 localPosition , string text = "")
    {
        GameObject instance = Instantiate(x);
        SceneManager.MoveGameObjectToScene(instance, SceneManager.GetSceneByName("Histogram"));
        instance.transform.SetParent(this.transform);
        RectTransform barTransform = instance.GetComponent<RectTransform>();


        barTransform.sizeDelta = deltaSize;
        barTransform.localPosition = localPosition;
        barTransform.localScale = new Vector3(1, 1, 1);

        if(instance.GetComponent<Text>() != null)
        {
            instance.GetComponent<Text>().text = text;
        }

        return instance;
    }

    


    

    
}











    

