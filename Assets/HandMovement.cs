using UnityEngine;

public class HandMovement : MonoBehaviour
{
    [SerializeField]
    NewMonoBehaviourScript newMono;

    [SerializeField]
    Vector3 multiplier;

    [SerializeField]
    Vector2 quality = new Vector2(1280,720);

    [SerializeField]
    float offsetZ = 0f;

    [SerializeField]
    float posT;

    [SerializeField, Range(-1, 1)]
    float rotT;

    Vector3? last = null;
    Quaternion lastRot = Quaternion.identity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var p = newMono.handData;
        Vector3 pos = new Vector3(
            (p.palmPixel.x - quality.x/2) * multiplier.x,
            (p.palmPixel.y - quality.y/2) * multiplier.y,
            (p.depthCm + offsetZ) * multiplier.z
        );

        //transform.position = pos;
        if ( last != null )
        {
            if (Vector3.Distance((Vector3)last, pos) < posT)
            {
                GetComponent<Rigidbody>().position = Vector3.Lerp(
                    GetComponent<Rigidbody>().position,
                    -pos,
                    0.1f
                );
            }
        }
        // 회전
        Quaternion rot = GetHandRotation(p);
        if(lastRot != null)
            if (Vector3.Dot(lastRot.eulerAngles.normalized, rot.eulerAngles.normalized) >= rotT)
                GetComponent<Rigidbody>().rotation = rot;

        last = pos;
        lastRot = rot;
    }

    Quaternion GetHandRotation(HandPacket p)
    {
        Vector3 up = (p.palmNormal - 2*Vector3.forward * p.palmNormal.z).normalized;

        // 기준 노멀 (네 정의: 기본 자세)
        Vector3 refUp = new Vector3(0, 0, -1);

        // 1차: 노멀 기준 회전
        Quaternion fromNormal = Quaternion.FromToRotation(refUp, up);

        return fromNormal;
    }
}
