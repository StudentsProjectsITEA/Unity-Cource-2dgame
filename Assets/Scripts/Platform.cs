using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    public float speed;
    public bool isGameOver;


    void Update () {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
	}
}
