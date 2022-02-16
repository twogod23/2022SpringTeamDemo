using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
 
public class Player : MonoBehaviour {
 
	private Animator animator;
	private Vector3 latestPos;  //前回のPosition
	public bool isTouch;
	[SerializeField]
	private float jumpPower = 5f;	
	//　歩く速さ
	[SerializeField]
	private float walkSpeed = 4f;
	[SerializeField]
	private Vector3 velocity;
	//　入力値
	private Vector3 input;
	//　rigidbody
	private Rigidbody rigid;
	//　地面に接地しているかどうか
	[SerializeField]
	private bool isGrounded;
	//　衝突しているかどうか
	[SerializeField]
	private bool isCollision;
	//　接地確認のコライダの位置のオフセット
	[SerializeField]
	private Vector3 groundPositionOffset = new Vector3(0f, 0.02f, 0f);
	//　接地確認の球のコライダの半径
	[SerializeField]
	private float groundColliderRadius = 0.29f;
	//　衝突確認のコライダの位置のオフセット
	[SerializeField]
	private Vector3 collisionPositionOffset = new Vector3(0f, 0.5f, 0.1f);
	//　衝突確認の球のコライダの半径
	[SerializeField]
	private float collisionColliderRadius = 0.3f;
	//　ジャンプ中かどうか
	[SerializeField]
	private bool isJump;
	//　ジャンプ後の着地判定までの遅延時間
	[SerializeField]
	private float delayTimeToLanding = 0.5f;
	//　ジャンプ後の時間
	[SerializeField]
	private float jumpTime;

	//　前方に段差があるか調べるレイを飛ばすオフセット位置
	[SerializeField]
	private Vector3 stepRayOffset = new Vector3(0f, 0.05f, 0f);
	//　レイを飛ばす距離
	[SerializeField]
	private float stepDistance = 0.5f;
	//　昇れる段差
	[SerializeField]
	private float stepOffset = 0.3f;
	//　昇れる角度
	[SerializeField]
	private float slopeLimit = 65f;
	//　昇れる段差の位置から飛ばすレイの距離
	[SerializeField]
	private float slopeDistance = 0.6f;
 
	void Start() {
		animator = GetComponent<Animator>();
		rigid = GetComponent<Rigidbody>();
		isTouch = false;
		isCollision = false;
		isGrounded = true;
	}
 
 
	void Update() {
		CheckGround();
		if (isGrounded && !isTouch) {
			velocity = Vector3.zero;
            input.x = Input.GetAxis("Horizontal");
			input.z = Input.GetAxis("Vertical");
			input.Normalize();
 
			//　方向キーが多少押されている
			if (input.magnitude > 0f) {
				animator.SetFloat("Speed", input.magnitude);
				transform.LookAt(rigid.position + input);

				var stepRayPosition = rigid.position + stepRayOffset;
 
				//　ステップ用のレイが地面に接触しているかどうか
				if (Physics.Linecast(stepRayPosition, stepRayPosition + rigid.transform.forward * stepDistance, out var stepHit)) {
					//　進行方向の地面の角度が指定以下、または昇れる段差より下だった場合の移動処理
 
					if (Vector3.Angle(rigid.transform.up, stepHit.normal) <= slopeLimit || (Vector3.Angle(rigid.transform.up, stepHit.normal) > slopeLimit
						&& !Physics.Linecast(rigid.position + new Vector3(0f, stepOffset, 0f), rigid.position + new Vector3(0f, stepOffset, 0f) + rigid.transform.forward * slopeDistance))
					) {
						velocity = new Vector3(0f, (Quaternion.FromToRotation(Vector3.up, stepHit.normal) * rigid.transform.forward * walkSpeed).y, 0f) + rigid.transform.forward * walkSpeed;
					} else {
						//　指定した条件に当てはまらない場合は速度を0にする
						velocity = Vector3.zero;
					}
 					//　前方の壁に接触していなければ
				} else {
					velocity = transform.forward * walkSpeed;
				}
				//　キーの押しが小さすぎる場合は移動しない
			} else {
				animator.SetFloat("Speed", 0f);
			}
			//　ジャンプ
			if (Input.GetButtonDown("Jump")) {
				//　ジャンプしたら接地していない状態にする
				isGrounded = false;
				isJump = true;
				jumpTime = 0f;
				velocity.y = jumpPower;
				// 2ax = v²-v₀²より
				//velocity.y = Mathf.Sqrt(-2 * Physics.gravity.y * jumpPower);
				animator.SetBool("Jumping", true);
			}
		}
		//　接触していたら移動方向の値は0にする
		if (!isGrounded && isCollision) {
			velocity = new Vector3(0f, velocity.y, 0f);
		}
		//　ジャンプ時間の計算
		if (isJump && jumpTime < delayTimeToLanding) {
			jumpTime += Time.deltaTime;
		}
		
		Vector3 diff = transform.position - latestPos;   //前回からどこに進んだかをベクトルで取得
        latestPos = transform.position;  //前回のPositionの更新

        //ベクトルの大きさが0.01以上の時に向きを変える処理をする
        if ((diff.magnitude > 0.01f) && !isTouch)
        {
            transform.rotation = Quaternion.LookRotation(diff); //向きを変更する
        }
		

		//　接触していたら移動方向の値は0にする
		if (!isGrounded && isCollision && isTouch) {
			velocity = new Vector3(0f, 0, 0f);
        }
	}
 
	void FixedUpdate() {
		if(isGrounded && !isTouch)
		{		
		    // カメラの方向から、X-Z平面の単位ベクトルを取得
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
 
            // 方向キーの入力値とカメラの向きから、移動方向を決定
            Vector3 moveForward = cameraForward * Input.GetAxis("Vertical") + Camera.main.transform.right * Input.GetAxis("Horizontal");
            // 移動方向にスピードを掛ける。
            rigid.velocity = moveForward * walkSpeed;
		}

		if (isCollision && isTouch) {
			velocity = new Vector3(0f, 0f, 0f);
		}
	}
 
	private void OnCollisionEnter(Collision collision) {
		//　指定したコライダと接触、かつ接触確認コライダと接触していたら衝突状態にする
		if (Physics.CheckSphere(rigid.position + transform.up * collisionPositionOffset.y + transform.forward * collisionPositionOffset.z, collisionColliderRadius, ~LayerMask.GetMask("Player"))) {
				isCollision = true;
		}
		//生き物とぶつかったらアニメーションを再生して操作できなくする
        Debug.Log("hit");
        foreach (ContactPoint point in collision.contacts)
        {
            Vector3 relativePoint = transform.InverseTransformPoint(point.point);
		}
	}

	private void OnCollisionExit(Collision collision) {
		//　指定したコライダと離れたら衝突していない状態にする
		isCollision = false;
	}
 
	//　地面のチェック
	private void CheckGround() {
		//　地面に接地しているか確認
		if (Physics.CheckSphere(rigid.position + groundPositionOffset, groundColliderRadius, ~LayerMask.GetMask("Player"))) {
			//　ジャンプ中
			if (isJump) {
				if (jumpTime >= delayTimeToLanding) {
					isGrounded = true;
					isJump = false;
				} else {
					isGrounded = false;
				}
			} else {
				isGrounded = true;
			}
		} else {
			isGrounded = false;
		}
		animator.SetBool("Jumping", !isGrounded);
	}
 	
 
 
	void OnDrawGizmos() {
		//　接地確認のギズモ
		Gizmos.DrawWireSphere(transform.position + groundPositionOffset, groundColliderRadius);
		Gizmos.color = Color.blue;
		//　衝突確認のギズモ
		Gizmos.DrawWireSphere(transform.position + transform.up * collisionPositionOffset.y + transform.forward * collisionPositionOffset.z, collisionColliderRadius);
	}	
}
 