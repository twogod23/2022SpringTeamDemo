using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    private float inputHorizontal;
    private float inputVertical;
    //プレイヤーの移動する速さ
    public float move_speed = 15;

    //プレイヤーの回転する速さ
    public float rotate_speed = 5;

    //プレイヤーのRigidbody
    private Rigidbody Rig = null;

    //地面に着地しているか判定する変数
    public bool Grounded;

    //ジャンプ力
    public float Jumppower;


    void Start()
    {
        Rig = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        Jump();
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");     
    }

    private void FixedUpdate()
    {
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 1, 1)).normalized;
 
        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;
 
        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        Rig.velocity = moveForward * move_speed + new Vector3(0, Rig.velocity.y, 0);
 
        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(moveForward);
        }

        Vector3 move_direction = cameraForward * inputHorizontal + Camera.main.transform.right * inputVertical;

        Rig.MovePosition(Rig.position + transform.TransformDirection(move_direction) * move_speed * Time.deltaTime);
    }

    void Jump()
    {
        if (Grounded == true)//  もし、Groundedがtrueなら、
        {
            if (Input.GetKeyDown(KeyCode.Space))//  もし、スペースキーがおされたなら、  
            {
                Grounded = false;//  Groundedをfalseにする
                Rig.AddForce(transform.up * Jumppower * 100);//  上にJumpPower分力をかける
            }
        }
    }

    void OnCollisionEnter(Collision other)//  他オブジェクトに触れた時の処理
    {
        if (other.gameObject.tag == "Cloud")//  もしPlanetというタグがついたオブジェクトに触れたら、
        {
            Grounded = true;//  Groundedをtrueにする
        }
    }
}