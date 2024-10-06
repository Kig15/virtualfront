using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : NetworkBehaviour
{
    public void StartHost()
    {
        //ホスト開始
        // NetworkManager.Singleton.StartHost();
        Debug.Log(NetworkManager.Singleton.StartHost());
        //シーンを切り替え
        NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
        NetworkManager.Singleton.OnClientDisconnectCallback += (clientId) =>
        {
            Debug.Log("Client disconnected: " + clientId);
        };
    }

    public void StartClient()
    {
        //ホストに接続
        NetworkManager.Singleton.StartClient();
    }
}