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
    
    
    public GameObject bullet, bulletposition;//弾丸オブジェクト　弾丸発射位置
    public AudioClip ShotSound, NoAmmoSound;//発砲音　弾切れ音
    public AudioSource audioSource;//音声管理
    public Animator anim;//アニメーション管理
    public TextMeshPro bulletNum;//物理的なUI
    public ParticleSystem MuzzleFlash;//マズルフラッシュエフェクト
    public OVRGrabbable grabbable;
    private OVRInput.Controller controller;
    public OVRInput.Button shotButton;

    public int MagazineMax = 12;//マガジン最大容量
    public float ReloadSec;//リロード時間


    private bool isReload = false;
    private int Magazine = 0;
   

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();//初期化（コンポーネント取得）
        grabbable = GetComponent<OVRGrabbable>();
        Reload();
    }

    // Update is called once per frame
    void Update()
    {
       
        hundgunShot();
    }


    public void hundgunShot()//射撃関数
    {

        controller = grabbable.grabbedBy.GetController();
        if (grabbable.isGrabbed && OVRInput.GetDown(shotButton, controller))//Here I want to add more functions. using grab
            {
                if (Magazine > 0)//残弾あり
                {
                    anim.SetTrigger("Fire");//発砲アニメーション起動
                    Instantiate(bullet, bulletposition.transform.position, transform.rotation);//弾丸生成
                   
                    MuzzleFlash.Play();
                    audioSource.PlayOneShot(ShotSound);//発砲音
                    Magazine--;//マガジン残弾を減らす
                    bulletNum.text = "" + Magazine;//UIに残弾を代入
                }
                else//残弾なし
                {
                    audioSource.PlayOneShot(NoAmmoSound);//空音
                    if (isReload == false)
                    {
                        bulletNum.text = "Reload";
                        isReload = true;
                        Invoke("Reload", ReloadSec);//Reload関数を呼び出す
                    }
                }
            }
        
    }

    public void Reload()//リロード関数
    {
        Magazine = MagazineMax;//弾薬補充
        isReload = false;
        bulletNum.text = "" + Magazine;//UIに残弾を代入
    }

   

    /*
     public OVRInput.Controller GetController()
    {

        return m_controller; 
    
    }
     */
}
