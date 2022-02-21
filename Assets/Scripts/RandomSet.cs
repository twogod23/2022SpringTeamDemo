using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class RandomSet : MonoBehaviour
{
    public Vector3 pos1 = new Vector3(145.4f, 14.1f, 29.8f);
    public Vector3 pos2 = new Vector3(145.4f, 14.1f, 11.8f);
    public Vector3 pos3 = new Vector3(145.4f, 14.1f, -5.9f);   //決められた3個の座標
    public List<GameObject> clouds = new List<GameObject>();  //ゲームオブジェクト3個

    void Start()
    {
        clouds = clouds.OrderBy ( a => Guid.NewGuid () ).ToList ();

        GameObject cloud1 = Instantiate(clouds[0], pos1, Quaternion.identity);
        GameObject cloud2 = Instantiate(clouds[1], pos2, Quaternion.identity);
        GameObject cloud3 = Instantiate(clouds[2], pos3, Quaternion.identity);
        cloud1.transform.parent = this.transform;
        cloud2.transform.parent = this.transform;
        cloud3.transform.parent = this.transform;
    }

    public void Set()
    {
        foreach(Transform child in gameObject.transform){
            Destroy(child.gameObject);
        }
        clouds = clouds.OrderBy ( a => Guid.NewGuid () ).ToList ();

        GameObject cloud1 = Instantiate(clouds[0], pos1, Quaternion.identity);
        GameObject cloud2 = Instantiate(clouds[1], pos2, Quaternion.identity);
        GameObject cloud3 = Instantiate(clouds[2], pos3, Quaternion.identity);
        cloud1.transform.parent = this.transform;
        cloud2.transform.parent = this.transform;
        cloud3.transform.parent = this.transform;
    }

}
