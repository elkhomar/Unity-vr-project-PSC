using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class Throwable : MonoBehaviour
{
    // Start is called before the first frame update

    public void isThrown()
    {
        // change physics
        this.GetComponent<Ricochet>().enabled = true;
        this.GetComponent<Ricochet>().p0 = this.gameObject.transform.position;
        this.GetComponent<Ricochet>().v0 = this.GetComponent<Rigidbody>().velocity;
        Debug.Log(this.GetComponent<Rigidbody>().velocity);
        Destroy(this.GetComponent<XRGrabInteractable>());
        Destroy(this.GetComponent<Rigidbody>());

    }
}
