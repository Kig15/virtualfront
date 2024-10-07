using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandShieldController : MonoBehaviour
{
    public int ShieldHelth;//シールドの体力管理
    public int ShieldHelthMax;//シールドの最大体力
    public TextMeshPro ShieldUI;//物理的なUI
    public GameObject Shield;//シールド
    public MeshCollider MeshCollider;
    public OVRGrabbable grabbable;
    private OVRInput.Controller controller;
    public OVRInput.Button shotButton;

    // Start is called before the first frame update
    void Start()
    {
        ShieldHelth = ShieldHelthMax;
        ShieldUI.text = "" + ShieldHelth;
        grabbable = GetComponent<OVRGrabbable>();
        Shield.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ShieldControll();
    }

    private void ShieldControll()
    {
        controller = grabbable.grabbedBy.GetController();
        if (grabbable.isGrabbed && OVRInput.Get(shotButton, controller))
        {
            Shield.SetActive(true);
            return;
        }
       

            Shield.SetActive(false);
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            ShieldHelth -= collision.gameObject.GetComponent<BulletContloller>().damage;
            Destroy(collision.gameObject);
            ShieldUI.text = "" + ShieldHelth;
        }
    }
}
