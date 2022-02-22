using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceScript : MonoBehaviour
{
    public Slider slider;
    
    //距離を測る2物体の指定
    public GameObject player;
    public GameObject goal;

    //座標を代入する関数の宣言
    //ゴールのx座標
    float Gposx = 0;
    //プレイヤーの初期x座標
    float SPposx = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        slider.value = 0;

        //2物体の初期x座標を取得
        Gposx = goal.transform.position.x;
        SPposx = player.transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーのx座標を取得
        float Pposx = player.transform.position.x;

        float distA = Pposx - SPposx;
        float distB = Gposx - Pposx;
        float distC = Gposx - SPposx;

        if (distA < 0) //sliderが0未満にならない処理
        {
            slider.value = 0;
        }
        else if (distB < 0) //sliderが1を超えない処理
        {
            slider.value = 1;
        }
        else //sliderの計算
        {
            slider.value = Pposx / distC;
        }
    }
}
