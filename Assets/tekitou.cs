using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class tekitou : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // var transport = NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        // if (transport is Unity.Netcode.Transports.UTP.UnityTransport unityTransport)
        // {
        //     // 接続先のIPアドレスとポートを指定
        //     unityTransport.SetConnectionData(ipAddress, port);
        // }
        // NetworkManager.Singleton.StartClient();
    }

    // Update is called once per frame
    void Update()
    {
        // サーバーをホストとして起動するボタン
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            NetworkManager.Singleton.StartHost();
        }

        // クライアントとしてサーバーに接続するボタン
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            NetworkManager.Singleton.StartClient();
        }

        // サーバーのみを起動するボタン（クライアントは接続しない）
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            NetworkManager.Singleton.StartServer();
        }
    }
}
