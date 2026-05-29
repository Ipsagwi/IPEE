using Unity.VisualScripting;
using UnityEngine;

public class creatingbaalls : MonoBehaviour
{
    public int particleCount;
    public GameObject circle;
    [SerializeField, Range(0.0f, 1.0f)]
    float scale_Randomness = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < particleCount; i++)
        {
            circle.transform.localScale *= Random.Range(1 - scale_Randomness, 1 + scale_Randomness);
            Instantiate(circle,new Vector3(transform.position.x + Random.Range(-3.0f, 3.0f), transform.position.y + Random.Range(-3.0f, 3.0f), transform.position.z + Random.Range(-3.0f, 3.0f)),transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
