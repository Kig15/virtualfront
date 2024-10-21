
using System;
using System.Collections.Generic;
using Meta.WitAi.Json;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using WebSocketSharp.Server;

public class Seqs
{
    public int[] seqs;
}
public class Objmgr<T>
{
    public List<T> objlist;
    public List<int> id_list;
    public int globalcount;
    public void Add(T obj)
    {
        Debug.Log("add data");
        objlist.Add(obj);
        globalcount++;
        id_list.Add(globalcount);
    }
    public void Remove(T obj)
    {
        objlist.Remove(obj);
        id_list.RemoveAt(objlist.IndexOf(obj));
    }
}
public class ServerManager : MonoBehaviour
{
    WebSocketServer ws;
    public int port = 12345;

    // public GameObject[] datalist;
    public Senddata senddata;
    public Objdat objdata;
    public GameObject[] leftctrl;
    public GameObject[] rightctrl;
    private int Sendcount;
    public GameObject glock;
    public GameObject sield;
    public GameObject bullet;
    public GameObject obstacles;
    public GameObject syringe;
    static public Objmgr<GameObject> glocks;
    static public Objmgr<GameObject> sields;
    static public Objmgr<GameObject> bullets;
    static public Objmgr<GameObject> obstacless;
    static public Objmgr<GameObject> syringes;
    static public Objmgr<GameObject> lglocks;
    static public Objmgr<GameObject> lsields;
    static public Objmgr<GameObject> lbullets;
    static public Objmgr<GameObject> lobstacless;
    static public Objmgr<GameObject> lsyringes;


    void Start()
    {
        //ポート番号を指定
        ws = new WebSocketServer(port);
        //クライアントからの通信時の挙動を定義したクラス、「ExWebSocketBehavior」を登録
        ws.AddWebSocketService<ExWebSocketBehavior>("/");
        //サーバ起動
        ws.Start();
        Debug.Log("サーバ起動");

        objdata = new Objdat
        {
            position = new ThreeData
            {
                x = 0,
                y = 0,
                z = 0
            },
            rotation = new FourData
            {
                x = 0,
                y = 0,
                z = 0,
                w = 0
            },
        };

        ExWebSocketBehavior.ctrl = new Ctrl[1]{new Ctrl{
            a = false,
            b = false,
            x = false,
            y = false,
            R1 = 0,
            R2 = 0,
            L1 = 0,
            L2 = 0
        }};
        senddata = new Senddata
        {
            type = "data",
            data = JsonConvert.SerializeObject(ExWebSocketBehavior.ctrl)
        };
        ExWebSocketBehavior.right = new Objdat[4]{
            new Objdat{
                position = new ThreeData{
                    x = 0,
                    y = 0,
                    z = 0
                },
                rotation = new FourData
                {
                    x = 0,
                    y = 0,
                    z = 0,
                    w = 0
                },
                id_list = new int[0] 
            },
            new Objdat{
                position = new ThreeData{
                    x = 0,
                    y = 0,
                    z = 0
                },
                rotation = new FourData
                {
                    x = 0,
                    y = 0,
                    z = 0,
                    w = 0
                },
                id_list = new int[0]
            },
            new Objdat{
                position = new ThreeData{
                    x = 0,
                    y = 0,
                    z = 0
                },
                rotation = new FourData
                {
                    x = 0,
                    y = 0,
                    z = 0,
                    w = 0
                },
                id_list = new int[0]
            },
            new Objdat{
                position = new ThreeData{
                    x = 0,
                    y = 0,
                    z = 0
                },
                rotation = new FourData
                {
                    x = 0,
                    y = 0,
                    z = 0,
                    w = 0
                },
                id_list = new int[0]
            },

        };
        ExWebSocketBehavior.left = new Objdat[4]{
            new Objdat{
                position = new ThreeData{
                    x = 0,
                    y = 0,
                    z = 0
                },
                rotation = new FourData
                {
                    x = 0,
                    y = 0,
                    z = 0,
                    w = 0
                },
                id_list = new int[0]
            },
            new Objdat{
                position = new ThreeData{
                    x = 0,
                    y = 0,
                    z = 0
                },
                rotation = new FourData
                {
                    x = 0,
                    y = 0,
                    z = 0,
                    w = 0
                },
                id_list = new int[0]
            },
            new Objdat{
                position = new ThreeData{
                    x = 0,
                    y = 0,
                    z = 0
                },
                rotation = new FourData
                {
                    x = 0,
                    y = 0,
                    z = 0,
                    w = 0
                },
                id_list = new int[0]
            },
            new Objdat{
                position = new ThreeData{
                    x = 0,
                    y = 0,
                    z = 0
                },
                rotation = new FourData
                {
                    x = 0,
                    y = 0,
                    z = 0,
                    w = 0
                },
                id_list = new int[0]
            },

        };

        glocks = new Objmgr<GameObject>
        {
            objlist = new List<GameObject>(),
            id_list = new List<int>()
        };
        sields = new Objmgr<GameObject>
        {
            objlist = new List<GameObject>(),
            id_list = new List<int>()
        };
        bullets = new Objmgr<GameObject>
        {
            objlist = new List<GameObject>(),
            id_list = new List<int>()
        };
        obstacless = new Objmgr<GameObject>
        {
            objlist = new List<GameObject>(),
            id_list = new List<int>()
        };
        syringes = new Objmgr<GameObject>
        {
            objlist = new List<GameObject>(),
            id_list = new List<int>()
        };
    }
    public void Update()
    {

        for (int i = 0; i < rightctrl.Length; i++)
        {
            //Debug.Log(JsonConvert.SerializeObject(ExWebSocketBehavior.right[i]));
            rightctrl[i].transform.SetPositionAndRotation(new Vector3(ExWebSocketBehavior.right[i].position.x, ExWebSocketBehavior.right[i].position.y, ExWebSocketBehavior.right[i].position.z), new Quaternion(ExWebSocketBehavior.right[i].rotation.x, ExWebSocketBehavior.right[i].rotation.y, ExWebSocketBehavior.right[i].rotation.z, ExWebSocketBehavior.right[i].rotation.w));
            leftctrl[i].transform.SetPositionAndRotation(new Vector3(ExWebSocketBehavior.left[i].position.x, ExWebSocketBehavior.left[i].position.y, ExWebSocketBehavior.left[i].position.z), new Quaternion(ExWebSocketBehavior.left[i].rotation.x, ExWebSocketBehavior.left[i].rotation.y, ExWebSocketBehavior.left[i].rotation.z, ExWebSocketBehavior.left[i].rotation.w));
        }
        Sendcount++;
        if (Sendcount > 1000)
        {
            Sendcount = 0;
        }
        objdata ??= new Objdat
        {
            position = new ThreeData
            {
                x = 0,
                y = 0,
                z = 0
            },
            rotation = new FourData
            {
                x = 0,
                y = 0,
                z = 0,
                w = 0
            },
        };
        if (Sendcount % 3 == 0)
        {
            objdata.id_list = glocks.id_list.ToArray();
            Debug.Log(glocks.globalcount);
            for (int i = 0; i < objdata.id_list.Length; i++)
            {
                 Debug.Log(i);
                objdata.id = objdata.id_list[i];
                if(lglocks != glocks)
                {
                    objdata.position.x = glocks.objlist[i].transform.position.x;
                    objdata.position.y = glocks.objlist[i].transform.position.y;
                    objdata.position.z = glocks.objlist[i].transform.position.z;
                    objdata.rotation.x = glocks.objlist[i].transform.rotation.x;
                    objdata.rotation.y = glocks.objlist[i].transform.rotation.y;
                    objdata.rotation.z = glocks.objlist[i].transform.rotation.z;
                    senddata ??= new Senddata
                    {
                        type = "data",
                        data = JsonConvert.SerializeObject(ExWebSocketBehavior.ctrl)
                    };
                    senddata.type = "d_glock";
                    senddata.data = JsonConvert.SerializeObject(objdata);
                    ExWebSocketBehavior.ExSend(JsonConvert.SerializeObject(senddata), ExWebSocketBehavior.clientList);
                    Debug.Log("send data");
                    //lglocks = glocks;
                }
            }
            objdata.id_list = sields.id_list.ToArray();
            for (int i = 0; i < objdata.id_list.Length; i++)
            {
                // Debug.Log(i);
                objdata.id = objdata.id_list[i];
                // Debug.Log(datalist[i].transform.position.x);
                if (!(objdata.position.x == sields.objlist[i].transform.position.x && objdata.position.y == sields.objlist[i].transform.position.y && objdata.position.z == sields.objlist[i].transform.position.z && objdata.rotation.x == sields.objlist[i].transform.rotation.x && objdata.rotation.y == sields.objlist[i].transform.rotation.y && objdata.rotation.z == sields.objlist[i].transform.rotation.z))
                {
                    // isequal = true;
                    objdata.position.x = sields.objlist[i].transform.position.x;
                    objdata.position.y = sields.objlist[i].transform.position.y;
                    objdata.position.z = sields.objlist[i].transform.position.z;
                    objdata.rotation.x = sields.objlist[i].transform.rotation.x;
                    objdata.rotation.y = sields.objlist[i].transform.rotation.y;
                    objdata.rotation.z = sields.objlist[i].transform.rotation.z;
                    senddata ??= new Senddata
                    {
                        type = "data",
                        data = JsonConvert.SerializeObject(ExWebSocketBehavior.ctrl)
                    };
                    senddata.type = "d_sield";
                    senddata.data = JsonConvert.SerializeObject(objdata);
                    ExWebSocketBehavior.ExSend(JsonConvert.SerializeObject(senddata), ExWebSocketBehavior.clientList);
                }
            }
            objdata.id_list = bullets.id_list.ToArray();
            for (int i = 0; i < objdata.id_list.Length; i++)
            {
                // Debug.Log(i);
                objdata.id = objdata.id_list[i];
                // Debug.Log(datalist[i].transform.position.x);
                if (!(objdata.position.x == bullets.objlist[i].transform.position.x && objdata.position.y == bullets.objlist[i].transform.position.y && objdata.position.z == bullets.objlist[i].transform.position.z && objdata.rotation.x == bullets.objlist[i].transform.rotation.x && objdata.rotation.y == bullets.objlist[i].transform.rotation.y && objdata.rotation.z == bullets.objlist[i].transform.rotation.z))
                {
                    // isequal = true;
                    objdata.position.x = bullets.objlist[i].transform.position.x;
                    objdata.position.y = bullets.objlist[i].transform.position.y;
                    objdata.position.z = bullets.objlist[i].transform.position.z;
                    objdata.rotation.x = bullets.objlist[i].transform.rotation.x;
                    objdata.rotation.y = bullets.objlist[i].transform.rotation.y;
                    objdata.rotation.z = bullets.objlist[i].transform.rotation.z;
                    senddata ??= new Senddata
                    {
                        type = "data",
                        data = JsonConvert.SerializeObject(ExWebSocketBehavior.ctrl)
                    };
                    senddata.type = "d_bullet";
                    senddata.data = JsonConvert.SerializeObject(objdata);
                    ExWebSocketBehavior.ExSend(JsonConvert.SerializeObject(senddata), ExWebSocketBehavior.clientList);
                }
            }
            objdata.id_list = obstacless.id_list.ToArray();
            for (int i = 0; i < objdata.id_list.Length; i++)
            {
                // Debug.Log(i);
                objdata.id = objdata.id_list[i];
                // Debug.Log(datalist[i].transform.position.x);
                if (!(objdata.position.x == obstacless.objlist[i].transform.position.x && objdata.position.y == obstacless.objlist[i].transform.position.y && objdata.position.z == obstacless.objlist[i].transform.position.z && objdata.rotation.x == obstacless.objlist[i].transform.rotation.x && objdata.rotation.y == obstacless.objlist[i].transform.rotation.y && objdata.rotation.z == obstacless.objlist[i].transform.rotation.z))
                {
                    // isequal = true;
                    objdata.position.x = obstacless.objlist[i].transform.position.x;
                    objdata.position.y = obstacless.objlist[i].transform.position.y;
                    objdata.position.z = obstacless.objlist[i].transform.position.z;
                    objdata.rotation.x = obstacless.objlist[i].transform.rotation.x;
                    objdata.rotation.y = obstacless.objlist[i].transform.rotation.y;
                    objdata.rotation.z = obstacless.objlist[i].transform.rotation.z;
                    senddata ??= new Senddata
                    {
                        type = "data",
                        data = JsonConvert.SerializeObject(ExWebSocketBehavior.ctrl)
                    };
                    senddata.type = "d_obstacles";
                    senddata.data = JsonConvert.SerializeObject(objdata);
                    ExWebSocketBehavior.ExSend(JsonConvert.SerializeObject(senddata), ExWebSocketBehavior.clientList);
                }
            }
            objdata.id_list = syringes.id_list.ToArray();
            for (int i = 0; i < objdata.id_list.Length; i++)
            {
                // Debug.Log(i);
                objdata.id = objdata.id_list[i];
                // Debug.Log(datalist[i].transform.position.x);
                if (!(objdata.position.x == syringes.objlist[i].transform.position.x && objdata.position.y == syringes.objlist[i].transform.position.y && objdata.position.z == syringes.objlist[i].transform.position.z && objdata.rotation.x == syringes.objlist[i].transform.rotation.x && objdata.rotation.y == syringes.objlist[i].transform.rotation.y && objdata.rotation.z == syringes.objlist[i].transform.rotation.z))
                {
                    // isequal = true;
                    objdata.position.x = syringes.objlist[i].transform.position.x;
                    objdata.position.y = syringes.objlist[i].transform.position.y;
                    objdata.position.z = syringes.objlist[i].transform.position.z;
                    objdata.rotation.x = syringes.objlist[i].transform.rotation.x;
                    objdata.rotation.y = syringes.objlist[i].transform.rotation.y;
                    objdata.rotation.z = syringes.objlist[i].transform.rotation.z;
                    senddata ??= new Senddata
                    {
                        type = "data",
                        data = JsonConvert.SerializeObject(ExWebSocketBehavior.ctrl)
                    };
                    senddata.type = "d_syringe";
                    senddata.data = JsonConvert.SerializeObject(objdata);
                    ExWebSocketBehavior.ExSend(JsonConvert.SerializeObject(senddata), ExWebSocketBehavior.clientList);
                }
            }

        }

        // obj = GameObject.Find(ExWebSocketBehavior.recdata.data.name);
        // obj.transform.position = new Vector3(ExWebSocketBehavior.recdata.data.position.x, ExWebSocketBehavior.recdata.data.position.y, ExWebSocketBehavior.recdata.data.position.z);
        // obj.transform.rotation = new Quaternion(ExWebSocketBehavior.recdata.data.rotation.x, ExWebSocketBehavior.recdata.data.rotation.y, ExWebSocketBehavior.recdata.data.rotation.z, 0);
        // obj.transform.localScale = new Vector3(ExWebSocketBehavior.recdata.data.scale.x, ExWebSocketBehavior.recdata.data.scale.y, ExWebSocketBehavior.recdata.data.scale.z);
        // Debug.Log();
        // rightctrl.transform.localScale = new Vector3(ExWebSocketBehavior.right[i].scale.x, ExWebSocketBehavior.right[i].scale.y, ExWebSocketBehavior.right[i].scale.z);
        // rightctrl.transform.localScale = new Vector3(recdata.scale.x, recdata.scale.y, recdata.scale.z);
    }
    private void OnApplicationQuit()
    {
        Debug.Log("サーバ停止");
        ws.Stop();
    }
    public class ExWebSocketBehavior : WebSocketBehavior
    {
        //誰が現在接続しているのか管理するリスト。
        public static List<ExWebSocketBehavior> clientList = new List<ExWebSocketBehavior>();
        //接続者に番号を振るための変数。
        public static int globalSeq = 0;
        //自身の番号
        int seq;
        public Seqs seqs = new Seqs();
        private int i;
        static public Ctrl[] ctrl;
        public static Objdat[] right;
        public static Objdat[] left;
        // static public GameObject[] rightctrollers = new GameObject[4];
        // static public GameObject[] leftctrollers = new GameObject[4];

        //誰かがログインしてきたときに呼ばれるメソッド
        protected override void OnOpen()
        {
            //ログインしてきた人には、番号をつけて、リストに登録。
            globalSeq++;
            this.seq = globalSeq;
            clientList.Add(this);

            Debug.Log("Seq" + this.seq + " Login. (" + this.ID + ")");

            //接続者全員にメッセージを送る
            this.Send("{\"seq\":" + seq + ",\"type\":\"login\",\"data\":{\"data\":\"Login\"}}");
            i = 0;
            seqs.seqs = new int[clientList.Count];
            foreach (var client in clientList)
            {
                Debug.Log("a" + seqs.seqs[i]);
                seqs.seqs[i] = client.seq;
                Debug.Log(seqs.seqs[i]);
                i++;
            }
            i = 0;
            // Debug.Log("test");
            Debug.Log("{type:\"otherlogin\",\"data\":{\"seq\":" + seq + ",\"data\":" + JsonConvert.SerializeObject<Seqs>(seqs) + "}}");
            foreach (var client in clientList)
            {
                client.Send("{\"seq\":" + seq + ",type:\"otherlogin\",\"data\":{\"data\":" + JsonConvert.SerializeObject<Seqs>(seqs) + "}}");
            }
        }
        //誰かがメッセージを送信してきたときに呼ばれるメソッド
        protected override void OnMessage(MessageEventArgs e)
        {
            // Debug.Log("Seq:" + seq + "..." + e.Data);
            // //接続者全員にメッセージを送る
            // foreach (var client in clientList)
            // {
            //     client.Send("{\"seq\":" + seq + ",\"type\":\"data\",\"data\":{\"data\":" + e.Data + "}}");
            // }
            //Debug.Log(e.Data);
            var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(e.Data);
            string type = jsonObject["type"].ToString();
            // int seq = int.Parse(jsonObject["seq"].ToString());
            // Debug.Log(type);
            // if (type != "data") { Debug.Log(e.Data); }
            switch (type)
            {
                case "right":
                    Debug.Log(jsonObject["data"].ToString());
                    right[Array.IndexOf(seqs.seqs, seq)] = JsonConvert.DeserializeObject<Objdat>(jsonObject["data"].ToString());

                    break;
                case "left":
                    left[Array.IndexOf(seqs.seqs, seq)] = JsonConvert.DeserializeObject<Objdat>(jsonObject["data"].ToString());
                    break;
                case "action":
                    ctrl[Array.IndexOf(seqs.seqs, seq)] = JsonConvert.DeserializeObject<Ctrl>(jsonObject["data"].ToString());
                    ctrl ??= new Ctrl[1]{
                        new Ctrl{
                            a = false,
                            b = false,
                            x = false,
                            y = false,
                            R1 = 0,
                            R2 = 0,
                            L1 = 0,
                            L2 = 0
                        }
                    };
                    break;
                default:
                    break;
            }
        }

        //誰かがログアウトしてきたときに呼ばれるメソッド
        protected override void OnClose(CloseEventArgs e)
        {
            Debug.Log("Seq" + this.seq + " Logout. (" + this.ID + ")");

            //ログアウトした人を、リストから削除。
            clientList.Remove(this);
            i = 0;
            seqs.seqs = new int[clientList.Count];
            foreach (var client in clientList)
            {
                Debug.Log("a" + seqs.seqs[i]);
                seqs.seqs[i] = client.seq;
                Debug.Log(seqs.seqs[i]);
                i++;
            }
            i = 0;
            Debug.Log("test");
            Debug.Log("{type:\"otherlogin\",\"data\":{\"seq\":" + seq + ",\"data\":" + JsonConvert.SerializeObject<Seqs>(seqs) + "}}");
            foreach (var client in clientList)
            {
                client.Send("{\"seq\":" + seq + ",type:\"otherlogin\",\"data\":{\"data\":" + JsonConvert.SerializeObject<Seqs>(seqs) + "}}");
            }
        }

        static public void ExSend(string text, List<ExWebSocketBehavior> clientlists)
        {
            foreach (var clients in clientlists)
            {
                clients.Send(text);
            }
        }
        static
    public void Senddat(Senddata senddat)
        {
            // ws.Send(JsonConvert.SerializeObject(senddata));
            Debug.LogError(senddat.data);
            ExSend(JsonConvert.SerializeObject(senddat), clientList);
        }
    }
}