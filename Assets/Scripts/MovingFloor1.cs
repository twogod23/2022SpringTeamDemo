using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloor1 : MonoBehaviour
{
    [SerializeField]private int counter = 0;
    float move = 0.01f;
 
    void Update()
    {
        Vector3 p = new Vector3(0, 0, move);
        transform.Translate(p);
        counter++;
        
        if (counter == 750)
        {
            counter = 0;
            move *= -1;
        }
    }
}