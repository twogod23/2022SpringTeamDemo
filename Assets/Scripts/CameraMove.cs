using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [Header("プレイヤーオブジェクト")]
    public GameObject player;

    private float playerX = 0.0f;  //キャラのX座標取得
    private float playerY = 0.0f;  //キャラのY座標取得
    private float playerZ = 0.0f;  //キャラのZ座標取得
    private float offsetX = 0.0f;  //キャラとカメラのX座標オフセット
    private float offsetY = 0.0f;  //キャラとカメラのY座標オフセット
    private float offsetZ = 0.0f;  //キャラとカメラのZ座標オフセット 
    //private float cameraX = 0.0f;  //カメラのX座標取得

    // Start is called before the first frame update
    void Start()
    {
        //カメラとキャラの位置の差をオフセットとして取り込む
        offsetX = transform.position.x - player.transform.position.x;
        offsetY = transform.position.y - player.transform.position.y;
        offsetZ = transform.position.z - player.transform.position.z;
        //X軸は操作しないので初期位置を取得しておく
        //float cameraX = transform.position.x;


        //初期のキャラの位置を取得する
        playerX = player.transform.position.x;
        playerY = player.transform.position.y;
        playerZ = player.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        //キャラが移動してる時だけキャラの位置を取得する
        if (playerX < player.transform.position.x || playerY < player.transform.position.y || playerZ < player.transform.position.z)
            playerX = player.transform.position.x;
            playerY = player.transform.position.y;
            playerZ = player.transform.position.z;

        //キャラとのオフセットを計算して座標移動させる
        transform.position = new Vector3(playerX + offsetX, playerY + offsetY, playerZ + offsetZ);
    }
}