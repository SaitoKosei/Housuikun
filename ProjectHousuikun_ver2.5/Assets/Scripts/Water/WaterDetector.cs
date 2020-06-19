using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //水に触れたオブジェクトがRigidbody2Dを持っていたら
        if (collision.GetComponent<Rigidbody2D>() != null)
            //波を立たせる関数を呼び出す
            transform.parent.GetComponent<Water>().Splash
                (transform.position.x, 
                collision.GetComponent<Rigidbody2D>().velocity.y * 
                collision.GetComponent<Rigidbody2D>().mass / 40.0f);
    }
}
