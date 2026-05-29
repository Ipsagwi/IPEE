using UnityEngine;

public class dir : MonoBehaviour
{
    public float velocity = 0;
    public float weight;
    public float Ek;
    Vector3 prevPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weight = transform.lossyScale.magnitude / Mathf.Sqrt(3);
        GetComponent<Rigidbody>().mass = weight;
        prevPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        velocity = Vector3.Distance(prevPos, transform.position);
        Ek = weight * velocity * velocity / 2;
        prevPos = transform.position;
    }
}
