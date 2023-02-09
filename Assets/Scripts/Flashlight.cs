using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    Light llight;

    void Start()
    {
        llight = GetComponent<Light>();
    }

    void Update()
    {

        if (Input.GetKeyUp(KeyCode.L))
        {
            gameObject.GetComponent<AudioSource>().Play();
            llight.enabled = !llight.enabled;
        }
    }
}
