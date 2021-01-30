using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightBar : MonoBehaviour
{
    public RandomManager randomManager;
    public List<Bubble> bubbleList = new List<Bubble>();

    public List<Image> buttonImage = new List<Image>();

    public Transform averageWindow;
    public Text switchText;

    public GameObject optionPanel;

    public GameObject AverageView;
    public GameObject VaraianceView;

    public Toggle Averagetoggle;
    public Toggle Varaiancetoggle;

    public GameObject BinomialOption;
    public Slider binomialSlider;
    public InputField BinomialCountText;

    public GameObject BinomialView;
    public Text BinomialText;

    public bool isCountChanged = false;

    private void Start()
    {
        BinomialOption.SetActive(false);
        BinomialView.SetActive(false);

        buttonImage[1].color = Color.white;
        buttonImage[0].color = Color.gray;

        changeBinomialCount();
        BinomialText.text = binomialSlider.value.ToString();
    }

    public void PressCoin()
    {
        if (!randomManager.nowSampling)
        {
            if (randomManager.nowSimulationMode != RandomManager.SimulationMode.Coin)
            {
                BinomialOption.SetActive(false);
                BinomialView.SetActive(false);

                buttonImage[2].color = Color.white;
                buttonImage[1].color = Color.white;
                buttonImage[0].color = Color.gray;
                randomManager.nowSimulationMode = RandomManager.SimulationMode.Coin;
                randomManager.operationWindow.resetSimulation();
            }
        }
        
    }

    public void PressDice()
    {
        if (!randomManager.nowSampling)
        {
            if (randomManager.nowSimulationMode != RandomManager.SimulationMode.Dice)
            {
                BinomialOption.SetActive(false);
                BinomialView.SetActive(false);

                buttonImage[2].color = Color.white;
                buttonImage[0].color = Color.white;
                buttonImage[1].color = Color.gray;
                randomManager.nowSimulationMode = RandomManager.SimulationMode.Dice;
                randomManager.operationWindow.resetSimulation();
            }
        }
        
    }

    public void PressBinomial()
    {
        if (!randomManager.nowSampling)
        {
            if (randomManager.nowSimulationMode != RandomManager.SimulationMode.Binomial)
            {
                BinomialOption.SetActive(true);
                BinomialView.SetActive(true);

                buttonImage[0].color = Color.white;
                buttonImage[1].color = Color.white;
                buttonImage[2].color = Color.gray;
                randomManager.nowSimulationMode = RandomManager.SimulationMode.Binomial;
                randomManager.operationWindow.resetSimulation();
            }
        }
    }

    public void PressInformationButton()
    {
        if (bubbleList[0].IsView)
        {
            foreach (var i in bubbleList)
            {
                i.IsView = false;
            }
        }
        else
        {
            foreach (var i in bubbleList)
            {
                i.IsView = true;
            }
        }
    }

    public void PressOptioneButton()
    {
        if (optionPanel.activeSelf)
        {
            optionPanel.SetActive(false);
        }
        else
        {
            optionPanel.SetActive(true);
        }
    }

    public void switchDisplay()
    {
        if(averageWindow.GetSiblingIndex() == 1)
        {
            averageWindow.SetSiblingIndex(2);
            switchText.text = "標準偏差";
        }
        else
        {
            averageWindow.SetSiblingIndex(1);
            switchText.text = "平均";
        }
    }

    public void switchAverage()
    {
        AverageView.SetActive(Averagetoggle.isOn);
    }

    public void switchVaraiance()
    {
        VaraianceView.SetActive(Varaiancetoggle.isOn);
    }
    public void changeBinomialCount()
    {
        isCountChanged = true;
        BinomialCountText.text = binomialSlider.value.ToString();
    }
}
