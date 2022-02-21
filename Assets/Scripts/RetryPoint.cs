using System.Collections.Generic;
using UnityEngine;

public class RetryPoint : MonoBehaviour
{
    private Stack<Transform> _retryPointStack = new Stack<Transform>();
    public GameObject randomset;

    void Start()
    {
        randomset = GameObject.Find("RamdomSet");
    }

    private void OnTriggerEnter(Collider other)
    {
        //リトライエリアをスタックに追加
        if (other.gameObject.layer == LayerMask.NameToLayer("RetryPoint"))
        {
            _retryPointStack.Push(other.gameObject.transform);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        //Missに接触
        if (other.gameObject.layer == LayerMask.NameToLayer("Miss"))
        {
            //座標を戻す
            this.gameObject.transform.position = _retryPointStack.Peek().position;
            randomset.GetComponent<RandomSet>().Set();

            //その他、フェードや諸々の設定を元に戻すなどのリトライ処理
            //元に戻したい処理が増えてくるなら、
            //Destroyして、Instantiateするってのもありかも
        }
    }
}