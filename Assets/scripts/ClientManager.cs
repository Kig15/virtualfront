using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Collections.Generic;


public class Ctrl
{
    public bool a;
    public bool b;
    public bool x;
    public bool y;
    public float R1;
    public float R2;
    public float L1;
    public float L2;
}
public class ThreeData
{
    public float x;
    public float y;
    public float z;
}
public class FourData : ThreeData
{
    public float w;
}
public class Objdat
{
    public int id;
    public int[] id_list;
    public ThreeData position;
    public FourData rotation;
}
public class Senddata
{
    public string type;
    public string data;
}
// public class Objmgr <T>{
//     public List<T> objlist;
//     public int id;
//     public void Add(T obj){
//         objlist.Add(obj);
//         this.id = objlist.Count - 1;
//     }
// }
public class ClientManager : MonoBehaviour
{
    public WebSocket ws;
    // public Text chatText;
    // public Button sendButton;
    // public InputField messageInput;
    public string ip = "192.168.11.4";
    public int port = 12345;
    public GameObject[] datalist;
    public Senddata senddata;
    public Objdat objdata;
    public Ctrl ctrl;
    public GameObject leftctrl;
    public GameObject rightctrl;
    public Objdat recdata;
    public Objdat[] reclist;
    public GameObject glock;
    public GameObject sield;
    public GameObject bullet;
    public GameObject obstacles;
    public GameObject syringe;
    public Objmgr<GameObject> glocks;
    public Objmgr<GameObject> sields;
    public Objmgr<GameObject> bullets;
    public Objmgr<GameObject> obstacless;
    public Objmgr<GameObject> syringes;
    public Objdat recglock;
    public Objdat recsield;
    public Objdat recbullet;
    public Objdat recobstacles;
    public Objdat recsyringe;
    public Vector3 pos;
    public Quaternion rot;
    public float GunScale = 0.4f;
    private async Task WaitOneSecond()
    {
        await Task.Delay(1000);
    }
    //サーバへ、メッセージを送信する
    public void Update()
    {
        Debug.Log(glock.name);
        // data = SceneManager.GetActiveScene().GetRootGameObjects();
        objdata ??= new Objdat();
        // obj = GameObject.Find("Cube");

        //スペースキーが押されたら
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     data[0].transform.position = new Vector3(data[0].transform.position.x + 1f, 0, 0);
        // }
        senddata ??= new Senddata();
        objdata.id = 0;
        objdata.position.x = rightctrl.transform.position.x;
        objdata.position.y = rightctrl.transform.position.y;
        objdata.position.z = rightctrl.transform.position.z;
        objdata.rotation.x = rightctrl.transform.rotation.x;
        objdata.rotation.y = rightctrl.transform.rotation.y;
        objdata.rotation.z = rightctrl.transform.rotation.z;
        objdata.rotation.w = rightctrl.transform.rotation.w;
        objdata.id_list = new int[1];
        senddata.type = "right";
        senddata.data = JsonConvert.SerializeObject(objdata);
        Senddat();
        objdata.id = 1;
        objdata.position.x = leftctrl.transform.position.x;
        objdata.position.y = leftctrl.transform.position.y;
        objdata.position.z = leftctrl.transform.position.z;
        objdata.rotation.x = leftctrl.transform.rotation.x;
        objdata.rotation.y = leftctrl.transform.rotation.y;
        objdata.rotation.z = leftctrl.transform.rotation.z;
        objdata.rotation.w = leftctrl.transform.rotation.w;
        objdata.id_list = new int[1];
        senddata.type = "left";
        senddata.data = JsonConvert.SerializeObject(objdata);
        Senddat();
        // Debug.Log(recdata.data.name);
        // foreach (OVRInput.OVRControllerBase i in OVRInput.controllers){
        //     if(i != null){continue;}
        //     // i.currentState.RHandTrigger = OVRInput.Get(OVRInput.Button.One) ? 1.0f : 0.0f;
        //     // Debug.Log("button1 :" + (i.currentState.Buttons >> 1 & 1) + " button2 :" + (i.currentState.Buttons >> 2 & 1) + " button3 :" + (i.currentState.Buttons >> 3 & 1) + " button4 :" + (i.currentState.Buttons >> 4 & 1) + " R1 :" + i.currentState.RIndexTrigger + " R2 :" + i.currentState.RHandTrigger + " L1 :" + i.currentState.LIndexTrigger + " L2 :" + i.currentState.LHandTrigger);
        // }

        //send ctrl information
        ctrl.a = OVRInput.Get(OVRInput.Button.One);
        ctrl.b = OVRInput.Get(OVRInput.Button.Two);
        ctrl.x = OVRInput.Get(OVRInput.Button.Three);
        ctrl.y = OVRInput.Get(OVRInput.Button.Four);
        ctrl.R1 = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);//右トリガー
        ctrl.R2 = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);//左トリガー
        ctrl.L1 = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger);//右つかむボタン
        ctrl.L2 = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger);//左掴むボタン
        senddata.type = "action";
        senddata.data = JsonConvert.SerializeObject(ctrl);
        Senddat();
        // Debug.Log(recdata.id);
        // datalist[recdata.id].transform.position = new Vector3(recdata.position.x, recdata.position.y, recdata.position.z);
        // datalist[recdata.id].transform.rotation = new Quaternion(recdata.rotation.x, recdata.rotation.y, recdata.rotation.z, 0);
        // foreach (Objdat i in reclist){
        //     datalist[i.id].transform.SetPositionAndRotation(new Vector3(i.position.x, i.position.y, i.position.z), new Quaternion(i.rotation.x, i.rotation.y, i.rotation.z, 0));
        // }
        pos.y = recglock.position.y;
        pos.x = recglock.position.x;
        pos.z = recglock.position.z;
        rot.x = recglock.rotation.x;
        rot.y = recglock.rotation.y;
        rot.z = recglock.rotation.z;
        if (glocks.globalcount < recglock.id_list.Length)
        {
            
            //Debug.Log((Instantiate(glock, pos, rot)).transform.position.x);
            glocks.Add(Instantiate(glock, pos, rot));
            glocks.objlist[recglock.id - 1].transform.localScale = new Vector3(GunScale, GunScale,GunScale);//マジックナンバーは嫌だ
            //Debug.Log(glocks.objlist[0].name);
            //glocks.id_list.Add(recdata.id);

        }
        else if (glocks.objlist.Count > recglock.id_list.Length)
        {
            Destroy(glocks.objlist[recglock.id - 1]);
        }
        else
        {
            glocks.objlist[recglock.id - 1].transform.SetPositionAndRotation(pos,rot);
        }
    }
    public void RecvText(string text)
    {
        //Debug.Log(text);
        var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(text);
        string type = jsonObject["type"].ToString();
        // int seq = int.Parse(jsonObject["seq"].ToString());
        // Debug.Log(type);
        // if (type != "data") { Debug.Log(text); }
        switch (type)
        {
            // case "login":
            //     clientdata = JsonConvert.DeserializeObject<Clientdata>(jsonObject["data"].ToString());
            //     Debug.Log(clientdata.seq);
            //     break;
            case "data":
                // Debug.Log(seq + " : " + clientdata.seq);
                // Debug.Log(text);
                Debug.Log(jsonObject["data"].ToString());
                recdata = JsonConvert.DeserializeObject<Objdat>(jsonObject["data"].ToString());
                reclist[recdata.id] = recdata;
                break;
            case "d_glock":
                //Debug.Log(jsonObject["data"].ToString());
                recglock = JsonConvert.DeserializeObject<Objdat>(jsonObject["data"].ToString());
                break;
            case "d_sield":
                recdata = JsonConvert.DeserializeObject<Objdat>(jsonObject["data"].ToString());
                if (sields.objlist.Count <= recdata.id_list.Length)
                {
                    sields.objlist[recdata.id] = Instantiate(sield, new Vector3(recdata.position.x, recdata.position.y, recdata.position.z), new Quaternion(recdata.rotation.x, recdata.rotation.y, recdata.rotation.z, 0));
                }
                else if (sields.objlist.Count > recdata.id_list.Length)
                {
                    Destroy(sields.objlist[recdata.id]);
                }
                else
                {
                    sields.objlist[recdata.id].transform.SetPositionAndRotation(new Vector3(recdata.position.x, recdata.position.y, recdata.position.z), new Quaternion(recdata.rotation.x, recdata.rotation.y, recdata.rotation.z, 0));
                }
                break;
            case "d_bullet":
                recdata = JsonConvert.DeserializeObject<Objdat>(jsonObject["data"].ToString());
                if (bullets.objlist.Count <= recdata.id_list.Length)
                {
                    bullets.objlist[recdata.id] = Instantiate(bullet, new Vector3(recdata.position.x, recdata.position.y, recdata.position.z), new Quaternion(recdata.rotation.x, recdata.rotation.y, recdata.rotation.z, 0));
                }
                else if (bullets.objlist.Count > recdata.id_list.Length)
                {
                    Destroy(bullets.objlist[recdata.id]);
                }
                else
                {
                    bullets.objlist[recdata.id].transform.SetPositionAndRotation(new Vector3(recdata.position.x, recdata.position.y, recdata.position.z), new Quaternion(recdata.rotation.x, recdata.rotation.y, recdata.rotation.z, 0));
                }
                break;
            case "d_obstacles":
                recdata = JsonConvert.DeserializeObject<Objdat>(jsonObject["data"].ToString());
                if (obstacless.objlist.Count <= recdata.id_list.Length)
                {
                    obstacless.objlist[recdata.id] = Instantiate(obstacles, new Vector3(recdata.position.x, recdata.position.y, recdata.position.z), new Quaternion(recdata.rotation.x, recdata.rotation.y, recdata.rotation.z, 0));
                }
                else if (obstacless.objlist.Count > recdata.id_list.Length)
                {
                    Destroy(obstacless.objlist[recdata.id]);
                }
                else
                {
                    obstacless.objlist[recdata.id].transform.SetPositionAndRotation(new Vector3(recdata.position.x, recdata.position.y, recdata.position.z), new Quaternion(recdata.rotation.x, recdata.rotation.y, recdata.rotation.z, 0));
                }
                break;
            case "d_syringe":
                recdata = JsonConvert.DeserializeObject<Objdat>(jsonObject["data"].ToString());
                if (syringes.objlist.Count <= recdata.id_list.Length)
                {
                    syringes.objlist[recdata.id] = Instantiate(syringe, new Vector3(recdata.position.x, recdata.position.y, recdata.position.z), new Quaternion(recdata.rotation.x, recdata.rotation.y, recdata.rotation.z, 0));
                }
                else if (syringes.objlist.Count > recdata.id_list.Length)
                {
                    Destroy(syringes.objlist[recdata.id]);
                }
                else
                {
                    syringes.objlist[recdata.id].transform.SetPositionAndRotation(new Vector3(recdata.position.x, recdata.position.y, recdata.position.z), new Quaternion(recdata.rotation.x, recdata.rotation.y, recdata.rotation.z, 0));
                }
                break;
            default:
                break;
        }
    }
    //サーバの接続が切れたときのメッセージを、ChatTextに表示する
    public void RecvClose()
    {
        Debug.Log("Disconnected");
    }

    void Start()
    {
        //接続処理。接続先サーバと、ポート番号を指定する
        ws = new WebSocket("ws://" + ip + ":" + port);
        ws.Connect();

        //送信ボタンが押されたときに実行する処理「SendText」を登録する
        // sendButton.onClick.AddListener(SendText);
        //サーバからメッセージを受信したときに実行する処理「RecvText」を登録する
        ws.OnMessage += (sender, e) => RecvText(e.Data);
        //サーバとの接続が切れたときに実行する処理「RecvClose」を登録する
        ws.OnClose += (sender, e) => RecvClose();
        objdata = new Objdat
        {

            position = new ThreeData(),
            rotation = new FourData(),
        };
        ctrl = new Ctrl
        {
            a = false,
            b = false,
            x = false,
            y = false,
            R1 = 0,
            R2 = 0,
            L1 = 0,
            L2 = 0
        };
        recdata =
        objdata = new Objdat
        {

            position = new ThreeData(),
            rotation = new FourData(),
        };
        reclist = new Objdat[datalist.Length];
        glocks = new Objmgr<GameObject>
        {
            objlist = new List<GameObject>(),
            id_list = new List<int> { 0 }
        };
        Debug.Log(glock.name);

        pos = new Vector3();
        rot = new Quaternion();
    }
    public void Senddat()
    {
        ws.Send(JsonConvert.SerializeObject(senddata));
    }
}