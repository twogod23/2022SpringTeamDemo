using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoonFallFloor : MonoBehaviour
{
    // Start is called before the first frame update
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject, 0f);
        }
    }
}