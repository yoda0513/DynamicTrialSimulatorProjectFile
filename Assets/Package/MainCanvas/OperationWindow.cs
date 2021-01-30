using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * ➀UIのボタンのような操作の管理 
 * 
 * 
 */

public class OperationWindow : MonoBehaviour
{
    internal RandomManager randomManager;
    public Slider speedSlider;
    public Toggle autoToggle;
    public Text ButtonText;
 

    

    void Start()
    {

        ButtonText.text = "開始";
        randomManager = SceneChange.getSceneComponent<RandomManager>(SceneChange.mainSceneName);

        

        changeSpeed();
    }



    public void SimulationStartButton()
    {
        if(randomManager.data.Count < randomManager.maxSampleSize)
        {
            if (!randomManager.nowSampling) //スタート
            {
                randomManager.Reset.interactable = false;
                randomManager.rightBar.binomialSlider.interactable = false;

                if (randomManager.nowSimulationMode == RandomManager.SimulationMode.Coin)
                {
                    randomManager.Dice.interactable = false;
                    randomManager.Binomial.interactable = false;
                }
                else if(randomManager.nowSimulationMode == RandomManager.SimulationMode.Dice)
                {
                    randomManager.Coin.interactable = false;
                    randomManager.Binomial.interactable = false;
                }else if(randomManager.nowSimulationMode == RandomManager.SimulationMode.Binomial)
                {
                    if (randomManager.rightBar.isCountChanged) resetSimulation();

                    randomManager.Coin.interactable = false;
                    randomManager.Dice.interactable = false;
                }



                if (randomManager.AutoMode) ButtonText.text = "終了";

                StartCoroutine(randomManager.AutoSamplingStart(randomManager.nowSimulationMode));
            }
            else //中断
            {
                randomManager.rightBar.binomialSlider.interactable = true;

                randomManager.Reset.interactable = true;
                randomManager.Dice.interactable = true;
                randomManager.Coin.interactable = true;
                randomManager.Binomial.interactable = true;

                ButtonText.text = "開始";
                randomManager.nowSampling = false;
            }
        }
        
    }

    public void resetSimulation()
    {
        if (!randomManager.nowSampling)
        {
            randomManager.rightBar.isCountChanged = false;
            foreach (Transform i in randomManager.CoinGrid.transform)
            {
                Destroy(i.gameObject);
            }

            if (randomManager.nowSimulationMode == RandomManager.SimulationMode.Coin)
            {
                randomManager.hist.setHist(2, 600);
                randomManager.graphs[0].setGraph(1000, 1, 2);
                randomManager.graphs[1].setGraph(1000, 0, 2);
            }
            else if(randomManager.nowSimulationMode == RandomManager.SimulationMode.Dice)
            {
                randomManager.hist.setHist(6, 200);
                randomManager.graphs[0].setGraph(1000, 1, 6);
                randomManager.graphs[1].setGraph(1000, 1, 2);
            }
            else
            {
                randomManager.rightBar.BinomialText.text = randomManager.rightBar.binomialSlider.value.ToString();
                randomManager.hist.setHist(69, 400 ,true,0);

                float x = (randomManager.rightBar.binomialSlider.value) * 0.5f;
                float y = Mathf.Sqrt(x * 0.5f);

                randomManager.graphs[0].setGraph(1000, (int)x-3, (int)x+3);
                randomManager.graphs[1].setGraph(1000, (int)y-2, (int)y+2);

                for(int i=0;i< randomManager.rightBar.binomialSlider.value; i++)
                {
                    GameObject child = Instantiate(randomManager.coinPrefab, randomManager.CoinGrid.transform);
                    //child.transform.SetParent(randomManager.CoinGrid.transform);
                }
            }



            randomManager.nowSampling = false;
            randomManager.resetSimulation();
        }
    }

    public void changeSpeed()
    {
        randomManager.RotationSpeed = (speedSlider.maxValue - speedSlider.value);
    }

    public void changeAutoSwitch()
    {
        randomManager.AutoMode = autoToggle.isOn;
    }
}
