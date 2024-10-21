// using UnityEngine;
// using WebSocketSharp;
// using Newtonsoft.Json;
// using UnityEngine.SceneManagement;
// using System.Threading.Tasks;
// using System.Collections.Generic;
// using System.Net.Sockets;
// using System.Text;
// using System;
// using System.Net;

// public class UdpClientManager : MonoBehaviour
// {
//     private UdpClient udpClient;
//     // public Text chatText;
//     // public Button sendButton;
//     // public InputField messageInput;
//     public string ip = "192.168.11.4";
//     public int port = 12345;
//     public GameObject[] datalist;
//     public Senddata senddata;
//     public Objdat objdata;
//     public Ctrl ctrl;
//     public GameObject leftctrl;
//     public GameObject rightctrl;
//     public Objdat recdata;
//     public Objdat[] reclist;
//     private async Task WaitOneSecond()
//     {
//         await Task.Delay(1000);
//     }
//     //サーバへ、メッセージを送信する
//     public void Update()
//     {
//         // data = SceneManager.GetActiveScene().GetRootGameObjects();
//         objdata ??= new Objdat();
//         // obj = GameObject.Find("Cube");

//         //スペースキーが押されたら
//         // if (Input.GetKeyDown(KeyCode.Space))
//         // {
//         //     data[0].transform.position = new Vector3(data[0].transform.position.x + 1f, 0, 0);
//         // }
//         senddata ??= new Senddata();
//         objdata.id = 0;
//         objdata.position.x = rightctrl.transform.position.x;
//         objdata.position.y = rightctrl.transform.position.y;
//         objdata.position.z = rightctrl.transform.position.z;
//         objdata.rotation.x = rightctrl.transform.rotation.x;
//         objdata.rotation.y = rightctrl.transform.rotation.y;
//         objdata.rotation.z = rightctrl.transform.rotation.z;
//         senddata.type = "right";
//         senddata.data = JsonConvert.SerializeObject(objdata);
//         Senddat(JsonConvert.SerializeObject(senddata));
//         objdata.id = 1;
//         objdata.position.x = leftctrl.transform.position.x;
//         objdata.position.y = leftctrl.transform.position.y;
//         objdata.position.z = leftctrl.transform.position.z;
//         objdata.rotation.x = leftctrl.transform.rotation.x;
//         objdata.rotation.y = leftctrl.transform.rotation.y;
//         objdata.rotation.z = leftctrl.transform.rotation.z;
//         senddata.type = "left";
//         senddata.data = JsonConvert.SerializeObject(objdata);
//         Senddat(JsonConvert.SerializeObject(senddata));
//         // Debug.Log(recdata.data.name);
//         // foreach (OVRInput.OVRControllerBase i in OVRInput.controllers){
//         //     if(i != null){continue;}
//         //     // i.currentState.RHandTrigger = OVRInput.Get(OVRInput.Button.One) ? 1.0f : 0.0f;
//         //     // Debug.Log("button1 :" + (i.currentState.Buttons >> 1 & 1) + " button2 :" + (i.currentState.Buttons >> 2 & 1) + " button3 :" + (i.currentState.Buttons >> 3 & 1) + " button4 :" + (i.currentState.Buttons >> 4 & 1) + " R1 :" + i.currentState.RIndexTrigger + " R2 :" + i.currentState.RHandTrigger + " L1 :" + i.currentState.LIndexTrigger + " L2 :" + i.currentState.LHandTrigger);
//         // }

//         //send ctrl information
//         ctrl.a = OVRInput.Get(OVRInput.Button.One);
//         ctrl.b = OVRInput.Get(OVRInput.Button.Two);
//         ctrl.x = OVRInput.Get(OVRInput.Button.Three);
//         ctrl.y = OVRInput.Get(OVRInput.Button.Four);
//         ctrl.R1 = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
//         ctrl.R2 = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
//         ctrl.L1 = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger);
//         ctrl.L2 = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger);
//         senddata.type = "action";
//         senddata.data = JsonConvert.SerializeObject(ctrl);
//         Senddat(JsonConvert.SerializeObject(senddata));
//         // Debug.Log(recdata.id);
//         // datalist[recdata.id].transform.position = new Vector3(recdata.position.x, recdata.position.y, recdata.position.z);
//         // datalist[recdata.id].transform.rotation = new Quaternion(recdata.rotation.x, recdata.rotation.y, recdata.rotation.z, 0);
//         foreach (Objdat i in reclist){
//             datalist[i.id].transform.SetPositionAndRotation(new Vector3(i.position.x, i.position.y, i.position.z), new Quaternion(i.rotation.x, i.rotation.y, i.rotation.z, 0));
//         }
//     }
//     public void RecvText(string text)
//     {
//         // Debug.Log(text);
//         var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(text);
//         string type = jsonObject["type"].ToString();
//         // int seq = int.Parse(jsonObject["seq"].ToString());
//         // Debug.Log(type);
//         // if (type != "data") { Debug.Log(text); }
//         switch (type)
//         {
//             // case "login":
//             //     clientdata = JsonConvert.DeserializeObject<Clientdata>(jsonObject["data"].ToString());
//             //     Debug.Log(clientdata.seq);
//             //     break;
//             case "data":
//                 // Debug.Log(seq + " : " + clientdata.seq);
//                 // Debug.Log(text);
//                 Debug.Log(jsonObject["data"].ToString());
//                 recdata = JsonConvert.DeserializeObject<Objdat>(jsonObject["data"].ToString());
//                 reclist[recdata.id] = recdata;
//                 break;
//             default:
//                 break;
//         }
//     }
//     //サーバの接続が切れたときのメッセージを、ChatTextに表示する
//     public void RecvClose()
//     {
//         Debug.Log("Disconnected");
//     }

//     void Start()
//     {
//         udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
//         //接続処理。接続先サーバと、ポート番号を指定する
//         // ws = new WebSocket("ws://" + ip + ":" + port);
//         // ws.Connect();

//         //送信ボタンが押されたときに実行する処理「SendText」を登録する
//         // sendButton.onClick.AddListener(SendText);
//         //サーバからメッセージを受信したときに実行する処理「RecvText」を登録する
//         // ws.OnMessage += (sender, e) => RecvText(e.Data);
//         //サーバとの接続が切れたときに実行する処理「RecvClose」を登録する
//         // ws.OnClose += (sender, e) => RecvClose();
//         objdata = new Objdat
//         {

//             position = new ThreeData(),
//             rotation = new ThreeData(),
//         };
//         ctrl = new Ctrl
//         {
//             a = false,
//             b = false,
//             x = false,
//             y = false,
//             R1 = 0,
//             R2 = 0,
//             L1 = 0,
//             L2 = 0
//         };
//         recdata =
//         objdata = new Objdat
//         {

//             position = new ThreeData(),
//             rotation = new ThreeData(),
//         };
//         reclist = new Objdat[datalist.Length];

//     }
//     public void Senddat(string message)
//     {
//         byte[] sendData = Encoding.UTF8.GetBytes(message);
//         udpClient.Send(sendData, sendData.Length, ip, port);
//         // ws.Send(JsonConvert.SerializeObject(senddata));
//     }

//     void ReceiveCallback(IAsyncResult ar)
//     {
//         // クライアントからのデータを受信
//         IPEndPoint clientEndpoint = new IPEndPoint(IPAddress.Any, 0);
//         byte[] receivedData = udpClient.EndReceive(ar, ref clientEndpoint);
//         string receivedMessage = Encoding.UTF8.GetString(receivedData);

//         // 受信データを処理 (例: JSONからオブジェクトデータをデシリアライズ)
//         Debug.Log("Received data: " + receivedMessage);

//         // 必要なら、ここでクライアントに返信を行う
//         // string responseMessage = "Data received";
//         // byte[] responseData = Encoding.UTF8.GetBytes(responseMessage);
//         // udpClient.Send(responseData, responseData.Length, clientEndpoint);

//         // 次のデータ受信を開始
//         udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
//     }
// }


