using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Preparation, // 準備フェーズ
        Battle,      // 戦闘フェーズ
        End          // 終了フェーズ
    }

    public static GameManager instance;// シングルトンインスタンス
    public GameObject[] players;// プレイヤーオブジェクト
    public PlayerController[] playerController;// プレイヤーコンポーネント(必ずplayersと同じ順番で入れること！)
    public MakeMap map;// マップオブジェクト
    public GameState currentState;// 現在のゲーム状態

    public GameObject[] PlayerRespownPoint;// プレイヤーのリスポーンポイント

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(GameState.Preparation);
    }

    // Update is called once per frame
    void Update()
    {

        // 状態が変わった時に呼ばれる処理
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

    // ゲームの状態を変更するメソッド
    public void ChangeState(GameState newState)
    {
        currentState = newState;
        Debug.Log("State changed to: " + newState);

        // 状態が変わった時に呼ばれる処理
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
        map.MapDestroyer();//マップを削除
        foreach (var player in players)
        {
            player.tag = "GhostTeam";//全員をゴーストチームに
           

        }
        foreach(var playerController in playerController)
        {
            playerController.isDead = false;//死んでいるか
            playerController.ButtleOK = false;//準備OKか
            playerController.isInvincible = true;//無敵か
            playerController.HealthUI.text = "Preparation Phase";
            playerController.PlayerHealth = playerController.PlayerMaxHealth;
            playerController.ChangeMaterial();

        }
        foreach(var playerRespownPoint in PlayerRespownPoint)
        {
            playerRespownPoint.SetActive(true);//リスポーンポイントを表示
        }
        // 準備フェーズに入った際の初期処理を記述

    }

    // 準備フェーズのアップデート処理
    void UpdatePreparation()
    {
        int OKPlayer = 0;
        // 準備フェーズ中の処理
        foreach (var playerController in playerController)
        {
            if (playerController.ButtleOK)
            {
                OKPlayer++;
            }
        }

        /* 準備が完了したら戦闘フェーズへ */
        if (OKPlayer == players.Length)
        {
            ChangeState(GameState.Battle);
        }
    }

    // 戦闘フェーズの開始時に呼ばれる処理
    void StartBattle()
    {
        Debug.Log("Battle Phase Started");
        foreach (var playerController in playerController)
        {

            playerController.HealthUI.text = "Ready...";//全員にバトルフェーズであることを表示

        }
        Invoke("BeganButtle", 3.0f);//3秒後にBeganButtleを呼び出す
        // 戦闘フェーズに入った際の初期処理を記述
    }

    void BeganButtle()
    {
        foreach (var playerController in playerController)
        {
            playerController.isInvincible = false;//全員の無敵を解除
            playerController.HealthUI.text = "100/100";//全員のHUDに体力を表示
        }
        foreach (var playerRespownPoint in PlayerRespownPoint)
        {
            playerRespownPoint.SetActive(false);//リスポーンポイントを非表示
        }
        map.MapGenerator();//マップを生成

    }

    // 戦闘フェーズのアップデート処理
    void UpdateBattle()
    {

        // 戦闘フェーズ中の処理
        int playerMenber = players.Length;
        foreach (var playerController in playerController)
        {
           if(playerController.isDead)
            {
                playerMenber--;
            }
        }
        if (playerMenber < players.Length)/* 戦闘が終了したら終了フェーズへ 1v1想定でつくってあるので一人減ったら終わり */
        {
            ChangeState(GameState.End);
        }
    }

    // 終了フェーズの開始時に呼ばれる処理
    void StartEnd()
    {
        Debug.Log("End Phase Started");
        // 終了フェーズに入った際の初期処理を記述
        foreach (var playerController in playerController)
        {
            playerController.isInvincible = true;//全員を無敵に
            if (!playerController.isDead)
            {
                playerController.HealthUI.text = "YOU WIN";
            }
        }
        Invoke("End", 10f);//3秒後にEndを呼び出す
    }

    void End()
    {
        ChangeState(GameState.Preparation);
    }

    // 終了フェーズのアップデート処理
    void UpdateEnd()
    {
        // 終了フェーズ中の処理
        if (true/* 終了処理が終わったら、例えばメニューに戻るなど */)
        {
            // ゲーム終了後の処理
        }
    }
}
