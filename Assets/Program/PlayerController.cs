using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float PlayerHealth =0 ;
    public float PlayerMaxHealth = 100;
    public TextMeshPro HealthUI;//体力のUI
    public float HealPerFlame = 0.16f;
    public bool isHealing;
    public bool ChangeTeam;
    public bool isDead;//死亡フラグ
    public bool ButtleOK;//戦闘可能フラグ
    public bool isInvincible;//無敵フラグs
    [SerializeField] 
    Material[] materialArray;//0はRed 1はBlue
    [SerializeField]
    Material DefaultMaterial;
    [SerializeField]
    GameObject[] HitBoxList;
    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth = PlayerMaxHealth;//プレイヤーの体力をMAXに
    }

    // Update is called once per frame
    void Update()
    {
        Heal();
    }

    public void dagame(float damagenum)
    {
        if(isInvincible)
        {
            return;
        }//無敵状態ならダメージを受けない

        if (PlayerHealth - damagenum > 0)
        {
            PlayerHealth -= damagenum;
            HealthUI.text = PlayerHealth + "/" + PlayerMaxHealth;
        }
        else if (PlayerHealth - damagenum <= 0) 
        {
            PlayerHealth = 0;
            HealthUI.text = "YOU DIED";
            isDead = true;
        }//引数にダメージを入れて体力から引く、0になったら死亡
    }

    public void Heal()
    {
        if (isDead)
        {
            return;//死亡フラグが立っていたら回復しない
        }

        if (isHealing)
        {
            if (PlayerHealth < PlayerMaxHealth)
            {
                PlayerHealth += HealPerFlame;
                HealthUI.text = (int)PlayerHealth + "/" + PlayerMaxHealth;
            }
            else
            {
                PlayerHealth = PlayerMaxHealth;
                HealthUI.text = (int)PlayerHealth + "/" + PlayerMaxHealth;
                isHealing = false;
            }
        }//ishealingがtrueだと秒間１０づつ回復（MAXになったら回復が止まる）

    }

    public void ChangeMaterial(int materialnum = 2)
    {
        tag = gameObject.tag;
        switch(tag)
        {
            case "RedTeam":
                materialnum = 0;
                break;
            case "BlueTeam":
                materialnum = 1;
                break;
            default:
                foreach (var hitbox in HitBoxList)
                {
                    hitbox.GetComponent<Renderer>().material = DefaultMaterial;
                }
                ChangeTeam = false;
                return;
        }
       
            foreach (var hitbox in HitBoxList)
            {
                hitbox.GetComponent<Renderer>().material = materialArray[materialnum];
            }
            ChangeTeam = false;
        
       
    }//引数にマテリアルの番号を入れるとそのマテリアルに変わる
   
}
