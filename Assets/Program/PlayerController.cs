using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float PlayerHealth =0 ;
    public float PlayerMaxHealth = 100;
    public TextMeshPro HealthUI;//�̗͂�UI
    public float HealPerFlame = 0.16f;
    public bool isHealing;
    public bool ChangeTeam;
    public bool isDead;//���S�t���O
    public bool ButtleOK;//�퓬�\�t���O
    public bool isInvincible;//���G�t���Os
    [SerializeField] 
    Material[] materialArray;//0��Red 1��Blue
    [SerializeField]
    Material DefaultMaterial;
    [SerializeField]
    GameObject[] HitBoxList;
    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth = PlayerMaxHealth;//�v���C���[�̗̑͂�MAX��
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
        }//���G��ԂȂ�_���[�W���󂯂Ȃ�

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
        }//�����Ƀ_���[�W�����đ̗͂�������A0�ɂȂ����玀�S
    }

    public void Heal()
    {
        if (isDead)
        {
            return;//���S�t���O�������Ă�����񕜂��Ȃ�
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
        }//ishealing��true���ƕb�ԂP�O�Â񕜁iMAX�ɂȂ�����񕜂��~�܂�j

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
        
       
    }//�����Ƀ}�e���A���̔ԍ�������Ƃ��̃}�e���A���ɕς��
   
}
