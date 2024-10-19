using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletContloller : MonoBehaviour
{
    public float speed;//スピード
    public float lifetime;//射程（時間で決める）
    public int damage = 10;
    public BoxCollider thisColider;


    // Start is called before the first frame update
    void Start()
    {
         Destroy(gameObject,lifetime);//ｎ秒後に消滅
         if(ServerManager.bullets != null)ServerManager.bullets.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 force;
        force = gameObject.transform.forward * speed;
        GetComponent<Rigidbody>().AddForce(force);//生成されたら銃の向きに合わせて飛んでく
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
