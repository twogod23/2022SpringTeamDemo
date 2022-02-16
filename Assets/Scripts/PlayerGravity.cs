using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    //PlayerのTransform
    private Transform myTransform;

    //PlayerのRigidbody
    private Rigidbody rig = null;

    //重力減となる惑星
    private GameObject Cloud;

    //「Cloud」タグがついているオブジェクトを格納する配列
    private GameObject[] Clouds;

    //重力の強さ
    public float Gravity;

    //惑星に対するPlayerの向き
    private Vector3 Direction;

    //Rayが接触した惑星のポリゴンの法線
    private Vector3 Normal_vec = new Vector3(0,0,0);

    void Start()
    {
        rig = this.GetComponent<Rigidbody>();
        rig.constraints = RigidbodyConstraints.FreezeRotation;
        rig.useGravity = false;
        myTransform = transform;
    }

    void Update()
    {
        Attract();
        RayTest();
    }

    public void Attract()
    {
        Vector3 gravityUp = Normal_vec;

        Vector3 bodyUp = myTransform.up;

        myTransform.GetComponent<Rigidbody>().AddForce(gravityUp * Gravity);

        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * myTransform.rotation;

        myTransform.rotation = Quaternion.Lerp(myTransform.rotation, targetRotation, 120 * Time.deltaTime);

    }

    GameObject Choose_Cloud()
    {
        Clouds = GameObject.FindGameObjectsWithTag("Cloud");

        double[] Cloud_distance = new double[Clouds.Length];

        for (int i = 0; i < Clouds.Length; i++)
        {
            Cloud_distance[i] = Vector3.Distance(this.transform.position, Clouds[i].transform.position);
        }

        int min_index = 0;
        double min_distance = Mathf.Infinity;

        for (int j = 0; j < Clouds.Length; j++)
        {
            if (Cloud_distance[j] < min_distance)
            {
                min_distance = Cloud_distance[j];
                min_index = j;
            }
        }

        return Clouds[min_index];
    }

    void RayTest()
    {
        Cloud = Choose_Cloud();

        Direction = Cloud.transform.position - this.transform.position;

        Ray ray = new Ray(this.transform.position, Direction);

        //Rayが当たったオブジェクトの情報を入れる箱
        RaycastHit hit;

        //もしRayにオブジェクトが衝突したら
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            //Rayが当たったオブジェクトのtagがCloudだったら
            if (hit.collider.tag == "Cloud")
            {
                Normal_vec = hit.normal;
            }
        }
    }
}