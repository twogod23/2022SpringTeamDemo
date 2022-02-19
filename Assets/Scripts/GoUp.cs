using UnityEngine;
using System.Collections;
 
public class GoUp : MonoBehaviour {
	// x軸方向に加える風の力
	public float windX = 0f;
	// y軸方向に加える風の力
	public float windY = 0f;
	// z軸方向に加える風の力
	public float windZ = 0f;
  // Use this for initialization
  void Start () {
  }
 
  // Update is called once per frame
  void Update () {
  }
 
  void OnTriggerStay(Collider other) {
    if (other.gameObject.tag == "Player") {
		  // 当たった相手のrigidbodyコンポーネントを取得
		  Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();

	  	// rigidbodyがnullではない場合（相手のGameObjectにrigidbodyが付いている場合）
		  if (otherRigidbody != null)
		  {
			  // 相手のrigidbodyに力を加える
		  	otherRigidbody.AddForce(windX, windY, windZ, ForceMode.Force);
		  }
    }
  }
}