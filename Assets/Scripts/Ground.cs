using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour {

    private float timer = 0.0f;
    public Transform A, B;
    [Range(0,1)]
    public float speed;
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        transform.position = Vector3.Lerp(A.position, B.position, Mathf.PingPong(timer * speed, 1f));
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {

        }
    }
}
