using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


//関数をまとめるだけ
public static class HistogramFunction
{
    //カメラの座標（スクリーン座標）をグローバル座標に変換する
     static Vector3 convertCoordinate(Vector3 cameraPosition)
    {
        Vector3 returnVector = new Vector3(cameraPosition.x * (8 / 400), cameraPosition.y * (5 / 250), cameraPosition.z);

        return returnVector;
    }

   


}


public class Distibution
{
    public List<float> classTable = new List<float>();  //階級を羅列したもの。階級の数は「classtableの要素-1」となる。
    public List<int> frequency = new List<int>();

    public int sampleCount = 0;
    public int classCount = 0;
    public int classWidth = 0;
    public int maxFrequency = 0;

    //コンストラクター
    public Distibution(List<float> data)
    {
        int min = (int)Math.Floor(data.Min());
        int max = (int)Math.Ceiling(data.Max()) + 1;
        sampleCount = data.Count;
        classCount = (int)Math.Ceiling(1 + Math.Log(sampleCount, 2)); //スタージェスの公式
        classWidth = (int)Math.Ceiling((float)(max - min) / (float)classCount);


        classTable.Add(min); //一番最初の行をセット

        for (int i = 1; i < classCount; i++)  //階級リストを生成
        {
            classTable.Add(classTable[i - 1] + classWidth);
        }


        for (int i = 0; i < classTable.Count - 1; i++)  //度数リストを生成
        {
            int y = 0;

            foreach (var k in data)
            {
                if ((k >= classTable[i]) && (k < classTable[i + 1]))
                {
                    y++;
                }


            }

            frequency.Add(y);
        }

        maxFrequency = frequency.Max();
    }
}




