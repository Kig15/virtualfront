using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    public PlayerController controller;//�_���[�W�𑗐M����N���X
    public Collider hitbox;//�ӂ蔻��
    [SerializeField][Range(0f, 2f)] float damageNum = 1f;//�_���[�W�{��
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "bullet")
        {
            controller.dagame((float)other.gameObject.GetComponent<BulletContloller>().damage * damageNum);
        }//���������̂��e�Ȃ�_���[�W���擾���ĕ��ʔ{���������ă_���[�W�ɑ��
    }
}
