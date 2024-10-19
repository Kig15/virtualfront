using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesController : MonoBehaviour
{
    public GameObject[] Prefabs; // 生成するプレファブの配列
    private int number,number2; // ランダムに選ばれたプレファブのインデックス
    public GameObject[] SpownPoint; // 生成する位置
    private GameObject SpownedGameObject; // 生成されたGameObject
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        // プレファブの配列の長さを取得
        // プレファブの配列の長さからランダムなインデックスを取得
        number = Random.Range(0, Prefabs.Length);
        number2 = Random.Range(0, SpownPoint.Length);
        // Y軸の回転をランダムな角度に設定
        float randomYRotation = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, randomYRotation, transform.rotation.eulerAngles.z);
        // プレファブを生成
        SpownedGameObject = Prefabs[number];
       
        switch (number)
        {
            case 0:
                ServerManager.sields.Add(Instantiate(SpownedGameObject, SpownPoint[number2].transform.position, SpownedGameObject.transform.rotation));
                break;
            case 1:
                ServerManager.syringes.Add(Instantiate(SpownedGameObject, SpownPoint[number2].transform.position, SpownedGameObject.transform.rotation));
                break;
            default:
                Debug.Log("Error");
                break;
        }
    }


  
}
