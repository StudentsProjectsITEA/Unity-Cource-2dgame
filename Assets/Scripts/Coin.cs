using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    public float speed;
    public bool isAttracted;
    private Transform magnet;

    void Update()
    {
        if (!isAttracted)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate((magnet.position - transform.position) * speed * Time.deltaTime);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "magnet")
        {
            Debug.Log("GGGGGGGGG");
            magnet = collision.transform;
            isAttracted = true;
        }
    }

    private void OnEnable()
    {
        isAttracted = false;
    }
}
