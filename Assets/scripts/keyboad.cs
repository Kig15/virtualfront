using UnityEngine;
using Oculus.Platform;
using Oculus.Platform.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;

public class KeyboardInputHandler : MonoBehaviour
{
    public void OnCommitText(string inputText)
    {
        Debug.Log("Inputted text: " + inputText);
        // ここで入力テキストを使ってIPアドレス設定や他の処理を行う
        // 例: IPアドレスとして設定する
        ConnectToServer(inputText);
    }

    void ConnectToServer(string ipAddress)
    {
        // ここに接続処理を書く
        var unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        unityTransport.SetConnectionData(ipAddress, 7777);
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}