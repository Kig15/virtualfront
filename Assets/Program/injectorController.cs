using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class injectorController : MonoBehaviour
{
    public GameObject OVRCamera;//プレイヤー（PlayerControllerを持ったオブジェクトをプレイヤーとして取得したいだけ、OVRCameraは気にしなくていい）
    public PlayerController PlayerController;//プレイヤーのコード
    public OVRGrabbable_DeadCOPY grabbable;
    //private OVRInput.Controller controller;
    //public OVRInput.Button shotButton;//3つは掴む判定に必要なもの
    public AudioSource audioSource;//音声管理
    public AudioClip UseSound;//注射器使用音
    public float DestroySec = 2.0f;

    private float frequency = 1.0f;//コントローラーの振動
    private float amplitude = 0.8f;//バイブレーションの強度を指定します。0は振動なし、1は最大強度を意味します
    private float duration = 0.5f;//振動が持続する時間を指定します（秒数）。(初期値は銃の反動)
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

    public void injectUse()//注射器使用関数
    {
        if (isUseable)
        {
            if (grabbable.isGrabbed)
            {
                //controller = grabbable.grabbedBy.GetController();
                if (((grabbable.grabbedBy.GetController().tag == "lefthand") ? ServerManager.ExWebSocketBehavior.ctrl[grabbable.grabbedBy.getnum()].IsgetDown(Button_sm.r2) : ServerManager.ExWebSocketBehavior.ctrl[grabbable.grabbedBy.getnum()].IsgetDown(Button_sm.r1)))
                {
                    PlayerController.isHealing = true;//プレイヤークラスのヒールをON
                    audioSource.PlayOneShot(UseSound);//音鳴らして
                                                      // Vibration.instance.StartVibration(frequency, amplitude, duration, controller);//コントローラー振動
                                                      //使用したら使用不可にしてn秒後に消える処理
                    isUseable = false;
                    Invoke("DestroyMe", DestroySec);
                }

            }//掴んでいたらコントロ－ラーを取得
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
            PlayerController = other.transform.root.gameObject.GetComponent<PlayerController>();//接触したプレイヤーのプレイヤークラスを取得
        }
    }
 }
