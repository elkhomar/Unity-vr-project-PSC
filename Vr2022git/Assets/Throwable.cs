using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Throwable : MonoBehaviour
{
    // Start is called before the first frame update

    public void isThrown()
    {
        Destroy(gameObject);
    }
}
