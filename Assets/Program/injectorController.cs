using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class injectorController : MonoBehaviour
{
    public GameObject OVRCamera;//�v���C���[�iPlayerController���������I�u�W�F�N�g���v���C���[�Ƃ��Ď擾�����������AOVRCamera�͋C�ɂ��Ȃ��Ă����j
    public PlayerController PlayerController;//�v���C���[�̃R�[�h
    public OVRGrabbable_DeadCOPY grabbable;
    //private OVRInput.Controller controller;
    //public OVRInput.Button shotButton;//3�͒͂ޔ���ɕK�v�Ȃ���
    public AudioSource audioSource;//�����Ǘ�
    public AudioClip UseSound;//���ˊ�g�p��
    public float DestroySec = 2.0f;

    private float frequency = 1.0f;//�R���g���[���[�̐U��
    private float amplitude = 0.8f;//�o�C�u���[�V�����̋��x���w�肵�܂��B0�͐U���Ȃ��A1�͍ő勭�x���Ӗ����܂�
    private float duration = 0.5f;//�U�����������鎞�Ԃ��w�肵�܂��i�b���j�B(�����l�͏e�̔���)
    private bool isUseable = true;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       
        injectUse();
            
    }

    public void injectUse()//���ˊ�g�p�֐�
    {
        if (isUseable)
        {
            if (grabbable.isGrabbed)
            {
                //controller = grabbable.grabbedBy.GetController();
                if (((grabbable.grabbedBy.GetController().tag == "lefthand") ? ServerManager.ExWebSocketBehavior.ctrl[grabbable.grabbedBy.getnum()].IsgetDown(Button_sm.r2) : ServerManager.ExWebSocketBehavior.ctrl[grabbable.grabbedBy.getnum()].IsgetDown(Button_sm.r1)))
                {
                    PlayerController.isHealing = true;//�v���C���[�N���X�̃q�[����ON
                    audioSource.PlayOneShot(UseSound);//���炵��
                                                      // Vibration.instance.StartVibration(frequency, amplitude, duration, controller);//�R���g���[���[�U��
                                                      //�g�p������g�p�s�ɂ���n�b��ɏ����鏈��
                    isUseable = false;
                    Invoke("DestroyMe", DestroySec);
                }

            }//�͂�ł�����R���g���|���[���擾
            //((grabbable.grabbedBy.GetController().tag == "lefthand") ? ServerManager.ExWebSocketBehavior.ctrl[grabbable.grabbedBy.getnum()].IsgetDown(Button_sm.r2) : ServerManager.ExWebSocketBehavior.ctrl[grabbable.grabbedBy.getnum()].IsgetDown(Button_sm.r1))
            
        }
    }

    private void DestroyMe()
    {
        ServerManager.syringes.Remove(gameObject);
        Destroy(gameObject);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "GameController") { 
            PlayerController = other.transform.root.gameObject.GetComponent<PlayerController>();//�ڐG�����v���C���[�̃v���C���[�N���X���擾
        }
    }
 }
