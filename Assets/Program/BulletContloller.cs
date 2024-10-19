using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletContloller : MonoBehaviour
{
    public float speed;//�X�s�[�h
    public float lifetime;//�˒��i���ԂŌ��߂�j
    public int damage = 10;
    public BoxCollider thisColider;


    // Start is called before the first frame update
    void Start()
    {
         Destroy(gameObject,lifetime);//���b��ɏ���
         if(ServerManager.bullets != null)ServerManager.bullets.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 force;
        force = gameObject.transform.forward * speed;
        GetComponent<Rigidbody>().AddForce(force);//�������ꂽ��e�̌����ɍ��킹�Ĕ��ł�
    }

    public void OnDestroy()
    {
        if (ServerManager.bullets != null) ServerManager.bullets.Remove(gameObject);
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (!(other.gameObject.tag == "GameController") || !(other.gameObject.tag == "Items") || !(other.gameObject.tag == "bullet"))
        {
           
            Destroy(gameObject);
        }
    }
}
