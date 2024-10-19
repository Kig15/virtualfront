using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeMap : MonoBehaviour
{
    [SerializeField]
    [Tooltip("生成するGameObject")]
    private GameObject[] createPrefab;
    [SerializeField]
    [Tooltip("生成する範囲A")]
    private Transform rangeA;
    [SerializeField]
    [Tooltip("生成する範囲B")]
    private Transform rangeB;
    [SerializeField]
    [Tooltip("配置するオブジェクトの個数")]
    public int[] ObjectNum = {0,0}; // 配置するオブジェクトの個数
    [SerializeField]
    [Tooltip("オブジェクト間の最小距離")]
    private float minDistance = 1.2f; // オブジェクト間の最小距離

    private List<Vector3> objectPositions = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MapDestroyer()
    {
        // Obstaclesタグが付いたすべてのオブジェクトを取得
        GameObject[] obstacles = ServerManager.obstacless.objlist.ToArray();
        // すべてのオブジェクトを削除
        foreach (GameObject obstacle in obstacles)
        {
            ServerManager.obstacless.Remove(obstacle);
            Destroy(obstacle);
        }

        // Obstaclesタグが付いたすべてのオブジェクトを取得
        obstacles = ServerManager.obstacless2.objlist.ToArray();
        // すべてのオブジェクトを削除
        foreach (GameObject obstacle in obstacles)
        {
            ServerManager.obstacless2.Remove(obstacle);
            Destroy(obstacle);
        }

        // shieldModuleタグが付いたすべてのオブジェクトを取得
        obstacles = ServerManager.sields.objlist.ToArray();
        // すべてのオブジェクトを削除
        foreach (GameObject obstacle in obstacles)
        {
            ServerManager.sields.Remove(obstacle);
            Destroy(obstacle);
        }

        // medicalタグが付いたすべてのオブジェクトを取得
        obstacles = ServerManager.syringes.objlist.ToArray();
        // すべてのオブジェクトを削除
        foreach (GameObject obstacle in obstacles)
        {
            ServerManager.syringes.Remove(obstacle);
            Destroy(obstacle);
        }
    }

    public void MapGenerator()
    {
        // Obstaclesタグが付いたすべてのオブジェクトを取得
        GameObject[] obstacles = ServerManager.obstacless.objlist.ToArray();
        // すべてのオブジェクトを削除
        foreach (GameObject obstacle in obstacles)
        {
            ServerManager.obstacless.Remove(obstacle);
            Destroy(obstacle);
        }

        // shieldModuleタグが付いたすべてのオブジェクトを取得
        obstacles = ServerManager.sields.objlist.ToArray();
        // すべてのオブジェクトを削除
        foreach (GameObject obstacle in obstacles)
        {
            ServerManager.sields.Remove(obstacle);
            Destroy(obstacle);
        }

        // medicalタグが付いたすべてのオブジェクトを取得
        obstacles = ServerManager.syringes.objlist.ToArray();
        // すべてのオブジェクトを削除
        foreach (GameObject obstacle in obstacles)
        {
            ServerManager.syringes.Remove(obstacle);
            Destroy(obstacle);
        }

        objectPositions.Clear(); // 位置リストのリセット
        for(int a = 0; a < ObjectNum.Length; a++)
        {


            for (int i = 0; i < ObjectNum[a]; i++)
            {

                Vector3 randomPosition;
                bool positionValid = false;

                // rangeAとrangeBのx座標の範囲内でランダムな数値を作成
                float x = Random.Range(rangeA.position.x, rangeB.position.x);
                // rangeAとrangeBのy座標の範囲内でランダムな数値を作成
                float y = Random.Range(rangeA.position.y, rangeB.position.y);
                // rangeAとrangeBのz座標の範囲内でランダムな数値を作成
                float z = Random.Range(rangeA.position.z, rangeB.position.z);
                randomPosition = new Vector3(x, y, z);

                // 既存のオブジェクトとの距離を確認
                positionValid = true;
                foreach (Vector3 pos in objectPositions)
                {
                    if (Vector3.Distance(randomPosition, pos) < minDistance)
                    {
                        positionValid = false;
                        break;
                    }
                }



                // 有効な位置にオブジェクトを生成
                if (positionValid)
                {
                    if (a == 0) {
                        ServerManager.obstacless.Add(Instantiate(createPrefab[a], randomPosition, createPrefab[a].transform.rotation));
                    }
                    else
                    {
                        ServerManager.obstacless2.Add(Instantiate(createPrefab[a], randomPosition, createPrefab[a].transform.rotation));
                    }
                    objectPositions.Add(randomPosition); // 位置をリストに追加
                }
            }
        }

      
    }


}
