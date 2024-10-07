using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletContloller : MonoBehaviour
{
    public float speed;//スピード
    public float lifetime;//射程（時間で決める）
    public int damage = 10;


    // Start is called before the first frame update
    void Start()
    {
         Destroy(gameObject,lifetime);//ｎ秒後に消滅
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 force;
        force = gameObject.transform.forward * speed;
        GetComponent<Rigidbody>().AddForce(force);//生成されたら銃の向きに合わせて飛んでく
    }
}
