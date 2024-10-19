//using System.Collections.Generic;
//using Meta.WitAi.Json;
//using UnityEngine;
//using WebSocketSharp;
//using WebSocketSharp.Server;

//public class Seqs_COPY
//{
//    public int[] seqs;
//}
//public class Ctrl_COPY
//{
//    public bool a;
//    public bool b;
//    public bool x;
//    public bool y;
//    public float R1;
//    public float R2;
//    public float L1;
//    public float L2;
//}

//public class ServerManager_COPY : MonoBehaviour
//{
//    WebSocketServer ws;
//    public GameObject[] data;
//    public GameObject[] onPlayerspawn;
//    public int[] relativecordx = new int[] { 0, 0, 1, 1 };
//    public int[] relativecordy = new int[] { 0, 1, 1, 0 };
//    public int port = 12345;
//    public Objdata objdata;
//    static int seqtions;
//    public Transform trans;
//    static public Ctrl ctrl;
//    void Start()
//    {
//        //ポート番号を指定
//        ws = new WebSocketServer(port);
//        //クライアントからの通信時の挙動を定義したクラス、「ExWebSocketBehavior」を登録
//        ws.AddWebSocketService<ExWebSocketBehavior>("/");
//        //サーバ起動
//        ws.Start();
//        Debug.Log("サーバ起動");
//    }
//    public void Update()
//    {
//        // if(seqtions < ExWebSocketBehavior.globalSeq){
//        //     seqtions =  ExWebSocketBehavior.globalSeq;
//        //     foreach(GameObject obj in onPlayerspawn){
//        //         trans.position.x =
//        //         Instantiate(obj);
//        //     }
//        // }
//        objdata ??= new Objdata();
//        for (int i = 0; i < data.Length; i++)
//        {
//            objdata.name = data[i].name;
//            objdata.position.x = data[i].transform.position.x;
//            objdata.position.y = data[i].transform.position.y;
//            objdata.position.z = data[i].transform.position.z;
//            objdata.rotation.x = data[i].transform.rotation.x;
//            objdata.rotation.y = data[i].transform.rotation.y;
//            objdata.rotation.z = data[i].transform.rotation.z;
//            objdata.scale.x = data[i].transform.localScale.x;
//            objdata.scale.y = data[i].transform.localScale.y;
//            objdata.scale.z = data[i].transform.localScale.z;

//            ExWebSocketBehavior.ExSend(JsonConvert.SerializeObject(objdata), ExWebSocketBehavior.clientList);
//        }
//    }
//    private void OnApplicationQuit()
//    {
//        Debug.Log("サーバ停止");
//        ws.Stop();
//    }
//    public class ExWebSocketBehavior : WebSocketBehavior
//    {
//        //誰が現在接続しているのか管理するリスト。
//        public static List<ExWebSocketBehavior> clientList = new List<ExWebSocketBehavior>();
//        //接続者に番号を振るための変数。
//        public static int globalSeq = 0;
//        //自身の番号
//        int seq;
//        public Seqs seqs = new Seqs();
//        private int i;

//        public static Recdata recdata;
//        //誰かがログインしてきたときに呼ばれるメソッド
//        protected override void OnOpen()
//        {
//            //ログインしてきた人には、番号をつけて、リストに登録。
//            globalSeq++;
//            this.seq = globalSeq;
//            clientList.Add(this);

//            Debug.Log("Seq" + this.seq + " Login. (" + this.ID + ")");

//            //接続者全員にメッセージを送る
//            this.Send("{\"seq\":" + seq + ",\"type\":\"login\",\"data\":{\"data\":\"Login\"}}");
//            i = 0;
//            seqs.seqs = new int[clientList.Count];
//            foreach (var client in clientList)
//            {
//                Debug.Log("a" + seqs.seqs[i]);
//                seqs.seqs[i] = client.seq;
//                Debug.Log(seqs.seqs[i]);
//                i++;
//            }
//            i = 0;
//            Debug.Log("test");
//            Debug.Log("{type:\"otherlogin\",\"data\":{\"seq\":" + seq + ",\"data\":" + JsonConvert.SerializeObject<Seqs>(seqs) + "}}");
//            foreach (var client in clientList)
//            {
//                client.Send("{\"seq\":" + seq + ",type:\"otherlogin\",\"data\":{\"data\":" + JsonConvert.SerializeObject<Seqs>(seqs) + "}}");
//            }
//        }
//        //誰かがメッセージを送信してきたときに呼ばれるメソッド
//        protected override void OnMessage(MessageEventArgs e)
//        {
//            // Debug.Log("Seq:" + seq + "..." + e.Data);
//            // //接続者全員にメッセージを送る
//            // foreach (var client in clientList)
//            // {
//            //     client.Send("{\"seq\":" + seq + ",\"type\":\"data\",\"data\":{\"data\":" + e.Data + "}}");
//            // }
//            // Debug.Log(e.Data);
//            var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(e.Data);
//            string type = jsonObject["type"].ToString();
//            int seq = int.Parse(jsonObject["seq"].ToString());
//            // Debug.Log(type);
//            if (type != "data") { Debug.Log(e.Data); }
//            switch (type)
//            {
//                case "data":
//                    recdata = JsonConvert.DeserializeObject<Recdata>(jsonObject["data"].ToString());
//                    // Debug.Log(recdata.data.position.x);
//                    break;
//                case "action":
//                    ctrl = JsonConvert.DeserializeObject<Ctrl>(jsonObject["data"].ToString());
//                    break;
//                default:
//                    break;
//            }
//        }

//        //誰かがログアウトしてきたときに呼ばれるメソッド
//        protected override void OnClose(CloseEventArgs e)
//        {
//            Debug.Log("Seq" + this.seq + " Logout. (" + this.ID + ")");

//            //ログアウトした人を、リストから削除。
//            clientList.Remove(this);
//            i = 0;
//            seqs.seqs = new int[clientList.Count];
//            foreach (var client in clientList)
//            {
//                Debug.Log("a" + seqs.seqs[i]);
//                seqs.seqs[i] = client.seq;
//                Debug.Log(seqs.seqs[i]);
//                i++;
//            }
//            i = 0;
//            Debug.Log("test");
//            Debug.Log("{type:\"otherlogin\",\"data\":{\"seq\":" + seq + ",\"data\":" + JsonConvert.SerializeObject<Seqs>(seqs) + "}}");
//            foreach (var client in clientList)
//            {
//                client.Send("{\"seq\":" + seq + ",type:\"otherlogin\",\"data\":{\"data\":" + JsonConvert.SerializeObject<Seqs>(seqs) + "}}");
//            }
//        }

//        static public void ExSend(string text, List<ExWebSocketBehavior> clientlists)
//        {
//            foreach (var clients in clientlists)
//            {
//                clients.Send(text);
//            }
//        }
//    }
//}
