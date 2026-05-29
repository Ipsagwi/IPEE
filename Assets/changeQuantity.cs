using TMPro;
using UnityEngine;

public class changeQuantity : MonoBehaviour
{
    public Material Rim;
    public Material normal;
    [SerializeField]
    GameObject lasttouched = null;
    //MeshRenderer? meshRenderer = null;
    public TextMeshProUGUI objName;
    public TextMeshProUGUI objInfo;
    public TextMeshProUGUI handInfo;
    dir infos;
    dir myinfo;
    NewMonoBehaviourScript newMono;
    int i = 0;
    public float interval = 0.5f;
    //MeshRenderer? initmeshrenderer = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //meshRenderer = initmeshrenderer;
        myinfo = GetComponent<dir>();
        newMono = GetComponent<NewMonoBehaviourScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*if (i == 0)
        {*/
            try
            {
                objName.text = lasttouched.name;
                objInfo.text = $"Velocity : {(infos.velocity * 100).ToString("0.00")} cm/s\nWeightt : {infos.weight.ToString("0.00")} N\nKinetic Energy : {infos.Ek.ToString("0.00")}\nFacing Point [  ]";
            }
            catch { }

            handInfo.text = $"Velocity : {(myinfo.velocity * 100).ToString("0.00")} cm/s\nWeightt : {myinfo.weight.ToString("0.00")} N\nKinetic Energy : {myinfo.Ek.ToString("0.00")}\nShape : {newMono.handData.gesture}";
        /*}
        i++;
        if (i >= 50 * interval)
        {
            i = 0;
        }*/
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("notouch"))
        {
            lasttouched = collision.gameObject;
            infos = lasttouched.GetComponent<dir>();
            /*if (meshRenderer != null)
            {
                meshRenderer.materials[1] = normal;
            }
            meshRenderer = collision.gameObject.GetComponent<MeshRenderer>();
            meshRenderer.materials[1] = Rim;*/

        }
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("notouch"))
        {
            lasttouched = collision.gameObject;
            infos = lasttouched.GetComponent<dir>();
            /*if (meshRenderer != null)
            {
                meshRenderer.materials[1] = normal;
            }
            meshRenderer = collision.gameObject.GetComponent<MeshRenderer>();
            meshRenderer.materials[1] = Rim;*/

        }
    }
}
