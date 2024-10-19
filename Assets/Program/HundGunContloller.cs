using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HundGunContloller : MonoBehaviour
{


    public GameObject bullet, bulletposition;//�e�ۃI�u�W�F�N�g�@�e�۔��ˈʒu
    public AudioClip ShotSound, NoAmmoSound;//���C���@�e�؂ꉹ
    public AudioSource audioSource;//�����Ǘ�
    public Animator anim;//�A�j���[�V�����Ǘ�
    public TextMeshPro bulletNum;//�����I��UI
    public ParticleSystem MuzzleFlash;//�}�Y���t���b�V���G�t�F�N�g
    public OVRGrabbable_DeadCOPY grabbable;
    private OVRInput.Controller controller;
    public OVRInput.Button shotButton;

    public int MagazineMax = 12;//�}�K�W���ő�e��
    public float ReloadSec;//�����[�h����


    private bool isReload = false;
    private int Magazine = 0;

    private float frequency = 1.0f;//�R���g���[���[�̐U��
    private float amplitude = 1f;//�o�C�u���[�V�����̋��x���w�肵�܂��B0�͐U���Ȃ��A1�͍ő勭�x���Ӗ����܂�
    private float duration = 0.1f;//�U�����������鎞�Ԃ��w�肵�܂��i�b���j�B(�����l�͏e�̔���)
    public int num;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();//�������i�R���|�[�l���g�擾�j
        grabbable = GetComponent<OVRGrabbable_DeadCOPY>();
        Reload();
        ServerManager.glocks.Add(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        hundgunShot();
        if(ServerManager.glocks.id_list.Count == 0)
        {
            ServerManager.glocks.Add(this.gameObject);
        }
    }


    public void hundgunShot()//�ˌ��֐�
    {
        if (grabbable.isGrabbed)
        {
            //controller = grabbable.grabbedBy.GetController().tag;

        


        if (((grabbable.grabbedBy.GetController().tag == "lefthand") ? ServerManager.ExWebSocketBehavior.ctrl[grabbable.grabbedBy.getnum()].IsgetDown(Button_sm.r2) : ServerManager.ExWebSocketBehavior.ctrl[grabbable.grabbedBy.getnum()].IsgetDown(Button_sm.r1)))//Here I want to add more functions. using grab
            {
                if (Magazine > 0)//�c�e����
                {
                    anim.SetTrigger("Fire");//���C�A�j���[�V�����N��
                    Instantiate(bullet, bulletposition.transform.position, transform.rotation);
                    //Debug.LogError(ServerManager.bullets.objlist.Count);
                    //�e�ې���
                    //ServerManager.datalist[1] = bullet;
                     //Vibration.instance.StartVibration(frequency, amplitude, duration, controller);
                     MuzzleFlash.Play();
                    audioSource.PlayOneShot(ShotSound);//���C��
                    Magazine--;//�}�K�W���c�e�����炷
                    bulletNum.text = "" + Magazine;//UI�Ɏc�e����
                    ServerManager.ammo[num] = bulletNum.text; //�T�o�܂˂̒l�ύX������client�ɕ\�������
                }
                else//�c�e�Ȃ�
                {
                    audioSource.PlayOneShot(NoAmmoSound);//��
                    if (isReload == false)
                    {
                        bulletNum.text = "Reload";
                        ServerManager.ammo[num] = bulletNum.text;
                        isReload = true;
                        Invoke("Reload", ReloadSec);//Reload�֐����Ăяo��
                    }
                }
            }
        }

    }

    public void Reload()//�����[�h�֐�
    {
        Magazine = MagazineMax;//�e���[
        isReload = false;
        bulletNum.text = "" + Magazine;//UI�Ɏc�e����
        ServerManager.ammo[num] = bulletNum.text;
    }

   

    /*
     public OVRInput.Controller GetController()
    {

        return m_controller; 
    
    }
     */
}
