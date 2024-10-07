using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandShieldController : MonoBehaviour
{
    public int ShieldHelth;//�V�[���h�̗̑͊Ǘ�
    public int ShieldHelthMax;//�V�[���h�̍ő�̗�
    public TextMeshPro ShieldUI;//�����I��UI
    public GameObject Shield;//�V�[���h
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
