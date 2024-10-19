using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesController : MonoBehaviour
{
    public GameObject[] Prefabs; // ��������v���t�@�u�̔z��
    private int number,number2; // �����_���ɑI�΂ꂽ�v���t�@�u�̃C���f�b�N�X
    public GameObject[] SpownPoint; // ��������ʒu
    private GameObject SpownedGameObject; // �������ꂽGameObject
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
        // �v���t�@�u�̔z��̒������擾
        // �v���t�@�u�̔z��̒������烉���_���ȃC���f�b�N�X���擾
        number = Random.Range(0, Prefabs.Length);
        number2 = Random.Range(0, SpownPoint.Length);
        // Y���̉�]�������_���Ȋp�x�ɐݒ�
        float randomYRotation = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, randomYRotation, transform.rotation.eulerAngles.z);
        // �v���t�@�u�𐶐�
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
