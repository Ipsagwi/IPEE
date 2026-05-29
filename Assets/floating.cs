using UnityEngine;

public class floating : MonoBehaviour
{
    Vector3 past = Vector3.zero;
    Vector3 dir = Vector3.zero;
    public float offset;
    bool istouched = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!istouched)
        {
            dir = (transform.position - past).normalized;
            past = transform.position;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        istouched=true;
        if (collision.gameObject.layer == LayerMask.NameToLayer("notouch"))
        {
            transform.position -= dir * offset;
            Debug.Log("touched");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        istouched=false;
    }
}
