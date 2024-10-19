using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandShieldController : MonoBehaviour
{
    public int ShieldHelth;//�V�[���h�̗̑͊Ǘ�
    public int ShieldHelthMax;//�V�[���h�̍ő�̗�
    public int OVERHEATsec = 10;//�V�[���h�̃I�[�o�[�q�[�g����
    public TextMeshPro ShieldUI;//�����I��UI
    public GameObject Shield;//�V�[���h
    public AudioSource audioSource;//�����Ǘ�
    public AudioClip EnableSound, DisableSound,OverHeatSound;//�W�J���@�i�[�T�E���h�@�I�[�o�[�q�[�g��
    public MeshCollider MeshCollider;//�ӂ蔻��
    public OVRGrabbable_DeadCOPY grabbable;
    private OVRInput.Controller controller;
    public OVRInput.Button shotButton;
    public bool isactive;

    private bool isOVERHEAT;
    private float frequency = 1.0f;//�R���g���[���[�̐U��
    private float amplitude = 0.5f;//�o�C�u���[�V�����̋��x���w�肵�܂��B0�͐U���Ȃ��A1�͍ő勭�x���Ӗ����܂�
    private float duration = 0.3f;//�U�����������鎞�Ԃ��w�肵�܂��i�b���j�B(�����l�͏e�̔���)

    // Start is called before the first frame update
    void Start()
    {
        ShieldHelth = ShieldHelthMax;
        ShieldUI.text = "" + ShieldHelth;
        grabbable = GetComponent<OVRGrabbable_DeadCOPY>();
        audioSource = GetComponent<AudioSource>();//�������i�R���|�[�l���g�擾�j
        Shield.SetActive(false);//�E�ʏ�̓V�[���h�����͔�\��gameObject.SetActive(false);�ɂȂ��Ă���
    }

    // Update is called once per frame
    void Update()
    {
        ShieldControll();
    }

    private void ShieldControll()
    {
        if (ShieldHelth > 0)
        {
            if (grabbable.isGrabbed)
            {
                //controller = grabbable.grabbedBy.GetController();
            }//�͂񂾂�R���g���[���[�擾
            if (grabbable.isGrabbed && OVRInput.GetDown(shotButton, controller))
            {
                audioSource.PlayOneShot(EnableSound);//���C��
                Vibration.instance.StartVibration(frequency, amplitude, duration, controller);
                Shield.SetActive(true);
                isactive = true;
            }//�g���K�[���������u�Ԃ̏���
            if (grabbable.isGrabbed && OVRInput.Get(shotButton, controller))
            {

                Shield.SetActive(true);//�E������g���K�[�������Ă���Ԃ����\��gameObject.SetActive(true);�ɂ��Ă���
                isactive = true;

                return;
            }//�����Ă���Ԃ̏���
            else if (grabbable.isGrabbed && OVRInput.GetUp(shotButton, controller))
            {
                audioSource.PlayOneShot(DisableSound);//���C��
                Shield.SetActive(false);
                isactive = false;
            }//�����u�Ԃ̏���
            else
            {
                Shield.SetActive(false);
                isactive = false;
            }
        }
        else if(ShieldHelth<= 0)
        {
            if (!isOVERHEAT)
            {
                audioSource.PlayOneShot(OverHeatSound);
                Shield.SetActive(false);
                ShieldUI.text = "OVERHEAT";
                Invoke("Reload", OVERHEATsec);
                Vibration.instance.StartVibration(frequency, amplitude, duration, controller);
                isOVERHEAT = true;
            }//�V�[���h�̗̑͂�0�ɂȂ�����OverHeat
        }
       

           
        
    }

    public void Reload()
    {
        ShieldHelth = ShieldHelthMax;
        ShieldUI.text = "" + ShieldHelth;
        isOVERHEAT = false;
    }




    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "bullet")
        {
            ShieldHelth -= other.gameObject.GetComponent<BulletContloller>().damage;
            ShieldUI.text = "" + ShieldHelth;
        }//���������̂��e��������_���[�W���Q�Ƃ��Ĕ�e ��e����͂���
    }
}
