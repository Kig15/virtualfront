using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;

public class NetworkManagerUI : MonoBehaviour
{
    public InputField ipAddressInputField;  // UIからIPアドレスを取得するためのInputField
    public UnityTransport transport;  // UnityTransportを設定するために必要

    // サーバー/クライアントを開始する
    public void StartHost()
    {
        ipAddressInputField = GameObject.Find("InputField").GetComponent<InputField>();
        Debug.Log(ipAddressInputField.text);
        var unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        unityTransport.SetConnectionData(ipAddressInputField.text, 7777);
        NetworkManager.Singleton.StartHost();  // ホストを開始
        NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void StartServer()
    {
        transport.ConnectionData.Address = ipAddressInputField.text; // IPアドレスをセット
        NetworkManager.Singleton.StartServer();  // サーバーを開始
    }

    public void StartClient()
    {
        transport.ConnectionData.Address = ipAddressInputField.text; // IPアドレスをセット
        NetworkManager.Singleton.StartClient();  // クライアントを開始
    }
}
