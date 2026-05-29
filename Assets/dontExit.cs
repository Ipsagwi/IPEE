using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class dontExit : MonoBehaviour
{
    [SerializeField]
    float minWallDist = float.MaxValue;
    [SerializeField]
    Vector3 minWallPos = Vector3.zero;
    [SerializeField]
    float d = 0;
    Vector3 Center;
    [SerializeField]
    RaycastHit[] hits;
    Rigidbody rb;
    /*Vector3 velo;
    Vector3 prev;*/
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hits = new RaycastHit[GameObject.Find("WALL").GetComponentsInChildren<Transform>().Length - 1];
        for (int i = 0; i < hits.Length; i++) { hits[i] = new RaycastHit(); }
        Debug.Log(hits.Length);
        Center = GameObject.Find("center").transform.position;
        rb = GetComponent<Rigidbody>();
        /*velo = Vector3.zero;
        prev = transform.position;*/
    }

    // Update is called once per frame
    void Update()
    {
        //velo = transform.position - prev;
        d = Vector3.Distance(transform.position, Center);
        minWallDist = float.MaxValue;
        try
        {
            Physics.RaycastNonAlloc(new Ray(Center, (transform.position - Center).normalized), hits, 100f);
            foreach (RaycastHit a in hits)
            {
                if (a.collider.gameObject.layer == LayerMask.NameToLayer("notouch"))
                {
                    if (minWallDist > a.distance)
                    {
                        minWallDist = a.distance;
                        minWallPos = a.point;
                    }
                }
                //Debug.Log(a.collider.name);
            }
        }
        catch
        {

        }
        finally
        {

        }
        if (minWallDist < d)
        {
            transform.position = minWallPos;
            //rb.linearVelocity = Vector3.zero;
            //Debug.Log("Exited");
        }
        //prev = transform.position;
    }
}
