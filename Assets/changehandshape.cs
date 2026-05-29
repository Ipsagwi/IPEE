//using NUnit.Framework;
using System.Collections.Generic;
using System;
using UnityEngine;

public class changehandshape : MonoBehaviour
{
    [SerializeField, Range(0, 3)]
    public int handIndex;

    int nowidx = 0;

    [SerializeField, Range(0, 3)]
    private int defaultIdx = 3;

    [SerializeField]
    private float scale;

    public Mesh[] hands;

    [SerializeField]
    NewMonoBehaviourScript newMono;

    [SerializeField]
    MeshCollider Collider;

    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    public bool grabbed = false;
    bool changeCol = false;

    HashSet<GameObject> collidedObjects = new HashSet<GameObject>();
    List<Vector3> offs = new List<Vector3>();

    GameObject pinchobj;

    [SerializeField]
    shiny shiny;

    class Item
    {
        public string name;
        public int value;

        public Item(string name, int value)
        {
            this.name = name;
            this.value = value;
        }
    }

    List<Item> gesture = new();

    public string shapename = "";

    string[] gname = new string[5]
    {
        "push",
        "grab",
        "pinch",
        "point",
        "none"
    };

    [SerializeField]
    shiny pinchedObj = null;
    shiny nullvar_shiny = null;
    Rigidbody Prb;

    bool selecting = false;

    Vector3 pinchoffs;
    Vector3 prevPos;
    Vector3 handVelocity;

    public float k = 100;

    Item FindItem(string itemName)
    {
        int index = gesture.FindIndex(x => x.name.Equals(itemName));

        if (index < 0)
        {
            return null;
        }

        return gesture[index];
    }

    void Start()
    {
        int i = 0;

        foreach (var n in gname)
        {
            gesture.Add(new(n, i));
            i++;
        }

        handIndex = defaultIdx;

        transform.GetComponent<MeshFilter>().mesh = hands[handIndex];
        Collider.sharedMesh = hands[handIndex];

        transform.localScale = scale * Vector3.one;

        Collider.isTrigger = true;

        prevPos = transform.position;


    }

    void Update()
    {
        handVelocity = (transform.position - prevPos) / Time.deltaTime / Time.deltaTime;
        prevPos = transform.position;

        try
        {
            handIndex = FindItem(newMono.handData.gesture).value;
        }
        catch { }
        finally
        {
            if (handIndex > 3)
                handIndex = defaultIdx;

            handIndex = Mathf.Clamp(handIndex, 0, hands.Length - 1);

            transform.localScale = scale * Vector3.one;

            if (nowidx != handIndex)
            {
                try
                {
                    transform.GetComponent<MeshFilter>().mesh = hands[handIndex];
                    Collider.sharedMesh = hands[handIndex];
                }
                catch
                {
                    transform.GetComponent<MeshFilter>().mesh = hands[defaultIdx];
                    Collider.sharedMesh = hands[defaultIdx];
                    handIndex = defaultIdx;
                }

                nowidx = handIndex;

                grabbed = false;
                selecting = false;

                try
                {
                    pinchedObj.pinched = false;
                    pinchedObj = nullvar_shiny;
                }
                catch { }

                offs.Clear();

                Collider.isTrigger = false;

                if (handIndex == 0)
                    Collider.isTrigger = true;

                if (handIndex == 1)
                {
                    grabbed = collidedObjects.Count > 0;
                    if (grabbed)
                        Collider.isTrigger = true;
                }

                if (handIndex == 2)
                {
                    selecting = true;
                }
            }

            if (handIndex == 2)
            {
                if (selecting)
                {
                    collidedObjects.RemoveWhere(x => x == null);
                    Collider.isTrigger = true;
                    if (collidedObjects.Count != 0)
                    {
                        GameObject nearestObj = null;
                        float dist = float.MaxValue;

                        foreach (var obj in collidedObjects)
                        {
                            if (obj == null)
                                continue;

                            shiny s = obj.GetComponent<shiny>();

                            if (s == null)
                                continue;

                            float d = Vector3.Distance(obj.transform.position, transform.position);

                            if (d < dist)
                            {
                                dist = d;
                                nearestObj = obj;
                            }
                        }

                        if (nearestObj != null)
                        {
                            pinchedObj = nearestObj.GetComponent<shiny>();

                            if (pinchedObj != null)
                            {
                                pinchedObj.pinched = true;

                                selecting = false;
                                pinchoffs = pinchedObj.transform.position - transform.position;
                                Prb = pinchedObj.GetComponent<Rigidbody>();
                            }
                        }
                    }
                }

                if (pinchedObj != null && Prb != null)
                {
                    Prb.MovePosition(transform.position + pinchoffs);
                }
            }
            else
            {
                if (pinchedObj != null)
                {
                    pinchedObj.pinched = false;

                    if (Prb != null)
                    {
                        Prb.AddForce(handVelocity * k);
                    }
                }

                pinchedObj = nullvar_shiny;
            }

            if (handIndex == 3)
            {

            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("notouch"))
        {
            collidedObjects.Add(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        changeCol = true;

        if (collidedObjects.Contains(collision.gameObject))
        {
            collidedObjects.Remove(collision.gameObject);
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("notouch"))
        {
            collidedObjects.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        changeCol = true;

        if (collidedObjects.Contains(collision.gameObject))
        {
            collidedObjects.Remove(collision.gameObject);
        }
    }
}