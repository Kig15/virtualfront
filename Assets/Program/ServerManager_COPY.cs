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
//        //�|�[�g�ԍ����w��
//        ws = new WebSocketServer(port);
//        //�N���C�A���g����̒ʐM���̋������`�����N���X�A�uExWebSocketBehavior�v��o�^
//        ws.AddWebSocketService<ExWebSocketBehavior>("/");
//        //�T�[�o�N��
//        ws.Start();
//        Debug.Log("�T�[�o�N��");
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
//        Debug.Log("�T�[�o��~");
//        ws.Stop();
//    }
//    public class ExWebSocketBehavior : WebSocketBehavior
//    {
//        //�N�����ݐڑ����Ă���̂��Ǘ����郊�X�g�B
//        public static List<ExWebSocketBehavior> clientList = new List<ExWebSocketBehavior>();
//        //�ڑ��҂ɔԍ���U�邽�߂̕ϐ��B
//        public static int globalSeq = 0;
//        //���g�̔ԍ�
//        int seq;
//        public Seqs seqs = new Seqs();
//        private int i;

//        public static Recdata recdata;
//        //�N�������O�C�����Ă����Ƃ��ɌĂ΂�郁�\�b�h
//        protected override void OnOpen()
//        {
//            //���O�C�����Ă����l�ɂ́A�ԍ������āA���X�g�ɓo�^�B
//            globalSeq++;
//            this.seq = globalSeq;
//            clientList.Add(this);

//            Debug.Log("Seq" + this.seq + " Login. (" + this.ID + ")");

//            //�ڑ��ґS���Ƀ��b�Z�[�W�𑗂�
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
//        //�N�������b�Z�[�W�𑗐M���Ă����Ƃ��ɌĂ΂�郁�\�b�h
//        protected override void OnMessage(MessageEventArgs e)
//        {
//            // Debug.Log("Seq:" + seq + "..." + e.Data);
//            // //�ڑ��ґS���Ƀ��b�Z�[�W�𑗂�
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

//        //�N�������O�A�E�g���Ă����Ƃ��ɌĂ΂�郁�\�b�h
//        protected override void OnClose(CloseEventArgs e)
//        {
//            Debug.Log("Seq" + this.seq + " Logout. (" + this.ID + ")");

//            //���O�A�E�g�����l���A���X�g����폜�B
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
