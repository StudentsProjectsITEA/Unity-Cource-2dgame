using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour {

    public Transform player;

    private void Update()
    {
        transform.position = player.position;
    }
}
