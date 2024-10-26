using System;
using System.Collections.Generic;
using Meta.WitAi.Json;
using Meta.WitAi.Lib;
using TMPro;
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
    public void Add(T obj,int id)
    {
        Debug.Log("add data");
        objlist.Add(obj);
        id_list.Add(id);
    }
    public void Remove(T obj)
    {
        id_list.RemoveAt(objlist.IndexOf(obj));
        objlist.Remove(obj);
        Debug.Log("remove data");
    }
}
public class Ctrlmgr: Ctrl
{
    private Ctrl oldctrl;
    private bool[] isgetdown;
    private bool[] isgetup;
    private bool flag;
    public Ctrlmgr()
    {
        isgetdown = new bool[8];
        isgetup = new bool[8];
        oldctrl = new Ctrl();
    }
    public bool IsgetDown(Button_sm button)
    {
        flag = isgetdown[(int)button];
        isgetdown[(int)button] = false;
        return flag;
    }
    public bool IsGetUp(Button_sm button)
    {
        flag = !isgetup[(int)button];
        isgetup[(int)button] |= false;
        return flag;
    }
    public void updata()
    {
            if(this.a != oldctrl.a) { isgetdown[0] = this.a; }
            if(this.b != oldctrl.b) { isgetdown[1] = this.b; }
            if(this.x != oldctrl.x) { isgetdown[2] = this.x; }
            if(this.y != oldctrl.y) { isgetdown[3] = this.y; }
            if((this.R1 >= 0.5) != (oldctrl.R1 >= 0.5)) { isgetdown[4] = (this.R1 >= 0.5); }
            if((this.R2 >= 0.5) != (oldctrl.R2 >= 0.5)) { isgetdown[5] = (this.R2 >= 0.5); }
            if ((this.L1 >= 0.5) != (oldctrl.L1 >= 0.5)) { isgetdown[6] = (this.L1 >= 0.5); }
            if ((this.L2 >= 0.5) != (oldctrl.L2 >= 0.5)) { isgetdown[6] = (this.L2 >= 0.5); }
            if (this.a != oldctrl.a) { isgetup[0] = !this.a; }
            if (this.b != oldctrl.b) { isgetup[1] = !this.b; }
            if (this.x != oldctrl.x) { isgetup[2] = !this.x; }
            if (this.y != oldctrl.y) { isgetup[3] = !this.y; }
            if ((this.R1 >= 0.5) != (oldctrl.R1 >= 0.5)) { isgetup[4] = !(this.R1 >= 0.5); }
            if ((this.R2 >= 0.5) != (oldctrl.R2 >= 0.5)) { isgetup[5] = !(this.R2 >= 0.5); }
            if ((this.L1 >= 0.5) != (oldctrl.L1 >= 0.5)) { isgetup[6] = !(this.L1 >= 0.5); }
            if ((this.L2 >= 0.5) != (oldctrl.L2 >= 0.5)) { isgetup[7] = !(this.L2 >= 0.5); }
            oldctrl.a = this.a;
            oldctrl.b = this.b;
            oldctrl.x = this.x;
            oldctrl.y = this.y;
            oldctrl.R1 = this.R1;
            oldctrl.R2 = this.R2;
            oldctrl.L1 = this.L1;
            oldctrl.L2 = this.L2;
    }
}
public class ServerManager : MonoBehaviour
{
    WebSocketServer ws;
    public int port = 12345;

    // public GameObject[] datalist;
    public Senddata senddata;
    public Objdat objdata;
    public sieldobj objdata2;
    public GameObject[] leftctrl;
    public GameObject[] rightctrl;
    public GameObject[] HeadHitBox;
    public GameObject[] BodyHitBox;
    public GameObject[] UpperArmRHitBox;
    public GameObject[] LowerArmRHitBox;
    public GameObject[] UpperArmLHitBox;
    public GameObject[] LowerArmLHitBox;
    private int Sendcount;
    public GameObject glock;
    public GameObject sield;
    public GameObject bullet;
    public GameObject obstacles;
    public GameObject obstacles2;
    public GameObject syringe;
    static public Objmgr<GameObject> glocks;
    static public Objmgr<GameObject> sields;
    static public Objmgr<GameObject> bullets;
    static public Objmgr<GameObject> obstacless;
    static public Objmgr<GameObject> obstacless2;
    static public Objmgr<GameObject> syringes;
    static private Objmgr<GameObject> lglocks;
    static private Objmgr<GameObject> lsields;
    static private Objmgr<GameObject> lbullets;
    static private Objmgr<GameObject> lobstacless;
    static private Objmgr<GameObject> lobstacless2;
    static private Objmgr<GameObject> lsyringes;
    static public bool[] isInvincible;
    private string[] lastdata;
    private string[] lastd_glock;
    private string[] lastd_sield;
    private string[] lastd_bullet;
    private string[] lastd_obstacles;
    private string[] lastd_syringe;
    private string[] lastd_obstacles2;
    private string[] lastammos;
    private string[] lastsield;
    private string[] lasthp;
    private bool isnew;
    private int sendnewcount;
    public Status st;
    public TextMeshPro[] text;
    public GameObject[] datalist;
    public Status status;
    public int[] hp;
    private int ic;
    static public string[] ammo = new string[8];
    

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

        ExWebSocketBehavior.ctrl = new Ctrlmgr[4]{
                        new Ctrlmgr{
                            a = false,
                            b = false,
                            x = false,
                            y = false,
                            R1 = 0,
                            R2 = 0,
                            L1 = 0,
                            L2 = 0
                        },
                        new Ctrlmgr{
                            a = false,
                            b = false,
                            x = false,
                            y = false,
                            R1 = 0,
                            R2 = 0,
                            L1 = 0,
                            L2 = 0
                        },
                        new Ctrlmgr{
                            a = false,
                            b = false,
                            x = false,
                            y = false,
                            R1 = 0,
                            R2 = 0,
                            L1 = 0,
                            L2 = 0
                        },
                        new Ctrlmgr{
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
        ExWebSocketBehavior.HeadHitBox = new Objdat[4]{
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
        ExWebSocketBehavior.BodyHitBox = new Objdat[4]{
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
        ExWebSocketBehavior.UpperArmRHitBox = new Objdat[4]{
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
        ExWebSocketBehavior.LowerArmRHitBox = new Objdat[4]{
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
        ExWebSocketBehavior.UpperArmLHitBox = new Objdat[4]{
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
        ExWebSocketBehavior.LowerArmLHitBox = new Objdat[4]{
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
        obstacless2 = new Objmgr<GameObject>
        {
            objlist = new List<GameObject>(),
            id_list = new List<int>()
        };
        syringes = new Objmgr<GameObject>
        {
            objlist = new List<GameObject>(),
            id_list = new List<int>()
        };
        hp = new int[4];
        isnew = false;
        lastdata = new string[datalist.Length];
        lastd_glock = new string[4];
        lastd_sield = new string[20];
        lastd_bullet = new string[20];
        lastd_obstacles = new string[10];
        lastd_obstacles2 = new string[10];
        lastd_syringe = new string[20];
        lastammos = new string[8];
    }
    public void Update()
    {
        if (ExWebSocketBehavior.isnewclient)
        {
            sendnewcount = Sendcount;
            isnew = true;
        }
        if(sendnewcount + 600 < Sendcount)
        {
            isnew = false;
        }
        ExWebSocketBehavior.ctrl[0].updata();
        ExWebSocketBehavior.ctrl[1].updata();
        ExWebSocketBehavior.ctrl[2].updata();
        ExWebSocketBehavior.ctrl[3].updata();
        for (int i = 0; i < rightctrl.Length; i++)
        {
            //Debug.Log(JsonConvert.SerializeObject(ExWebSocketBehavior.right[i]));
            rightctrl[i].transform.SetPositionAndRotation(new Vector3(ExWebSocketBehavior.right[i].position.x, ExWebSocketBehavior.right[i].position.y, ExWebSocketBehavior.right[i].position.z), new Quaternion(ExWebSocketBehavior.right[i].rotation.x, ExWebSocketBehavior.right[i].rotation.y, ExWebSocketBehavior.right[i].rotation.z, ExWebSocketBehavior.right[i].rotation.w));
            leftctrl[i].transform.SetPositionAndRotation(new Vector3(ExWebSocketBehavior.left[i].position.x, ExWebSocketBehavior.left[i].position.y, ExWebSocketBehavior.left[i].position.z), new Quaternion(ExWebSocketBehavior.left[i].rotation.x, ExWebSocketBehavior.left[i].rotation.y, ExWebSocketBehavior.left[i].rotation.z, ExWebSocketBehavior.left[i].rotation.w));
            HeadHitBox[i].transform.SetPositionAndRotation(new Vector3(ExWebSocketBehavior.HeadHitBox[i].position.x, ExWebSocketBehavior.HeadHitBox[i].position.y, ExWebSocketBehavior.HeadHitBox[i].position.z), new Quaternion(ExWebSocketBehavior.HeadHitBox[i].rotation.x, ExWebSocketBehavior.HeadHitBox[i].rotation.y, ExWebSocketBehavior.HeadHitBox[i].rotation.z, ExWebSocketBehavior.HeadHitBox[i].rotation.w));
            BodyHitBox[i].transform.SetPositionAndRotation(new Vector3(ExWebSocketBehavior.BodyHitBox[i].position.x, ExWebSocketBehavior.BodyHitBox[i].position.y, ExWebSocketBehavior.BodyHitBox[i].position.z), new Quaternion(ExWebSocketBehavior.BodyHitBox[i].rotation.x, ExWebSocketBehavior.BodyHitBox[i].rotation.y, ExWebSocketBehavior.BodyHitBox[i].rotation.z, ExWebSocketBehavior.BodyHitBox[i].rotation.w));
            UpperArmRHitBox[i].transform.SetPositionAndRotation(new Vector3(ExWebSocketBehavior.UpperArmRHitBox[i].position.x, ExWebSocketBehavior.UpperArmRHitBox[i].position.y, ExWebSocketBehavior.UpperArmRHitBox[i].position.z), new Quaternion(ExWebSocketBehavior.UpperArmRHitBox[i].rotation.x, ExWebSocketBehavior.UpperArmRHitBox[i].rotation.y, ExWebSocketBehavior.UpperArmRHitBox[i].rotation.z, ExWebSocketBehavior.UpperArmRHitBox[i].rotation.w));
            LowerArmRHitBox[i].transform.SetPositionAndRotation(new Vector3(ExWebSocketBehavior.LowerArmRHitBox[i].position.x, ExWebSocketBehavior.LowerArmRHitBox[i].position.y, ExWebSocketBehavior.LowerArmRHitBox[i].position.z), new Quaternion(ExWebSocketBehavior.LowerArmRHitBox[i].rotation.x, ExWebSocketBehavior.LowerArmRHitBox[i].rotation.y, ExWebSocketBehavior.LowerArmRHitBox[i].rotation.z, ExWebSocketBehavior.LowerArmRHitBox[i].rotation.w));
            UpperArmLHitBox[i].transform.SetPositionAndRotation(new Vector3(ExWebSocketBehavior.UpperArmLHitBox[i].position.x, ExWebSocketBehavior.UpperArmLHitBox[i].position.y, ExWebSocketBehavior.UpperArmLHitBox[i].position.z), new Quaternion(ExWebSocketBehavior.UpperArmLHitBox[i].rotation.x, ExWebSocketBehavior.UpperArmLHitBox[i].rotation.y, ExWebSocketBehavior.UpperArmLHitBox[i].rotation.z, ExWebSocketBehavior.UpperArmLHitBox[i].rotation.w));
            LowerArmLHitBox[i].transform.SetPositionAndRotation(new Vector3(ExWebSocketBehavior.LowerArmLHitBox[i].position.x, ExWebSocketBehavior.LowerArmLHitBox[i].position.y, ExWebSocketBehavior.LowerArmLHitBox[i].position.z), new Quaternion(ExWebSocketBehavior.LowerArmLHitBox[i].rotation.x, ExWebSocketBehavior.LowerArmLHitBox[i].rotation.y, ExWebSocketBehavior.LowerArmLHitBox[i].rotation.z, ExWebSocketBehavior.LowerArmLHitBox[i].rotation.w));
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
        objdata2 ??= new sieldobj
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
            hp = 0,
            isactive = false
        };
        if (true)
        {

            ic = Sendcount % text.Length;
            st ??= new Status{
                id = 0,
                data = ""
            };
            st.id = ic;
            st.data = text[ic].text;
            senddata.type = "hp";
            senddata.data = JsonConvert.SerializeObject(st);
            ExWebSocketBehavior.Senddat(senddata, ic);
            //foreach(var i in text)
            //{
            //    Debug.LogError(i.text);
            //}
            

            ic = Sendcount % datalist.Length;
                // Debug.Log(i);
                objdata.id = ic;
                // Debug.Log(datalist[i].transform.position.x);
                objdata.position.x = datalist[ic].transform.position.x;
                objdata.position.y = datalist[ic].transform.position.y;
                objdata.position.z = datalist[ic].transform.position.z;
                objdata.rotation.x = datalist[ic].transform.rotation.x;
                objdata.rotation.y = datalist[ic].transform.rotation.y;
                objdata.rotation.z = datalist[ic].transform.rotation.z;
                objdata.rotation.w = datalist[ic].transform.rotation.w;
            if ((ic == 2) || (ic == 3))
            {
                //Debug.LogError(JsonConvert.SerializeObject(objdata));
            }

            // ExWebSocketBehavior.ExSend(JsonConvert.SerializeObject(objdata), ExWebSocketBehavior.clientList);

            senddata ??= new Senddata
                {
                    type = "data",
                    data = JsonConvert.SerializeObject(ExWebSocketBehavior.ctrl)
                };
                senddata.type = "data";
                senddata.data = JsonConvert.SerializeObject(objdata);
            //Debug.LogError("flag::"+isnew);
            if((lastdata[ic] != senddata.data) || isnew)
            {
                lastdata[ic] = senddata.data;
                ExWebSocketBehavior.ExSend(JsonConvert.SerializeObject(senddata), ExWebSocketBehavior.clientList);
            }
                //ExWebSocketBehavior.ExSend(JsonConvert.SerializeObject(senddata), ExWebSocketBehavior.clientList);

            ic = Sendcount % ((4 > glocks.id_list.Count)? glocks.id_list.Count : 4);
            objdata.id_list = glocks.id_list.ToArray();
            //Debug.Log(glocks.globalcount);
                 //Debug.Log(i);
                objdata.id = objdata.id_list[ic];
                objdata.position.x = glocks.objlist[ic].transform.position.x;
                objdata.position.y = glocks.objlist[ic].transform.position.y;
                objdata.position.z = glocks.objlist[ic].transform.position.z;
                objdata.rotation.x = glocks.objlist[ic].transform.rotation.x;
                objdata.rotation.y = glocks.objlist[ic].transform.rotation.y;
                objdata.rotation.z = glocks.objlist[ic].transform.rotation.z;
                objdata.rotation.w = glocks.objlist[ic].transform.rotation.w;
                senddata ??= new Senddata
                {
                    type = "data",
                    data = JsonConvert.SerializeObject(ExWebSocketBehavior.ctrl)
                };
                senddata.type = "d_glock";
                senddata.data = JsonConvert.SerializeObject(objdata);
                if ((lastd_glock[ic] != senddata.data) || isnew)
                {
                    lastd_glock[ic] = senddata.data;
                ExWebSocketBehavior.ExSend(JsonConvert.SerializeObject(senddata), ExWebSocketBehavior.clientList);
            }
                    //ExWebSocketBehavior.ExSend(JsonConvert.SerializeObject(senddata), ExWebSocketBehavior.clientList);
                    //Debug.Log("send data");
                    //lglocks = glocks;

                    status ??= new Status();
                    status.id = ic;
                    status.data = ammo[ic];
                    senddata.type = "ammos";
                    senddata.data = JsonConvert.SerializeObject(status);
            if ((lastammos[ic] != senddata.data) || isnew)
                    {
                        lastammos[ic] = senddata.data;
                ExWebSocketBehavior.ExSend(JsonConvert.SerializeObject(senddata), ExWebSocketBehavior.clientList);
            }
                    //ExWebSocketBehavior.ExSend(JsonConvert.SerializeObject(senddata), ExWebSocketBehavior.clientList);
                
                if(bullets.objlist.Count == 0)
            {
                if (objdata.id_list.Length == 0)
                {
                    objdata.id = -1;
                    objdata.position.x = 0;
                    objdata.position.y = 0;
                    objdata.position.z = 0;
                    objdata.rotation.x = 0;
                    objdata.rotation.y = 0;
                    objdata.rotation.z = 0;
                    senddata ??= new Senddata
                    {
                        type = "data",
                        data = JsonConvert.SerializeObject(ExWebSocketBehavior.ctrl)
                    };
                    senddata.type = "d_bullet";
                    senddata.data = JsonConvert.SerializeObject(objdata);
                    //ExWebSocketBehavior.ExSend(JsonConvert.SerializeObject(senddata), ExWebSocketBehavior.clientList);
                }
            }
            else
            {
                ic = Sendcount % ((bullets.objlist.Count > 20) ? 20 : bullets.objlist.Count);
                objdata.id_list = bullets.id_list.ToArray();
                // Debug.Log(i);
                objdata.id = objdata.id_list[ic];
                // Debug.Log(datalist[i].transform.position.x);
                if (bullets.objlist[ic] != null)
                {
                    // isequal = true;
                    //Debug.Log(bullets.objlist.Count);
                    objdata.position.x = bullets.objlist[ic].transform.position.x;
                    objdata.position.y = bullets.objlist[ic].transform.position.y;
                    objdata.position.z = bullets.objlist[ic].transform.position.z;
                    objdata.rotation.x = bullets.objlist[ic].transform.rotation.x;
                    objdata.rotation.y = bullets.objlist[ic].transform.rotation.y;
                    objdata.rotation.z = bullets.objlist[ic].transform.rotation.z;
                    objdata.rotation.w = bullets.objlist[ic].transform.rotation.w;
                    senddata ??= new Senddata
                    {
                        type = "data",
                        data = JsonConvert.SerializeObject(ExWebSocketBehavior.ctrl)
                    };
                    senddata.type = "d_bullet";
                    senddata.data = JsonConvert.SerializeObject(objdata);
                    if((lastd_bullet[ic] != senddata.data) || isnew)
                    {
                        lastd_bullet[ic] = senddata.data;
                        //ExWebSocketBehavior.ExSend(JsonConvert.SerializeObject(senddata), ExWebSocketBehavior.clientList);
                    }
                    //ExWebSocketBehavior.ExSend(JsonConvert.SerializeObject(senddata), ExWebSocketBehavior.clientList);
                }
            }
            
            
            if(sields.id_list.Count != 0)
            {
                ic = Sendcount % ((sields.id_list.Count > 10) ? 10 : sields.id_list.Count);
                objdata2.id_list = sields.id_list.ToArray();
                // Debug.Log(i);
                objdata2.id = objdata2.id_list[ic];
                // Debug.Log(datalist[i].transform.position.x);
                if (true)
                {
                    // isequal = true;
                    
                    objdata2.position.x = sields.objlist[ic].transform.position.x;
                    objdata2.position.y = sields.objlist[ic].transform.position.y;
                    objdata2.position.z = sields.objlist[ic].transform.position.z;
                    objdata2.rotation.x = sields.objlist[ic].transform.rotation.x;
                    objdata2.rotation.y = sields.objlist[ic].transform.rotation.y;
                    objdata2.rotation.z = sields.objlist[ic].transform.rotation.z;
                    objdata2.rotation.w = sields.objlist[ic].transform.rotation.w;
                    //Debug.LogError(sields.objlist[ic].transform.FindChild("Shield").gameObject.);
                    //objdata2.isactive = sields.objlist[ic].transform.FindChild("Shield").gameObject.GetComponent<HandShieldController>().isactive;
                    //objdata2.isactive = sields.objlist[ic].GetComponentInChildren<HandShieldController>().isactive;
                    senddata ??= new Senddata
                    {
                        type = "data",
                        data = JsonConvert.SerializeObject(ExWebSocketBehavior.ctrl)
                    };
                    senddata.type = "d_sield";
                    senddata.data = JsonConvert.SerializeObject(objdata2);
                    ExWebSocketBehavior.ExSend(JsonConvert.SerializeObject(senddata), ExWebSocketBehavior.clientList);
                }
            }

            objdata.id_list = obstacless.id_list.ToArray();
            for (int i = 0; i < objdata.id_list.Length; i++)
            {
                // Debug.Log(i);
                objdata.id = objdata.id_list[i];
                // Debug.Log(datalist[i].transform.position.x);
                if (true)
                {
                    // isequal = true;
                    objdata.position.x = obstacless.objlist[i].transform.position.x;
                    objdata.position.y = obstacless.objlist[i].transform.position.y;
                    objdata.position.z = obstacless.objlist[i].transform.position.z;
                    objdata.rotation.x = obstacless.objlist[i].transform.rotation.x;
                    objdata.rotation.y = obstacless.objlist[i].transform.rotation.y;
                    objdata.rotation.z = obstacless.objlist[i].transform.rotation.z;
                    objdata.rotation.w = obstacless.objlist[i].transform.rotation.w;
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
            objdata.id_list = obstacless2.id_list.ToArray();
            for (int i = 0; i < objdata.id_list.Length; i++)
            {
                // Debug.Log(i);
                objdata.id = objdata.id_list[i];
                // Debug.Log(datalist[i].transform.position.x);
                if (true)
                {
                    // isequal = true;
                    objdata.position.x = obstacless2.objlist[i].transform.position.x;
                    objdata.position.y = obstacless2.objlist[i].transform.position.y;
                    objdata.position.z = obstacless2.objlist[i].transform.position.z;
                    objdata.rotation.x = obstacless2.objlist[i].transform.rotation.x;
                    objdata.rotation.y = obstacless2.objlist[i].transform.rotation.y;
                    objdata.rotation.z = obstacless2.objlist[i].transform.rotation.z;
                    objdata.rotation.w = obstacless2.objlist[i].transform.rotation.w;
                    senddata ??= new Senddata
                    {
                        type = "data",
                        data = JsonConvert.SerializeObject(ExWebSocketBehavior.ctrl)
                    };
                    senddata.type = "d_obstacles2";
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
                if (true)
                {
                    // isequal = true;
                    objdata.position.x = syringes.objlist[i].transform.position.x;
                    objdata.position.y = syringes.objlist[i].transform.position.y;
                    objdata.position.z = syringes.objlist[i].transform.position.z;
                    objdata.rotation.x = syringes.objlist[i].transform.rotation.x;
                    objdata.rotation.y = syringes.objlist[i].transform.rotation.y;
                    objdata.rotation.z = syringes.objlist[i].transform.rotation.z;
                    objdata.rotation.w = syringes.objlist[i].transform.rotation.w;
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
        ExWebSocketBehavior.isnewclient = false;
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
        static public Seqs seqs = new Seqs();
        private int i;
        static public Ctrlmgr[] ctrl;
        public static Objdat[] right;
        public static Objdat[] left;
        public static Objdat[] HeadHitBox;
        public static Objdat[] BodyHitBox;
        public static Objdat[] UpperArmRHitBox;
        public static Objdat[] LowerArmRHitBox;
        public static Objdat[] UpperArmLHitBox;
        public static Objdat[] LowerArmLHitBox;
        public static bool isnewclient;
        public Status status;
        private Ctrl recctrl;
        // static public GameObject[] rightctrollers = new GameObject[4];
        // static public GameObject[] leftctrollers = new GameObject[4];

        //誰かがログインしてきたときに呼ばれるメソッド
        protected override void OnOpen()
        {
            //ログインしてきた人には、番号をつけて、リストに登録。
            isnewclient = true;
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
                case "invis":
                    status = JsonConvert.DeserializeObject<Status>(jsonObject["data"].ToString());
                    isInvincible[Array.IndexOf(seqs.seqs, seq)] = (status.data == "true")?true:false;
                    break;
                case "right":
                    //Debug.Log(jsonObject["data"].ToString());
                    right[Array.IndexOf(seqs.seqs, seq)] = JsonConvert.DeserializeObject<Objdat>(jsonObject["data"].ToString());

                    break;
                case "left":
                    left[Array.IndexOf(seqs.seqs, seq)] = JsonConvert.DeserializeObject<Objdat>(jsonObject["data"].ToString());
                    break;
                case "HeadHitBox":
                    HeadHitBox[Array.IndexOf(seqs.seqs, seq)] = JsonConvert.DeserializeObject<Objdat>(jsonObject["data"].ToString());
                    break;
                case "BodyHitBox":
                    BodyHitBox[Array.IndexOf(seqs.seqs, seq)] = JsonConvert.DeserializeObject<Objdat>(jsonObject["data"].ToString());
                    break;
                case "UpperArmRHitBox":
                    UpperArmRHitBox[Array.IndexOf(seqs.seqs, seq)] = JsonConvert.DeserializeObject<Objdat>(jsonObject["data"].ToString());
                    break;
                case "LowerArmRHitBox":
                    LowerArmRHitBox[Array.IndexOf(seqs.seqs, seq)] = JsonConvert.DeserializeObject<Objdat>(jsonObject["data"].ToString());
                    break;
                case "UpperArmLHitBox":
                    UpperArmLHitBox[Array.IndexOf(seqs.seqs, seq)] = JsonConvert.DeserializeObject<Objdat>(jsonObject["data"].ToString());
                    break;
                case "LowerArmLHitBox":
                    LowerArmLHitBox[Array.IndexOf(seqs.seqs, seq)] = JsonConvert.DeserializeObject<Objdat>(jsonObject["data"].ToString());
                    break;
                case "action":
                    recctrl = JsonConvert.DeserializeObject<Ctrl>(jsonObject["data"].ToString());
                    ctrl[Array.IndexOf(seqs.seqs, seq)].a = recctrl.a;
                    ctrl[Array.IndexOf(seqs.seqs, seq)].b = recctrl.b;
                    ctrl[Array.IndexOf(seqs.seqs, seq)].x = recctrl.x;
                    ctrl[Array.IndexOf(seqs.seqs, seq)].y = recctrl.y;
                    ctrl[Array.IndexOf(seqs.seqs, seq)].R1 = recctrl.R1;
                    ctrl[Array.IndexOf(seqs.seqs, seq)].R2 = recctrl.R2;
                    ctrl[Array.IndexOf(seqs.seqs, seq)].L1 = recctrl.L1;
                    ctrl[Array.IndexOf(seqs.seqs, seq)].L2 = recctrl.L2;

                    ctrl ??= new Ctrlmgr[4]{
                        new Ctrlmgr{
                            a = false,
                            b = false,
                            x = false,
                            y = false,
                            R1 = 0,
                            R2 = 0,
                            L1 = 0,
                            L2 = 0
                        },
                        new Ctrlmgr{
                            a = false,
                            b = false,
                            x = false,
                            y = false,
                            R1 = 0,
                            R2 = 0,
                            L1 = 0,
                            L2 = 0
                        },
                        new Ctrlmgr{
                            a = false,
                            b = false,
                            x = false,
                            y = false,
                            R1 = 0,
                            R2 = 0,
                            L1 = 0,
                            L2 = 0
                        },
                        new Ctrlmgr{
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
            //Debug.LogError(senddat.data);
            ExSend(JsonConvert.SerializeObject(senddat), clientList);
        }
        static public void Senddat(Senddata senddat,int seq)
        {
            //Debug.LogError(seq);
            // ws.Send(JsonConvert.SerializeObject(senddata));
            // ExSend(JsonConvert.SerializeObject(senddat), clientList);
            //Debug.Log(clientList.Count);
           if(!(clientList.Count <= seq))
            {
                clientList[seq].Send(JsonConvert.SerializeObject(senddat));
                //Debug.LogError(JsonConvert.SerializeObject(senddat));
            }
            
        }
    }
}
