using UnityEngine;
using System.Collections.Generic;

public class shiny : MonoBehaviour
{
    [SerializeField]
    Material material;

    [SerializeField]
    MeshRenderer meshRenderer;

    Material defaultM;

    Rigidbody rb;

    [SerializeField]
    float limit = 0.1f;

    [SerializeField]
    float release = 0.5f;

    float timer = 0;

    bool isexit = false;
    public bool exted = true;

    public bool pinched = false;

    bool actOne = true;
    [SerializeField]
    bool shine = false;

    [SerializeField]
    changehandshape changehandshape;

    public Vector3 dist = Vector3.zero;

    HashSet<Collider> cols = new HashSet<Collider>();

    Collider handcol;

    [SerializeField]
    bool iscontain = false;

    void Start()
    {
        material = GameObject.Find("highlightmat").GetComponent<MeshRenderer>().materials[0];
        defaultM = GameObject.Find("highlightmat").GetComponent<MeshRenderer>().materials[1];
        rb = GetComponent<Rigidbody>();
        changehandshape = GameObject.Find("hand").GetComponent<changehandshape>();
        meshRenderer = GetComponent<MeshRenderer>();

        handcol = GameObject.Find("hand").GetComponent<Collider>();

        /*List<Material> materials = new List<Material>
        {
            meshRenderer.materials[0], defaultM
        };
        meshRenderer.SetMaterials(materials);*/
    }

    void Update()
    {
        if (isexit)
        {
            timer += Time.deltaTime;

            if (changehandshape.grabbed)
            {
                isexit = true;

                if (actOne)
                {
                    dist = transform.position - changehandshape.transform.position;

                    // 핵심 수정: GameObject 비교 제거
                    iscontain = cols.Count > 0;

                    actOne = false;
                }
            }
            else
            {
                actOne = true;
            }
        }

        if (timer > limit)
        {
            exted = true;
        }

        if (timer > release)
        {
            isexit = false;
        }

        if (exted)
        {
            shine = false;
        }

        meshRenderer.material = shine ? material : defaultM;

        if (pinched && shine)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
        else if (changehandshape.grabbed && shine)
        {
            rb.useGravity = false;
            rb.isKinematic = true;

            rb.MovePosition(
                changehandshape.transform.position + dist
            );
        }
        else
        {
            rb.useGravity = true;
            rb.isKinematic = false;

            if (!isexit)
            {
                bool isit = false;

                foreach (var o in cols)
                {
                    if (o != null && o.gameObject.layer == LayerMask.NameToLayer("hand"))
                    {
                        isit = true;
                    }
                }

                if (!isit)
                    isexit = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("hand"))
        {
            timer = 0;
            isexit = false;
            exted = false;
            shine = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        cols.Add(collision.collider);
    }

    private void OnCollisionExit(Collision collision)
    {
        cols.Remove(collision.collider);

        if (collision.gameObject.layer == LayerMask.NameToLayer("hand"))
        {
            isexit = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("hand"))
        {
            timer = 0;
            isexit = false;
            exted = false;
            shine = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        cols.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        cols.Remove(other);
    }
}