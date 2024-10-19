using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandShieldController : MonoBehaviour
{
    public int ShieldHelth;//シールドの体力管理
    public int ShieldHelthMax;//シールドの最大体力
    public int OVERHEATsec = 10;//シールドのオーバーヒート時間
    public TextMeshPro ShieldUI;//物理的なUI
    public GameObject Shield;//シールド
    public AudioSource audioSource;//音声管理
    public AudioClip EnableSound, DisableSound,OverHeatSound;//展開音　格納サウンド　オーバーヒート音
    public MeshCollider MeshCollider;//辺り判定
    public OVRGrabbable_DeadCOPY grabbable;
    private OVRInput.Controller controller;
    public OVRInput.Button shotButton;
    public bool isactive;

    private bool isOVERHEAT;
    private float frequency = 1.0f;//コントローラーの振動
    private float amplitude = 0.5f;//バイブレーションの強度を指定します。0は振動なし、1は最大強度を意味します
    private float duration = 0.3f;//振動が持続する時間を指定します（秒数）。(初期値は銃の反動)

    // Start is called before the first frame update
    void Start()
    {
        ShieldHelth = ShieldHelthMax;
        ShieldUI.text = "" + ShieldHelth;
        grabbable = GetComponent<OVRGrabbable_DeadCOPY>();
        audioSource = GetComponent<AudioSource>();//初期化（コンポーネント取得）
        Shield.SetActive(false);//・通常はシールド部分は非表示gameObject.SetActive(false);になっている
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
            }//掴んだらコントローラー取得
            if (grabbable.isGrabbed && OVRInput.GetDown(shotButton, controller))
            {
                audioSource.PlayOneShot(EnableSound);//発砲音
                Vibration.instance.StartVibration(frequency, amplitude, duration, controller);
                Shield.SetActive(true);
                isactive = true;
            }//トリガーを押した瞬間の処理
            if (grabbable.isGrabbed && OVRInput.Get(shotButton, controller))
            {

                Shield.SetActive(true);//・それをトリガーを引いている間だけ表示gameObject.SetActive(true);にしている
                isactive = true;

                return;
            }//押している間の処理
            else if (grabbable.isGrabbed && OVRInput.GetUp(shotButton, controller))
            {
                audioSource.PlayOneShot(DisableSound);//発砲音
                Shield.SetActive(false);
                isactive = false;
            }//放す瞬間の処理
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
            }//シールドの体力が0になったらOverHeat
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
        }//当たったのが弾だったらダメージを参照して被弾 被弾判定はここ
    }
}
