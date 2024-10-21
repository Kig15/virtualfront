using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : NetworkBehaviour
{
    //オブジェクトと結びつける
    public InputField inputField;
    public Text text;

    void Start()
    {
        //Componentを扱えるようにする
        inputField = inputField.GetComponent<InputField>();
        text = text.GetComponent<Text>();

    }

    public void Textinput(){
        StartHost(text.text);
    }
    public void StartHost(string ip)
    {
        //ホスト開始
        // NetworkManager.Singleton.StartHost();
        var unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        unityTransport.SetConnectionData(ip, 7777);
        if(NetworkManager.Singleton.IsHost){
            Debug.Log("Host is already running.");
            NetworkManager.Singleton.Shutdown();
        }else if(NetworkManager.Singleton.IsClient){
            Debug.Log("Client is already running.");
            NetworkManager.Singleton.Shutdown();
        }

        Debug.Log(NetworkManager.Singleton.StartHost());
        //シーンを切り替え
        NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
        NetworkManager.Singleton.OnClientDisconnectCallback += (clientId) =>
        {
            Debug.Log("Client disconnected: " + clientId);
        };
    }
    public void StartHost()
    {
        //ホスト開始
        // NetworkManager.Singleton.StartHost();
        var unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        unityTransport.SetConnectionData("192.168.11.4", 7777);
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