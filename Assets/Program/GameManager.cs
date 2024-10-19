using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Preparation, // �����t�F�[�Y
        Battle,      // �퓬�t�F�[�Y
        End          // �I���t�F�[�Y
    }

    public static GameManager instance;// �V���O���g���C���X�^���X
    public GameObject[] players;// �v���C���[�I�u�W�F�N�g
    public PlayerController[] playerController;// �v���C���[�R���|�[�l���g(�K��players�Ɠ������Ԃœ���邱�ƁI)
    public MakeMap map;// �}�b�v�I�u�W�F�N�g
    public GameState currentState;// ���݂̃Q�[�����

    public GameObject[] PlayerRespownPoint;// �v���C���[�̃��X�|�[���|�C���g

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(GameState.Preparation);
    }

    // Update is called once per frame
    void Update()
    {

        // ��Ԃ��ς�������ɌĂ΂�鏈��
        switch (currentState)
        {
            case GameState.Preparation:
                UpdatePreparation();
                break;
            case GameState.Battle:
                UpdateBattle();
                break;
            case GameState.End:
                UpdateEnd();
                break;
        }
    }

    // �Q�[���̏�Ԃ�ύX���郁�\�b�h
    public void ChangeState(GameState newState)
    {
        currentState = newState;
        Debug.Log("State changed to: " + newState);

        // ��Ԃ��ς�������ɌĂ΂�鏈��
        switch (newState)
        {
            case GameState.Preparation:
                StartPreparation();
                break;
            case GameState.Battle:
                StartBattle();
                break;
            case GameState.End:
                StartEnd();
                break;
        }
    }

    void StartPreparation()
    {
        Debug.Log("Preparation Phase Started");
        map.MapDestroyer();//�}�b�v���폜
        foreach (var player in players)
        {
            player.tag = "GhostTeam";//�S�����S�[�X�g�`�[����
           

        }
        foreach(var playerController in playerController)
        {
            playerController.isDead = false;//����ł��邩
            playerController.ButtleOK = false;//����OK��
            playerController.isInvincible = true;//���G��
            playerController.HealthUI.text = "Preparation Phase";
            playerController.PlayerHealth = playerController.PlayerMaxHealth;
            playerController.ChangeMaterial();

        }
        foreach(var playerRespownPoint in PlayerRespownPoint)
        {
            playerRespownPoint.SetActive(true);//���X�|�[���|�C���g��\��
        }
        // �����t�F�[�Y�ɓ������ۂ̏����������L�q

    }

    // �����t�F�[�Y�̃A�b�v�f�[�g����
    void UpdatePreparation()
    {
        int OKPlayer = 0;
        // �����t�F�[�Y���̏���
        foreach (var playerController in playerController)
        {
            if (playerController.ButtleOK)
            {
                OKPlayer++;
            }
        }

        /* ����������������퓬�t�F�[�Y�� */
        if (OKPlayer == players.Length)
        {
            ChangeState(GameState.Battle);
        }
    }

    // �퓬�t�F�[�Y�̊J�n���ɌĂ΂�鏈��
    void StartBattle()
    {
        Debug.Log("Battle Phase Started");
        foreach (var playerController in playerController)
        {

            playerController.HealthUI.text = "Ready...";//�S���Ƀo�g���t�F�[�Y�ł��邱�Ƃ�\��

        }
        Invoke("BeganButtle", 3.0f);//3�b���BeganButtle���Ăяo��
        // �퓬�t�F�[�Y�ɓ������ۂ̏����������L�q
    }

    void BeganButtle()
    {
        foreach (var playerController in playerController)
        {
            playerController.isInvincible = false;//�S���̖��G������
            playerController.HealthUI.text = "100/100";//�S����HUD�ɑ̗͂�\��
        }
        foreach (var playerRespownPoint in PlayerRespownPoint)
        {
            playerRespownPoint.SetActive(false);//���X�|�[���|�C���g���\��
        }
        map.MapGenerator();//�}�b�v�𐶐�

    }

    // �퓬�t�F�[�Y�̃A�b�v�f�[�g����
    void UpdateBattle()
    {

        // �퓬�t�F�[�Y���̏���
        int playerMenber = players.Length;
        foreach (var playerController in playerController)
        {
           if(playerController.isDead)
            {
                playerMenber--;
            }
        }
        if (playerMenber < players.Length)/* �퓬���I��������I���t�F�[�Y�� 1v1�z��ł����Ă���̂ň�l��������I��� */
        {
            ChangeState(GameState.End);
        }
    }

    // �I���t�F�[�Y�̊J�n���ɌĂ΂�鏈��
    void StartEnd()
    {
        Debug.Log("End Phase Started");
        // �I���t�F�[�Y�ɓ������ۂ̏����������L�q
        foreach (var playerController in playerController)
        {
            playerController.isInvincible = true;//�S���𖳓G��
            if (!playerController.isDead)
            {
                playerController.HealthUI.text = "YOU WIN";
            }
        }
        Invoke("End", 10f);//3�b���End���Ăяo��
    }

    void End()
    {
        ChangeState(GameState.Preparation);
    }

    // �I���t�F�[�Y�̃A�b�v�f�[�g����
    void UpdateEnd()
    {
        // �I���t�F�[�Y���̏���
        if (true/* �I���������I�������A�Ⴆ�΃��j���[�ɖ߂�Ȃ� */)
        {
            // �Q�[���I����̏���
        }
    }
}
