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
    public OVRGrabbable_DeadCOPY grabbable;
    private OVRInput.Controller controller;
    public OVRInput.Button shotButton;

    public int MagazineMax = 12;//マガジン最大容量
    public float ReloadSec;//リロード時間


    private bool isReload = false;
    private int Magazine = 0;

    private float frequency = 1.0f;//コントローラーの振動
    private float amplitude = 1f;//バイブレーションの強度を指定します。0は振動なし、1は最大強度を意味します
    private float duration = 0.1f;//振動が持続する時間を指定します（秒数）。(初期値は銃の反動)
    public int num;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();//初期化（コンポーネント取得）
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


    public void hundgunShot()//射撃関数
    {
        if (grabbable.isGrabbed)
        {
            //controller = grabbable.grabbedBy.GetController().tag;

        


        if (((grabbable.grabbedBy.GetController().tag == "lefthand") ? ServerManager.ExWebSocketBehavior.ctrl[grabbable.grabbedBy.getnum()].IsgetDown(Button_sm.r2) : ServerManager.ExWebSocketBehavior.ctrl[grabbable.grabbedBy.getnum()].IsgetDown(Button_sm.r1)))//Here I want to add more functions. using grab
            {
                if (Magazine > 0)//残弾あり
                {
                    anim.SetTrigger("Fire");//発砲アニメーション起動
                    Instantiate(bullet, bulletposition.transform.position, transform.rotation);
                    //Debug.LogError(ServerManager.bullets.objlist.Count);
                    //弾丸生成
                    //ServerManager.datalist[1] = bullet;
                     //Vibration.instance.StartVibration(frequency, amplitude, duration, controller);
                     MuzzleFlash.Play();
                    audioSource.PlayOneShot(ShotSound);//発砲音
                    Magazine--;//マガジン残弾を減らす
                    bulletNum.text = "" + Magazine;//UIに残弾を代入
                    ServerManager.ammo[num] = bulletNum.text; //サバまねの値変更したらclientに表示される
                }
                else//残弾なし
                {
                    audioSource.PlayOneShot(NoAmmoSound);//空音
                    if (isReload == false)
                    {
                        bulletNum.text = "Reload";
                        ServerManager.ammo[num] = bulletNum.text;
                        isReload = true;
                        Invoke("Reload", ReloadSec);//Reload関数を呼び出す
                    }
                }
            }
        }

    }

    public void Reload()//リロード関数
    {
        Magazine = MagazineMax;//弾薬補充
        isReload = false;
        bulletNum.text = "" + Magazine;//UIに残弾を代入
        ServerManager.ammo[num] = bulletNum.text;
    }

   

    /*
     public OVRInput.Controller GetController()
    {

        return m_controller; 
    
    }
     */
}
