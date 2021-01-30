using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * データの表示を行う
 */

public class DataDisplay : MonoBehaviour
{

    //UI
    public Text sampleCount;
    public Text Sum;
    public Text Avarage;
    public Text Varaiance;


    public void setData(int count, int sum, float average, float varaiance)
    {
        this.sampleCount.text = $"{count}";
        Sum.text = $"{sum}";
        Avarage.text = $"{average}";
        Varaiance.text = $"{varaiance}";
    }
}
