using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    public PlayerController controller;//ダメージを送信するクラス
    public Collider hitbox;//辺り判定
    [SerializeField][Range(0f, 2f)] float damageNum = 1f;//ダメージ倍率
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
        }//当たったのが弾ならダメージを取得して部位倍率をかけてダメージに代入
    }
}
