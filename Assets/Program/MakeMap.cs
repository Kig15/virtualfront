using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeMap : MonoBehaviour
{
    [SerializeField]
    [Tooltip("��������GameObject")]
    private GameObject[] createPrefab;
    [SerializeField]
    [Tooltip("��������͈�A")]
    private Transform rangeA;
    [SerializeField]
    [Tooltip("��������͈�B")]
    private Transform rangeB;
    [SerializeField]
    [Tooltip("�z�u����I�u�W�F�N�g�̌�")]
    public int[] ObjectNum = {0,0}; // �z�u����I�u�W�F�N�g�̌�
    [SerializeField]
    [Tooltip("�I�u�W�F�N�g�Ԃ̍ŏ�����")]
    private float minDistance = 1.2f; // �I�u�W�F�N�g�Ԃ̍ŏ�����

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
        // Obstacles�^�O���t�������ׂẴI�u�W�F�N�g���擾
        GameObject[] obstacles = ServerManager.obstacless.objlist.ToArray();
        // ���ׂẴI�u�W�F�N�g���폜
        foreach (GameObject obstacle in obstacles)
        {
            ServerManager.obstacless.Remove(obstacle);
            Destroy(obstacle);
        }

        // Obstacles�^�O���t�������ׂẴI�u�W�F�N�g���擾
        obstacles = ServerManager.obstacless2.objlist.ToArray();
        // ���ׂẴI�u�W�F�N�g���폜
        foreach (GameObject obstacle in obstacles)
        {
            ServerManager.obstacless2.Remove(obstacle);
            Destroy(obstacle);
        }

        // shieldModule�^�O���t�������ׂẴI�u�W�F�N�g���擾
        obstacles = ServerManager.sields.objlist.ToArray();
        // ���ׂẴI�u�W�F�N�g���폜
        foreach (GameObject obstacle in obstacles)
        {
            ServerManager.sields.Remove(obstacle);
            Destroy(obstacle);
        }

        // medical�^�O���t�������ׂẴI�u�W�F�N�g���擾
        obstacles = ServerManager.syringes.objlist.ToArray();
        // ���ׂẴI�u�W�F�N�g���폜
        foreach (GameObject obstacle in obstacles)
        {
            ServerManager.syringes.Remove(obstacle);
            Destroy(obstacle);
        }
    }

    public void MapGenerator()
    {
        // Obstacles�^�O���t�������ׂẴI�u�W�F�N�g���擾
        GameObject[] obstacles = ServerManager.obstacless.objlist.ToArray();
        // ���ׂẴI�u�W�F�N�g���폜
        foreach (GameObject obstacle in obstacles)
        {
            ServerManager.obstacless.Remove(obstacle);
            Destroy(obstacle);
        }

        // shieldModule�^�O���t�������ׂẴI�u�W�F�N�g���擾
        obstacles = ServerManager.sields.objlist.ToArray();
        // ���ׂẴI�u�W�F�N�g���폜
        foreach (GameObject obstacle in obstacles)
        {
            ServerManager.sields.Remove(obstacle);
            Destroy(obstacle);
        }

        // medical�^�O���t�������ׂẴI�u�W�F�N�g���擾
        obstacles = ServerManager.syringes.objlist.ToArray();
        // ���ׂẴI�u�W�F�N�g���폜
        foreach (GameObject obstacle in obstacles)
        {
            ServerManager.syringes.Remove(obstacle);
            Destroy(obstacle);
        }

        objectPositions.Clear(); // �ʒu���X�g�̃��Z�b�g
        for(int a = 0; a < ObjectNum.Length; a++)
        {


            for (int i = 0; i < ObjectNum[a]; i++)
            {

                Vector3 randomPosition;
                bool positionValid = false;

                // rangeA��rangeB��x���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float x = Random.Range(rangeA.position.x, rangeB.position.x);
                // rangeA��rangeB��y���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float y = Random.Range(rangeA.position.y, rangeB.position.y);
                // rangeA��rangeB��z���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float z = Random.Range(rangeA.position.z, rangeB.position.z);
                randomPosition = new Vector3(x, y, z);

                // �����̃I�u�W�F�N�g�Ƃ̋������m�F
                positionValid = true;
                foreach (Vector3 pos in objectPositions)
                {
                    if (Vector3.Distance(randomPosition, pos) < minDistance)
                    {
                        positionValid = false;
                        break;
                    }
                }



                // �L���Ȉʒu�ɃI�u�W�F�N�g�𐶐�
                if (positionValid)
                {
                    if (a == 0) {
                        ServerManager.obstacless.Add(Instantiate(createPrefab[a], randomPosition, createPrefab[a].transform.rotation));
                    }
                    else
                    {
                        ServerManager.obstacless2.Add(Instantiate(createPrefab[a], randomPosition, createPrefab[a].transform.rotation));
                    }
                    objectPositions.Add(randomPosition); // �ʒu�����X�g�ɒǉ�
                }
            }
        }

      
    }


}
