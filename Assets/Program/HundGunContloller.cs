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
    public OVRGrabbable grabbable;
    private OVRInput.Controller controller;
    public OVRInput.Button shotButton;

    public int MagazineMax = 12;//�}�K�W���ő�e��
    public float ReloadSec;//�����[�h����


    private bool isReload = false;
    private int Magazine = 0;
   

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();//�������i�R���|�[�l���g�擾�j
        grabbable = GetComponent<OVRGrabbable>();
        Reload();
    }

    // Update is called once per frame
    void Update()
    {
       
        hundgunShot();
    }


    public void hundgunShot()//�ˌ��֐�
    {

        controller = grabbable.grabbedBy.GetController();
        if (grabbable.isGrabbed && OVRInput.GetDown(shotButton, controller))//Here I want to add more functions. using grab
            {
                if (Magazine > 0)//�c�e����
                {
                    anim.SetTrigger("Fire");//���C�A�j���[�V�����N��
                    Instantiate(bullet, bulletposition.transform.position, transform.rotation);//�e�ې���
                   
                    MuzzleFlash.Play();
                    audioSource.PlayOneShot(ShotSound);//���C��
                    Magazine--;//�}�K�W���c�e�����炷
                    bulletNum.text = "" + Magazine;//UI�Ɏc�e����
                }
                else//�c�e�Ȃ�
                {
                    audioSource.PlayOneShot(NoAmmoSound);//��
                    if (isReload == false)
                    {
                        bulletNum.text = "Reload";
                        isReload = true;
                        Invoke("Reload", ReloadSec);//Reload�֐����Ăяo��
                    }
                }
            }
        
    }

    public void Reload()//�����[�h�֐�
    {
        Magazine = MagazineMax;//�e���[
        isReload = false;
        bulletNum.text = "" + Magazine;//UI�Ɏc�e����
    }

   

    /*
     public OVRInput.Controller GetController()
    {

        return m_controller; 
    
    }
     */
}
