using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletContloller : MonoBehaviour
{
    public float speed;//�X�s�[�h
    public float lifetime;//�˒��i���ԂŌ��߂�j
    public int damage = 10;


    // Start is called before the first frame update
    void Start()
    {
         Destroy(gameObject,lifetime);//���b��ɏ���
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 force;
        force = gameObject.transform.forward * speed;
        GetComponent<Rigidbody>().AddForce(force);//�������ꂽ��e�̌����ɍ��킹�Ĕ��ł�
    }
}
