using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*Animationペインの処理を行う。
 * 
 * 以下の三つの機能を外部（SimulationManager）から使用できるように、関数を公開する。
 * 
 * 機能
 * ➀アニメーションに使うオブジェクトの生成、消去、動作の管理。
 * ②再生開始
 * ③再生ストップ
 * ④指定の面をむける
 * 
 * 現状、コイン（裏表）とサイコロ(六面)の機能を実装する予定であるため、それ相応の汎用性を計算したクラスを作成していく。
 * 
 */


public class AnimationPane : MonoBehaviour
{
    public List<GameObject> animationObjects = new List<GameObject>(); //Animatorコンポーネントを持つ、GameObjectを指定する。
    internal GameObject nowAnimationObject = null;
    internal Animator nowAnimator = null;

    //データ
    public List<Vector3> DiceQuaternion = new List<Vector3>();  //０～５の順で書いていく。
    public List<Vector3> CoinQuaternion = new List<Vector3>();

    //ハッシュ
    public static int rotation = Animator.StringToHash("Rotation");


 
    //関数群～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～
    public void resetAnimationObject(RandomManager.SimulationMode mode) //AnimationObjectを指定する。
    {
        if (nowAnimationObject != null) Destroy(nowAnimationObject);

        if(mode != RandomManager.SimulationMode.Binomial)
        {
            GameObject instance = null;

            if (mode == RandomManager.SimulationMode.Dice)
            {
                instance = animationObjects[0];
                nowAnimationObject = Instantiate(instance, new Vector3(0, 0, 0), Quaternion.Euler(Vector3.zero));
            }
            else if (mode == RandomManager.SimulationMode.Coin)
            {
                instance = animationObjects[1];
                nowAnimationObject = Instantiate(instance, new Vector3(0, 0, 0), Quaternion.Euler(-90, 180, 0));
            }


            nowAnimator = nowAnimationObject.GetComponent<Animator>();
        }
        
    }



    public void startAnimation()
    {
        nowAnimator.SetBool(rotation, true);
    }

    public void endAnimation()
    {
        nowAnimator.SetBool(rotation, false);
    }

    //Manager側のコルーチンでAnimatorのオンオフを切り替えてからつかう。

    public void setSurfaceToward(int SampleValue, RandomManager.SimulationMode mode)  
    {
        if(mode == RandomManager.SimulationMode.Dice)
        {
            nowAnimationObject.transform.rotation = Quaternion.Euler(DiceQuaternion[SampleValue - 1]); //1~6がでる
        }else if (mode == RandomManager.SimulationMode.Coin)
        {
            nowAnimationObject.transform.rotation = Quaternion.Euler(CoinQuaternion[SampleValue - 1]); //1~2がでる
        }

    }



 
}
