using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public PlatformManager PlatformManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlatformManager.platformSwitch();
    }
}