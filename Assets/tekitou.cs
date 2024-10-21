using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Unity.Netcode.Transports.UTP;
using UnityEngine.Networking;
public class Tekitou : NetworkBehaviour
{
    // Start is called before the first frame update

    public string serverIPAddress = "192.168.11.4";  // 接続したいサーバーのIPアドレス
    public ushort serverPort = 7777;  // 使用するポート番号
    void Start()
    {
        // var transport = NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        // if (transport is Unity.Netcode.Transports.UTP.UnityTransport unityTransport)
        // {
        //     // 接続先のIPアドレスとポートを指定
        //     unityTransport.SetConnectionData(ipAddress, port);
        // }
        // NetworkManager.Singleton.StartClient();
        var unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        // unityTransport.SetConnectionData(serverIPAddress, serverPort);

        // クライアントとして接続
        NetworkManager.Singleton.StartClient();
        // NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
        // Debug.Log(unityTransport.ConnectionData.Address);
        NetworkManager.Singleton.OnClientDisconnectCallback += (clientId) =>
        {
            Debug.Log("Client disconnected: " + clientId);
        };
        // string url = "http://google.com";
        // UnityWebRequest webRequest = UnityWebRequest.Get(url);
        // Debug.Log("webRequest: " + webRequest);
    }

    // Update is called once per frame
    void Update()
    {
        // // サーバーをホストとして起動するボタン
        // if (OVRInput.GetDown(OVRInput.Button.One))
        // {

        //     if (!NetworkManager.Singleton.IsHost)
        //     {
        //         NetworkManager.Singleton.StartHost();
        //     }
        //     else
        //     {
        //         Debug.LogWarning("Client is already running.");
        //     }
        //     gameObject.GetComponent<Renderer>().material.color = Color.blue;
        //     NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
        // }

        // // クライアントとしてサーバーに接続するボタン
        // else if (OVRInput.GetDown(OVRInput.Button.Two))
        // {
        //     // NetworkManager.Singleton.StartClient();
        //     if (!NetworkManager.Singleton.IsClient)
        //     {
        //         NetworkManager.Singleton.StartClient();
        //     }
        //     else
        //     {
        //         Debug.LogWarning("Client is already running.");
        //     }
        //     gameObject.GetComponent<Renderer>().material.color = Color.red;

        // }

        // // サーバーのみを起動するボタン（クライアントは接続しない）
        // else if (OVRInput.GetDown(OVRInput.Button.Three))
        // {
        //     NetworkManager.Singleton.StartServer();
        //     gameObject.GetComponent<Renderer>().material.color = Color.green;
        //     NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
        // }
        // else
        // {
        //     gameObject.GetComponent<Renderer>().material.color = Color.black;
        // }

        //clientでサーバーに接続できてるかを確認
        if (!NetworkManager.Singleton.IsClient)
        {
            Debug.Log("Client is disconnected.");
            var unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            unityTransport.SetConnectionData(serverIPAddress, serverPort);

            // クライアントとして接続
            NetworkManager.Singleton.StartClient();
        }

        // if(NetworkManager.Singleton.IsConnectedClient){
        //     Debug.Log("Client is connected.");
        // }
        // Oculus.Interaction.GrabInteractor.InjectSelector(OVRInput.Button.One);

    }


}
