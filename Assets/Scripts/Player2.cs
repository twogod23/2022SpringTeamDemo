using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    //プレイヤーの移動する速さ
    public float move_speed = 15;
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
    }

    private void FixedUpdate()
    { 
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 1, 1)).normalized;
        Vector3 cameraRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 1, 1)).normalized;
        Vector3 moveForward = cameraForward * Input.GetAxis("Vertical") + cameraRight * Input.GetAxis("Horizontal");

        Rig.velocity = moveForward * move_speed + new Vector3(0, Rig.velocity.y);

        if (moveForward.magnitude != 0f) transform.rotation = Quaternion.LookRotation(moveForward);
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
        if (other.gameObject.tag == "Cloud")//  もしCloudというタグがついたオブジェクトに触れたら、
        {
            Grounded = true;//  Groundedをtrueにする
        }
    }
}