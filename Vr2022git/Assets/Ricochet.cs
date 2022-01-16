using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ricochet : MonoBehaviour
{
    public GameObject go;


    // constants
    
    
    public float alpha = 0.35f;
    private  float tau = 0.1f;
    private float dt;
    // vx to vz ratio
    private float teta;

    
    // dynamic parameters changing every bounce
    
    public Vector3 p0 = new Vector3(0, 10, 0);
    public  Vector3 v0 = new Vector3(0, 0, 0);
    public float nV0; 
    
    public float beta;
    private float dr;
    private float dy;
    
    
    // dynamic parameters changing every frame

    private float t = 0;
    private float r = 0;
    private float y;



    
    void Start()
    {
        dt = Time.deltaTime;
        go.transform.position = p0;
        nV0 = v0.magnitude;
        
        // Computing teta
        teta = Mathf.Atan(v0.z / v0.x);
                
        // Computing beta 
        beta = Mathf.Atan(-v0.y / Mathf.Sqrt(v0.x*v0.x + v0.z*v0.z));
        
        // alpha = pebble orientation
        go.transform.Rotate(Vector3.forward, alpha*180/Mathf.PI);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        t += dt;
        
        // free fall
        if (go.transform.position.y >= 0)
        {
            r = t * nV0 * Mathf.Cos(beta);
            y = -9.8f * t * t * 0.5f - t*nV0*Mathf.Sin(beta) + p0.y;
            
            // enter accurate ricochet
            if (y < 0)
            {
                Debug.Log("acc");
                
                // compute new beta at interface
                dr = dt * (nV0 * Mathf.Cos(beta));
                dy = y - go.transform.position.y;
                beta = Mathf.Atan(-dy / dr);
                
                reset();
            }
        }
        
        // accurate ricochet
        else
        {
            r = Mathf.Cos(alpha) * nV0 * Mathf.Cos(alpha + beta) * t -
                Mathf.Sin(alpha) * tau * nV0 * Mathf.Sin(alpha + beta) * (Mathf.Exp(-t / tau) - 1);
        
            y = Mathf.Sin(alpha) * nV0 * Mathf.Cos(alpha + beta) * t +
                Mathf.Cos(alpha) * tau * nV0 * Mathf.Sin(alpha + beta) * (Mathf.Exp(-t / tau) - 1);
            
            // enter free fall
            if (y >= 0)
            {
                Debug.Log("ff");
                
                // compute new beta at interface
                dr = dt*(Mathf.Cos(alpha) * nV0 * Mathf.Cos(alpha + beta) 
                     + Mathf.Sin(alpha) * nV0 * Mathf.Sin(alpha + beta) * Mathf.Exp(-t / tau));
                dy = y - go.transform.position.y;
                beta = Mathf.Atan(-dy / dr);
                
                reset();
            }
        }
        go.transform.position = new Vector3(p0.x + r * Mathf.Cos(teta), y, p0.z + r * Mathf.Sin(teta));
    }

    public void reset()
    {
        // sets initial parameters for the next phase
        p0 = new Vector3(p0.x + r*Mathf.Cos(teta), y, p0.z + r*Mathf.Sin(teta));
        nV0 = (1 / dt) * Mathf.Sqrt(dr * dr + dy * dy);
        t = 0;
        r = 0;
        sink();
    }

    public void sink()
    {
        // if the pebble is too slow it sinks
        if (nV0 <= 0.01f)
        {
            nV0 = 0;
        }
    }
}
