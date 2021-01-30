using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.UI;

/*
 * 二項分布生成シミュレーション。
 * 現状、1/2（コイン）と1/6(サイコロ)の二項分布シミュレーションを想定している。
 * 
 * 内容としては、
 * 
 *
 */


public class RandomManager : MonoBehaviour
{
    //参照
    public DataDisplay dataDisplay;
    public OperationWindow operationWindow;

    public List<int> data = new List<int>();

    public List<GraphPlot> graphs = new List<GraphPlot>();
    public RightBar rightBar;

    public Button Reset;
    public Button Coin;
    public Button Dice;
    public Button Binomial;

    public GameObject CoinGrid;
    public GameObject coinPrefab;

    public Sprite head;
    public Sprite tail;

  
    public enum SimulationMode
    {
        Dice,
        Coin,
        Binomial
    }

    [SerializeField]
    internal SimulationMode nowSimulationMode = SimulationMode.Dice;
    [SerializeField]
    internal bool AutoMode = true; //trueの場合、連続でカウントし続ける。
    public int maxSampleSize = 1000;  //最大のサンプル数。設定の値に到達すると、サンプルができなくなる。
    public float RotationSpeed = 0.5f; 

    internal bool nowSampling = false; //サンプル中はTrue

    public Histogram hist;


   




    void Start()
    {
        operationWindow.changeAutoSwitch();
        
        setFirstStat();//起動時の状態にする

        foreach(Transform i in CoinGrid.transform)
        {
            Destroy(i.gameObject);
        }

    }

  











    //関数～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～

    //主要関数～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～

    //一回サンプリングを行う補助関数。
    //AutoSamplingStartとOneSamplingStartで使用される。 
    //x～yの値が実装される
    private void sampling(int x,int y) 
    {
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
        int sample = UnityEngine.Random.Range(x, y + 1);
        hist.addValue(sample);
        

        data.Add(sample);
    }

    private void BinomialSampling(int n)
    {
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
        int sample = 0;

        for (int i = 0; i < n; i++)
        {
            int x = UnityEngine.Random.Range(0, 2);
            if(x == 0)
            {
                if (x == 0) sample++;
                CoinGrid.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = head;
            }
            else
            {
                CoinGrid.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = tail;
            }
            

        }

        hist.addValue(sample, true);

        data.Add(sample);


    }



    //連続でサンプリングを行う。 magnitudeに0～1の値をいれて、その値に応じてスピードが決まる。
    //nowSamplingをfalseにすれば停止する。
    public IEnumerator AutoSamplingStart( SimulationMode mode)　　　
    {
        if (AutoMode)
        {
            operationWindow.ButtonText.text = "終了";
        }




        int minValue = 1;
        int maxValue = 2;
        if(mode == SimulationMode.Dice)
        {
            minValue = 1;
            maxValue = 6;
        }





        nowSampling = true;
        int binomialCount = (int)rightBar.binomialSlider.value;

        while (nowSampling)
        {
            if (mode != SimulationMode.Binomial)
            {
                animationPane.nowAnimator.enabled = true;
                animationPane.startAnimation(); //アニメーションを開始する。
                sampling(minValue, maxValue);


                yield return new WaitForSeconds(0.4f * RotationSpeed);

                animationPane.endAnimation(); //アニメーションの停止
            }
            else
            {

                BinomialSampling(binomialCount);
                yield return new WaitForSeconds(0.4f * RotationSpeed);
            }
            
            yield return null;

            float ave = (float)Math.Round(data.Average(), 3);
            float ste = (float)Math.Round(data.Stdev<int>(), 3);

            graphs[0].plot(ave);
            graphs[1].plot(ste);

            dataDisplay.setData(data.Count, data.Sum(), ave,  ste);




            if(mode != SimulationMode.Binomial)
            {
                animationPane.nowAnimator.enabled = false;

                animationPane.setSurfaceToward(data[data.Count - 1], nowSimulationMode);

                yield return new WaitForSeconds(1.3f * RotationSpeed);
            }
            

   

            if ((data.Count >= maxSampleSize)|| (!AutoMode))
            {
                Reset.interactable = true;
                Dice.interactable = true;
                Coin.interactable = true;
                Binomial.interactable = true;

                rightBar.binomialSlider.interactable = true;


                operationWindow.ButtonText.text = "開始";
                nowSampling = false;
            }

        }
    }


















    //初期化やリセットをおこなう～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～
    private void setFirstStat() //起動時の状態にする。
    {

        nowSimulationMode = RandomManager.SimulationMode.Coin;
        AutoMode = true;
        resetSimulation();
    }


    public void resetSimulation()　//シミュレーションデータの初期化、グラフなどのリセット、サイコロオブジェクトなどの配置
    {
        data = new List<int>();
        animationPane.resetAnimationObject(nowSimulationMode);
        dataDisplay.setData(data.Count(), data.Sum(), 0, 0);
    }




















    //各機能（ペインクラス）を取得する～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～
    public AnimationPane animationPane;
    

    


}


public static class stedv
{
    public static double Stdev<T>(this IEnumerable<T> src)
    {
        if (!src.Any()) throw new InvalidOperationException("Cannot compute median for an empty set.");
        //Doubleにキャストして処理を進める
        var doubleList = src.Select(a => Convert.ToDouble(a)).ToArray();

        //平均値算出
        double mean = doubleList.Average();
        //自乗和算出
        double sum2 = doubleList.Select(a => a * a).Sum();
        //分散 = 自乗和 / 要素数 - 平均値^2
        double variance = sum2 / doubleList.Count() - mean * mean;
        //標準偏差 = 分散の平方根
        return Math.Sqrt(variance);
    }
}
