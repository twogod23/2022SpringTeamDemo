using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //アニメーション
    private Animator animator;
    //入力値
	private Vector3 input;
    //プレイヤーの移動する速さ
    public float move_speed = 3;
    //プレイヤーのRigidbody
    private Rigidbody Rig;
    //地面に着地しているか判定する変数
    public bool Grounded;
    //ジャンプ力
    [SerializeField]private float Jumppower;

    //接地判定に関わるRay
    private Ray ray;
    //Rayを飛ばす距離
    [SerializeField]private float rayDistance = 0.5f;
    //Rayが当たった時の情報
    private RaycastHit hit;
    //Rayの発射位置
    private Vector3 rayPosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        Rig = this.GetComponent<Rigidbody>();
        
    }

    void Update()
    {
        
        rayPosition = transform.position + new Vector3(0, 0.3f, 0); // レイを発射する位置の調整
        ray = new Ray(rayPosition, transform.up * -1); // レイを下に飛ばす
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red); // レイを赤色で表示させる
 
        if (Physics.Raycast(ray, out hit, rayDistance)) // レイが当たった時の処理
        {
            if (hit.collider.tag == "Cloud") // レイが地面に触れたら、
            {
                Grounded = true; // 地面に触れたことにする
            } else {
                Grounded = false; // 地面に触れてないことにする
            }
        }
        Jump();
        Move();
    }

    void Move()
    {
        if(Grounded == true) {
            input.x = Input.GetAxis("Horizontal");
		    input.z = Input.GetAxis("Vertical");
		    input.Normalize();
            //　方向キーが多少押されている
			if (input.magnitude > 0f) {
                Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
                Vector3 cameraRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;
                Vector3 moveForward = cameraForward * input.z + cameraRight * input.x;

                Rig.velocity = moveForward * move_speed + new Vector3(0, Rig.velocity.y);

                if (moveForward.magnitude != 0f) transform.rotation = Quaternion.LookRotation(moveForward);
				animator.SetFloat("Speed", input.magnitude);
				//　キーの押しが小さすぎる場合は移動しない
			} else {
				animator.SetFloat("Speed", 0f);
			}
            
        }
        
    }

    void Jump()
    {
        if (Grounded == true)//  もし、Groundedがtrueなら、
        {
            animator.SetBool("Jump", false);
        }
        if (Grounded == true && Input.GetKeyDown(KeyCode.Space))//  もし、スペースキーがおされたなら、  
        {
            Grounded = false;//  Groundedをfalseにする
            Rig.AddForce(transform.up * Jumppower * 100);//  上にJumpPower分力をかける
            animator.SetBool("Jump", true);
        }
    }
}