using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using Unity.VisualScripting;
using System;

public enum Button_sm
{
    a = 0,
    b = 1,
    x = 2,
    y = 3,
    r1 = 4,
    r2 = 5,
    l1 = 6,
    l2 = 7,
}
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
public class sieldobj : Objdat
{
    public int hp;
    public bool isactive;
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
public class Status
{
    public int id;
    public string data;
}
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
    public GameObject HeadHitBox;
    public GameObject BodyHitBox;
    public GameObject UpperArmRHitBox;
    public GameObject LowerArmRHitBox;
    public GameObject UpperArmLHitBox;
    public GameObject LowerArmLHitBox;
    public Objdat recdata;
    public Objdat[] reclist;
    public GameObject glock;
    public GameObject sield;
    public GameObject bullet;
    public GameObject obstacles;
    public GameObject obstacles2;
    public GameObject syringe;
    public Objmgr<GameObject> glocks;
    public Objmgr<GameObject> sields;
    public Objmgr<GameObject> bullets;
    public Objmgr<GameObject> obstacless;
    public Objmgr<GameObject> obstacless2;
    public Objmgr<GameObject> syringes;
    public Objdat recglock;
    public sieldobj recsield;
    public Objdat recbullet;
    public Objdat recobstacles;
    public Objdat recobstacles2;
    public Objdat recsyringe;
    public Vector3 pos;
    public Quaternion rot;
    public float GunScale = 1f;
    public string[] ammo;
    public int[] sieldhp;
    public TextMeshPro[] glocktexts;
    public TextMeshPro hp;
    public int Maxhp;
    public int PlayerHealth;
    public bool isInvincible; //これpublicにして他からアクセスされるとチーターが生まれてしまうかも―場合分けしっかり
    private string hps;
    private int duped;
    private Status status;
    private bool once;
    private int aaa;
    public GameObject Masterobj;
    public Objdat absomaster;
    private bool iscordok;
    public int reccount;
    private async Task WaitOneSecond()
    {
        await Task.Delay(1000);
    }
    //サーバへ、メッセージを送信する
    public void Update()
    {
        reccount++;
        absomaster = new Objdat
        {
            position = new ThreeData
            {
                x = Masterobj.transform.position.x,
                y = 0,
                z = Masterobj.transform.position.z,
            },
            rotation = new FourData
            {
                x = 0,
                y = 0,
                z = 0,
                w = 0
            }
        };
        if (reccount % 20 == 0)
        {
            //Debug.LogError(JsonConvert.SerializeObject(glocks.id_list));
            for (int i = 0; i < glocks.objlist.Count; i++)
            {
                glocks.objlist[glocks.id_list.IndexOf((i == 0 ) ? 2:1)].transform.FindChild("Text (TMP)").GetComponent<TextMeshPro>().text = ammo[i];
            }
        }
        //if (!iscordok)
        //{
        //    Debug.LogError(iscordok);
        //    if (OVRInput.GetDown(OVRInput.Button.One)) { iscordok = true; }
        //    absomaster = new Objdat
        //    {
        //        position = new ThreeData
        //        {
        //            x = Masterobj.transform.position.x,
        //            y = 0,
        //            z = Masterobj.transform.position.z,
        //        },
        //        rotation = new FourData
        //        {
        //            x = 0,
        //            y = 0,
        //            z = 0,
        //            w = 0
        //        }
        //    };
        //}
        status ??= new Status();
        hp.text = hps;
        status.id = 0;
        status.data = (isInvincible) ? "true" : "false";
        senddata ??= new Senddata();
        senddata.type = "invincible";
        senddata.data = JsonConvert.SerializeObject(status);
        Senddat();
        //Debug.LogError(JsonConvert.SerializeObject(reclist));
        foreach (Objdat i in reclist)
        {
            if (i != null)
            {
                pos.y = i.position.y + absomaster.position.y;
                pos.x = i.position.x + absomaster.position.x;
                pos.z = i.position.z + absomaster.position.z;
                rot.x = i.rotation.x + absomaster.rotation.x;
                rot.y = i.rotation.y + absomaster.rotation.y;
                rot.z = i.rotation.z + absomaster.rotation.z;
                rot.w = i.rotation.w + absomaster.rotation.w;
                datalist[i.id].transform.SetPositionAndRotation(pos, rot);
                //Debug.LogError("changed:" + i.id + ";" + JsonConvert.SerializeObject(i.position) + ";" + JsonConvert.SerializeObject(i.rotation));
            }
        }
        //aaa = ammo[0];
        //ammo[0] = ammo[1];
        //ammo[1] = aaa;
        //Debug.LogWarning(glocktexts.Length);
        //for (int i = 0;i < glocktexts.Length; i++)
        //{
        //    Debug.LogWarning(glocktexts[i].text);
        //    glocktexts[i].text = "" + ammo[i];
        //}
        //Debug.Log(glock.name);
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
        objdata.position.x = rightctrl.transform.position.x - absomaster.position.x;
        objdata.position.y = rightctrl.transform.position.y - absomaster.position.y;
        objdata.position.z = rightctrl.transform.position.z - absomaster.position.z;
        objdata.rotation.x = rightctrl.transform.rotation.x - absomaster.rotation.x;
        objdata.rotation.y = rightctrl.transform.rotation.y - absomaster.rotation.y;
        objdata.rotation.z = rightctrl.transform.rotation.z - absomaster.rotation.z;
        objdata.rotation.w = rightctrl.transform.rotation.w - absomaster.rotation.w;
        objdata.id_list = new int[1];
        senddata.type = "right";
        senddata.data = JsonConvert.SerializeObject(objdata);
        Senddat();
        objdata.id = 1;
        objdata.position.x = leftctrl.transform.position.x - absomaster.position.x;
        objdata.position.y = leftctrl.transform.position.y - absomaster.position.y;
        objdata.position.z = leftctrl.transform.position.z - absomaster.position.z;
        objdata.rotation.x = leftctrl.transform.rotation.x - absomaster.rotation.x;
        objdata.rotation.y = leftctrl.transform.rotation.y - absomaster.rotation.y;
        objdata.rotation.z = leftctrl.transform.rotation.z - absomaster.rotation.z;
        objdata.rotation.w = leftctrl.transform.rotation.w - absomaster.rotation.w;
        objdata.id_list = new int[1];
        senddata.type = "left";
        senddata.data = JsonConvert.SerializeObject(objdata);
        Senddat();

        objdata.id = 2;
        objdata.position.x = HeadHitBox.transform.position.x - absomaster.position.x;
        objdata.position.y = HeadHitBox.transform.position.y - absomaster.position.y;
        objdata.position.z = HeadHitBox.transform.position.z - absomaster.position.z;
        objdata.rotation.x = HeadHitBox.transform.rotation.x - absomaster.rotation.x;
        objdata.rotation.y = HeadHitBox.transform.rotation.y - absomaster.rotation.y;
        objdata.rotation.z = HeadHitBox.transform.rotation.z - absomaster.rotation.z;
        objdata.rotation.w = HeadHitBox.transform.rotation.w - absomaster.rotation.w;
        objdata.id_list = new int[1];
        senddata.type = "HeadHitBox";
        senddata.data = JsonConvert.SerializeObject(objdata);
        Senddat();

        objdata.id = 3;
        objdata.position.x = BodyHitBox.transform.position.x - absomaster.position.x;
        objdata.position.y = BodyHitBox.transform.position.y - absomaster.position.y;
        objdata.position.z = BodyHitBox.transform.position.z - absomaster.position.z;
        objdata.rotation.x = BodyHitBox.transform.rotation.x - absomaster.rotation.x;
        objdata.rotation.y = BodyHitBox.transform.rotation.y - absomaster.rotation.y;
        objdata.rotation.z = BodyHitBox.transform.rotation.z - absomaster.rotation.z;
        objdata.rotation.w = BodyHitBox.transform.rotation.w - absomaster.rotation.w;
        objdata.id_list = new int[1];
        senddata.type = "BodyHitBox";
        senddata.data = JsonConvert.SerializeObject(objdata);
        Senddat();

        objdata.id = 4;
        objdata.position.x = UpperArmRHitBox.transform.position.x - absomaster.position.x;
        objdata.position.y = UpperArmRHitBox.transform.position.y - absomaster.position.y;
        objdata.position.z = UpperArmRHitBox.transform.position.z - absomaster.position.z;
        objdata.rotation.x = UpperArmRHitBox.transform.rotation.x - absomaster.rotation.x;
        objdata.rotation.y = UpperArmRHitBox.transform.rotation.y - absomaster.rotation.y;
        objdata.rotation.z = UpperArmRHitBox.transform.rotation.z - absomaster.rotation.z;
        objdata.rotation.w = UpperArmRHitBox.transform.rotation.w - absomaster.rotation.w;
        objdata.id_list = new int[1];
        senddata.type = "UpperArmRHitBox";
        senddata.data = JsonConvert.SerializeObject(objdata);
        Senddat();

        objdata.id = 5;
        objdata.position.x = UpperArmLHitBox.transform.position.x - absomaster.position.x;
        objdata.position.y = UpperArmLHitBox.transform.position.y - absomaster.position.y;
        objdata.position.z = UpperArmLHitBox.transform.position.z - absomaster.position.z;
        objdata.rotation.x = UpperArmLHitBox.transform.rotation.x - absomaster.rotation.x;
        objdata.rotation.y = UpperArmLHitBox.transform.rotation.y - absomaster.rotation.y;
        objdata.rotation.z = UpperArmLHitBox.transform.rotation.z - absomaster.rotation.z;
        objdata.rotation.w = UpperArmLHitBox.transform.rotation.w - absomaster.rotation.w;
        objdata.id_list = new int[1];
        senddata.type = "UpperArmLHitBox";
        senddata.data = JsonConvert.SerializeObject(objdata);
        Senddat();

        objdata.id = 6;
        objdata.position.x = LowerArmRHitBox.transform.position.x - absomaster.position.x;
        objdata.position.y = LowerArmRHitBox.transform.position.y - absomaster.position.y;
        objdata.position.z = LowerArmRHitBox.transform.position.z - absomaster.position.z;
        objdata.rotation.x = LowerArmRHitBox.transform.rotation.x - absomaster.rotation.x;
        objdata.rotation.y = LowerArmRHitBox.transform.rotation.y - absomaster.rotation.y;
        objdata.rotation.z = LowerArmRHitBox.transform.rotation.z - absomaster.rotation.z;
        objdata.rotation.w = LowerArmRHitBox.transform.rotation.w - absomaster.rotation.w;
        objdata.id_list = new int[1];
        senddata.type = "LowerArmRHitBox";
        senddata.data = JsonConvert.SerializeObject(objdata);
        Senddat();

        objdata.id = 7;
        objdata.position.x = LowerArmLHitBox.transform.position.x - absomaster.position.x;
        objdata.position.y = LowerArmLHitBox.transform.position.y - absomaster.position.y;
        objdata.position.z = LowerArmLHitBox.transform.position.z - absomaster.position.z;
        objdata.rotation.x = LowerArmLHitBox.transform.rotation.x - absomaster.rotation.x;
        objdata.rotation.y = LowerArmLHitBox.transform.rotation.y - absomaster.rotation.y;
        objdata.rotation.z = LowerArmLHitBox.transform.rotation.z - absomaster.rotation.z;
        objdata.rotation.w = LowerArmLHitBox.transform.rotation.w - absomaster.rotation.w;
        objdata.id_list = new int[1];
        senddata.type = "LowerArmLHitBox";
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
        if (recglock != null)
        {
            foreach (var list in recglock.id_list)
            {
                duped = -1;
                //Debug.LogError(JsonConvert.SerializeObject(glocks.id_list));

                Objmgr<GameObject> glocktmp = new Objmgr<GameObject>
                {
                    objlist = new List<GameObject>(glocks.objlist),
                    id_list = new List<int>(glocks.id_list)
                };
                foreach (var list2 in glocktmp.id_list)
                {
                    if ((list) == (list2))
                    {
                        duped = list2;
                        if ((list) == (recglock.id))
                        {
                            if (glocks.objlist.Count < recglock.id_list.Length)
                            {
                                pos.y = recglock.position.y + absomaster.position.y;
                                pos.x = recglock.position.x + absomaster.position.x;
                                pos.z = recglock.position.z + absomaster.position.z;
                                rot.x = recglock.rotation.x + absomaster.rotation.x;
                                rot.y = recglock.rotation.y + absomaster.rotation.y;
                                rot.z = recglock.rotation.z + absomaster.rotation.z;
                                rot.w = recglock.rotation.w + absomaster.rotation.w;
                                glocks.id_list.Remove(recglock.id - 1);
                                glocks.Add(Instantiate(glock, pos, rot));
                            }
                            else
                            {
                                pos.y = recglock.position.y + absomaster.position.y;
                                pos.x = recglock.position.x + absomaster.position.x;
                                pos.z = recglock.position.z + absomaster.position.z;
                                rot.x = recglock.rotation.x + absomaster.rotation.x;
                                rot.y = recglock.rotation.y + absomaster.rotation.y;
                                rot.z = recglock.rotation.z + absomaster.rotation.z;
                                rot.w = recglock.rotation.w + absomaster.rotation.w;
                                glocks.objlist[glocks.id_list.IndexOf(list)].transform.SetPositionAndRotation(pos, rot);
                            }

                        }
                    }
                }
                if (duped == -1)
                {
                    if (((list) == (recglock.id)) && (glocks.objlist.Count < recglock.id_list.Length))
                    {
                        //Debug.LogError(recglock.id + ";" + list + ";" + JsonConvert.SerializeObject(glocks.id_list));
                        glocks.Add(Instantiate(glock, pos, rot), list);
                        if (!once)
                        {
                            pos.y = recglock.position.y + absomaster.position.y;
                            pos.x = recglock.position.x + absomaster.position.x;
                            pos.z = recglock.position.z + absomaster.position.z;
                            rot.x = recglock.rotation.x + absomaster.rotation.x;
                            rot.y = recglock.rotation.y + absomaster.rotation.y;
                            rot.z = recglock.rotation.z + absomaster.rotation.z;
                            rot.w = recglock.rotation.w + absomaster.rotation.w;
                            glocks.id_list.Remove(0);
                        }
                    }
                }
            }
            //foreach (var list in glocks.id_list)
            //{
            //    duped = -1;
            //    Debug.LogError(JsonConvert.SerializeObject(glocks.id_list));
            //    foreach (var list2 in recglock.id_list)
            //    {
            //        if ((list) == (list2))
            //        {
            //            duped = list2;
            //        }
            //    }
            //    if (duped == -1)
            //    {
            //        glocks.Remove(glocks.objlist[glocks.id_list.IndexOf(list)]);
            //    }
            //}
        }
        if (recsield != null)
        {
            foreach (var list in recsield.id_list)
            {
                duped = -1;
                //Debug.LogError(JsonConvert.SerializeObject(sields.id_list));
                Objmgr<GameObject> sieldtmp2 = new Objmgr<GameObject>
                {
                    objlist = new List<GameObject>(sields.objlist),
                    id_list = new List<int>(sields.id_list)
                };
                foreach (var list2 in sieldtmp2.id_list)
                {
                    if ((list) == (list2))
                    {
                        duped = list2;
                        if ((list) == (recsield.id))
                        {
                            if (sields.objlist.Count < recsield.id_list.Length)
                            {
                                pos.y = recsield.position.y + absomaster.position.y;
                                pos.x = recsield.position.x + absomaster.position.x;
                                pos.z = recsield.position.z + absomaster.position.z;
                                rot.x = recsield.rotation.x + absomaster.rotation.x;
                                rot.y = recsield.rotation.y + absomaster.rotation.y;
                                rot.z = recsield.rotation.z + absomaster.rotation.z;
                                rot.w = recsield.rotation.w + absomaster.rotation.w;
                                sields.id_list.Remove(recsield.id - 1);
                                sields.Add(Instantiate(sield, pos, rot));
                            }
                            else
                            {
                                pos.y = recsield.position.y + absomaster.position.y;
                                pos.x = recsield.position.x + absomaster.position.x;
                                pos.z = recsield.position.z + absomaster.position.z;
                                rot.x = recsield.rotation.x + absomaster.rotation.x;
                                rot.y = recsield.rotation.y + absomaster.rotation.y;
                                rot.z = recsield.rotation.z + absomaster.rotation.z;
                                rot.w = recsield.rotation.w + absomaster.rotation.w;
                                sields.objlist[sields.id_list.IndexOf(list)].transform.SetPositionAndRotation(pos, rot);
                            }

                        }
                    }
                }
                if (duped == -1)
                {
                    if (((list) == (recsield.id)) && (sields.objlist.Count < recsield.id_list.Length))
                    {
                        //Debug.LogError(recsield.id + ";" + list + ";" + JsonConvert.SerializeObject(sields.id_list));
                        sields.Add(Instantiate(sield, pos, rot), list);
                        if (!once)
                        {
                            pos.y = recsield.position.y + absomaster.position.y;
                            pos.x = recsield.position.x + absomaster.position.x;
                            pos.z = recsield.position.z + absomaster.position.z;
                            rot.x = recsield.rotation.x + absomaster.rotation.x;
                            rot.y = recsield.rotation.y + absomaster.rotation.y;
                            rot.z = recsield.rotation.z + absomaster.rotation.z;
                            rot.w = recsield.rotation.w + absomaster.rotation.w;
                            sields.id_list.Remove(0);
                        }
                    }
                }
            }
            Objmgr<GameObject> sieldtmp = new Objmgr<GameObject> { 
                objlist = new List<GameObject>(sields.objlist),
                id_list = new List<int>(sields.id_list)
            };
            foreach (var list in sieldtmp.id_list)
            {
                duped = -1;
                //Debug.LogError(JsonConvert.SerializeObject(recsield.id_list));
                foreach (var list2 in recsield.id_list)
                {
                    if ((list) == (list2))
                    {
                        duped = list2;
                    }
                }
                if (duped == -1)
                {
                    GameObject remove = sields.objlist[sields.id_list.IndexOf(list)];
                    sields.Remove(remove);
                    Destroy(remove);
                    Debug.LogError(remove.name);
                }
            }
            //sields.objlist[sields.id_list.IndexOf(recsield.id)].transform.FindChild("Shield").gameObject.SetActive(recsield.isactive);
        }
        if (false)
        {
            if (recbullet != null)
            {
                if (recbullet.id != -1)
                {
                    foreach (var list in recbullet.id_list)
                    {
                        duped = -1;
                        //Debug.LogError(JsonConvert.SerializeObject(bullets.id_list));
                        foreach (var list2 in bullets.id_list)
                        {
                            if ((list) == (list2))
                            {
                                duped = list2;
                                if ((list) == (recbullet.id))
                                {
                                    if (bullets.objlist.Count < recbullet.id_list.Length)
                                    {
                                        pos.y = recbullet.position.y + absomaster.position.y;
                                        pos.x = recbullet.position.x + absomaster.position.x;
                                        pos.z = recbullet.position.z + absomaster.position.z;
                                        rot.x = recbullet.rotation.x + absomaster.rotation.x;
                                        rot.y = recbullet.rotation.y + absomaster.rotation.y;
                                        rot.z = recbullet.rotation.z + absomaster.rotation.z;
                                        rot.w = recbullet.rotation.w + absomaster.rotation.w;
                                        bullets.id_list.Remove(recbullet.id - 1);
                                        bullets.Add(Instantiate(bullet, pos, rot));
                                    }
                                    else
                                    {
                                        pos.y = recbullet.position.y + absomaster.position.y;
                                        pos.x = recbullet.position.x + absomaster.position.x;
                                        pos.z = recbullet.position.z + absomaster.position.z;
                                        rot.x = recbullet.rotation.x + absomaster.rotation.x;
                                        rot.y = recbullet.rotation.y + absomaster.rotation.y;
                                        rot.z = recbullet.rotation.z + absomaster.rotation.z;
                                        rot.w = recbullet.rotation.w + absomaster.rotation.w;
                                        bullets.objlist[bullets.id_list.IndexOf(list)].transform.SetPositionAndRotation(pos, rot);
                                    }

                                }
                            }
                        }
                        if (duped == -1)
                        {
                            if (((list) == (recbullet.id)) && (bullets.objlist.Count < recbullet.id_list.Length))
                            {
                                //Debug.LogError(recbullet.id + ";" + list + ";" + JsonConvert.SerializeObject(bullets.id_list));
                                if (bullets == null)
                                {
                                    Debug.LogError("null");
                                }
                                bullets.Add(Instantiate(bullet, pos, rot), list);
                                if (!once)
                                {
                                    pos.y = recbullet.position.y + absomaster.position.y;
                                    pos.x = recbullet.position.x + absomaster.position.x;
                                    pos.z = recbullet.position.z + absomaster.position.z;
                                    rot.x = recbullet.rotation.x + absomaster.rotation.x;
                                    rot.y = recbullet.rotation.y + absomaster.rotation.y;
                                    rot.z = recbullet.rotation.z + absomaster.rotation.z;
                                    rot.w = recbullet.rotation.w + absomaster.rotation.w;
                                    bullets.id_list.Remove(0);
                                }
                            }
                        }
                    }
                }
                else
                {
                    //Debug.Log(JsonConvert.SerializeObject(bullets.id_list.ToArray()));
                    for (int i = 0; i < bullets.objlist.Count; i++)
                    {
                        bullets.Remove(bullets.objlist[i]);
                    }
                    foreach (var list in bullets.objlist)
                    {
                        Destroy(list);
                    }
                }
            }
        }
        if (recobstacles != null)
        {
            foreach (var list in recobstacles.id_list)
            {
                duped = -1;
                //Debug.LogError(JsonConvert.SerializeObject(obstacless.id_list));
                Objmgr<GameObject> obstmp1 = new Objmgr<GameObject>
                {
                    objlist = new List<GameObject>(obstacless.objlist),
                    id_list = new List<int>(obstacless.id_list)
                };
                foreach (var list2 in obstmp1.id_list)
                {
                    if ((list) == (list2))
                    {
                        duped = list2;
                        if ((list) == (recobstacles.id))
                        {
                            if (obstmp1.objlist.Count < recobstacles.id_list.Length)
                            {
                                pos.y = recobstacles.position.y + absomaster.position.y;
                                pos.x = recobstacles.position.x + absomaster.position.x;
                                pos.z = recobstacles.position.z + absomaster.position.z;
                                rot.x = recobstacles.rotation.x + absomaster.rotation.x;
                                rot.y = recobstacles.rotation.y + absomaster.rotation.y;
                                rot.z = recobstacles.rotation.z + absomaster.rotation.z;
                                rot.w = recobstacles.rotation.w + absomaster.rotation.w;
                                obstmp1.id_list.Remove(recobstacles.id - 1);
                                obstacless.Add(Instantiate(obstacles, pos, rot));
                            }
                            else
                            {
                                pos.y = recobstacles.position.y + absomaster.position.y;
                                pos.x = recobstacles.position.x + absomaster.position.x;
                                pos.z = recobstacles.position.z + absomaster.position.z;
                                rot.x = recobstacles.rotation.x + absomaster.rotation.x;
                                rot.y = recobstacles.rotation.y + absomaster.rotation.y;
                                rot.z = recobstacles.rotation.z + absomaster.rotation.z;
                                rot.w = recobstacles.rotation.w + absomaster.rotation.w;
                                obstacless.objlist[obstmp1.id_list.IndexOf(list)].transform.SetPositionAndRotation(pos, rot);
                            }

                        }
                    }
                }
                if (duped == -1)
                {
                    if (((list) == (recobstacles.id)) && (obstacless.objlist.Count < recobstacles.id_list.Length))
                    {
                        //Debug.LogError(recobstacles.id + ";" + list + ";" + JsonConvert.SerializeObject(obstacless.id_list));
                        obstacless.Add(Instantiate(obstacles, pos, rot), list);
                        if (!once)
                        {
                            pos.y = recobstacles.position.y + absomaster.position.y;
                            pos.x = recobstacles.position.x + absomaster.position.x;
                            pos.z = recobstacles.position.z + absomaster.position.z;
                            rot.x = recobstacles.rotation.x + absomaster.rotation.x;
                            rot.y = recobstacles.rotation.y + absomaster.rotation.y;
                            rot.z = recobstacles.rotation.z + absomaster.rotation.z;
                            rot.w = recobstacles.rotation.w + absomaster.rotation.w;
                            obstacless.id_list.Remove(0);
                        }
                    }
                }
            }
            Objmgr<GameObject> obstmp = new Objmgr<GameObject> { 
                objlist = new List<GameObject>(obstacless.objlist),
                id_list = new List<int>(obstacless.id_list)
            };
            foreach (var list in obstmp.id_list)
            {
                duped = -1;
                //Debug.LogError(JsonConvert.SerializeObject(obstacless.id_list));
                foreach (var list2 in recobstacles.id_list)
                {
                    if ((list) == (list2))
                    {
                        duped = list2;
                    }
                }
                if (duped == -1)
                {
                    int index = obstacless.id_list.IndexOf(list);
                    Destroy(obstacless.objlist[index]);
                    obstacless.Remove(obstacless.objlist[index]);
                }
            }
        }
        if (recobstacles2 != null)
        {
            foreach (var list in obstacless2.id_list)
            {
                duped = -1;
                //Debug.LogError(JsonConvert.SerializeObject(obstacless2.id_list));
                if(obstacless2 == null)
                {
                    Debug.LogError("null");
                }
                //Debug.LogError(JsonConvert.SerializeObject(obstacless.id_list));
                Objmgr<GameObject> obstmp1 = new Objmgr<GameObject>
                {
                    objlist = new List<GameObject>(obstacless2.objlist),
                    id_list = new List<int>(obstacless2.id_list)
                };
                foreach (var list2 in obstmp1.id_list)
                {
                    if ((list) == (list2))
                    {
                        duped = list2;
                        if ((list) == (recobstacles2.id))
                        {
                            if (obstacless2.objlist.Count < recobstacles2.id_list.Length)
                            {
                                pos.y = recobstacles2.position.y + absomaster.position.y;
                                pos.x = recobstacles2.position.x + absomaster.position.x;
                                pos.z = recobstacles2.position.z + absomaster.position.z;
                                rot.x = recobstacles2.rotation.x + absomaster.rotation.x;
                                rot.y = recobstacles2.rotation.y + absomaster.rotation.y;
                                rot.z = recobstacles2.rotation.z + absomaster.rotation.z;
                                rot.w = recobstacles2.rotation.w + absomaster.rotation.w;
                                obstacless2.id_list.Remove(recobstacles2.id - 1);
                                obstacless2.Add(Instantiate(glock, pos, rot));
                            }
                            else
                            {
                                pos.y = recobstacles2.position.y + absomaster.position.y;
                                pos.x = recobstacles2.position.x + absomaster.position.x;
                                pos.z = recobstacles2.position.z + absomaster.position.z;
                                rot.x = recobstacles2.rotation.x + absomaster.rotation.x;
                                rot.y = recobstacles2.rotation.y + absomaster.rotation.y;
                                rot.z = recobstacles2.rotation.z + absomaster.rotation.z;
                                rot.w = recobstacles2.rotation.w + absomaster.rotation.w;
                                obstacless2.objlist[obstacless2.id_list.IndexOf(list)].transform.SetPositionAndRotation(pos, rot);
                            }

                        }
                    }
                }
                if (duped == -1)
                {
                    if (((list) == (recobstacles2.id)) && (obstacless2.objlist.Count < recobstacles2.id_list.Length))
                    {
                        //Debug.LogError(recobstacles2.id + ";" + list + ";" + JsonConvert.SerializeObject(obstacless2.id_list));
                        obstacless2.Add(Instantiate(glock, pos, rot), list);
                        if (!once)
                        {
                            pos.y = recobstacles2.position.y + absomaster.position.y;
                            pos.x = recobstacles2.position.x + absomaster.position.x;
                            pos.z = recobstacles2.position.z + absomaster.position.z;
                            rot.x = recobstacles2.rotation.x + absomaster.rotation.x;
                            rot.y = recobstacles2.rotation.y + absomaster.rotation.y;
                            rot.z = recobstacles2.rotation.z + absomaster.rotation.z;
                            rot.w = recobstacles2.rotation.w + absomaster.rotation.w;
                            obstacless2.id_list.Remove(0);
                        }
                    }
                }
            }
            Objmgr<GameObject> obstmp = new Objmgr<GameObject> { 
                objlist = new List<GameObject>(obstacless2.objlist),
                id_list = new List<int>(obstacless2.id_list)
            };
            foreach (var list in obstmp.id_list)
            {
                duped = -1;
                //Debug.LogError(JsonConvert.SerializeObject(obstacless2.id_list));
                foreach (var list2 in recobstacles2.id_list)
                {
                    if ((list) == (list2))
                    {
                        duped = list2;
                    }
                }
                if (duped == -1)
                {
                    int index = obstacless2.id_list.IndexOf(list);
                    Debug.LogError(index +":"+JsonConvert.SerializeObject(obstacless2.id_list));
                    if (index != -1)
                    {
                        Destroy(obstacless2.objlist[index]);
                        obstacless2.Remove(obstacless2.objlist[index]);

                    }
                }
            }
        }
        if (recsyringe != null)
        {
            foreach (var list in recsyringe.id_list)
            {
                duped = -1;
                //Debug.LogError(JsonConvert.SerializeObject(syringes.id_list));
                foreach (var list2 in syringes.id_list)
                {
                    if ((list) == (list2))
                    {
                        duped = list2;
                        if ((list) == (recsyringe.id))
                        {
                            if (syringes.objlist.Count < recsyringe.id_list.Length)
                            {
                                pos.y = recsyringe.position.y + absomaster.position.y;
                                pos.x = recsyringe.position.x + absomaster.position.x;
                                pos.z = recsyringe.position.z + absomaster.position.z;
                                rot.x = recsyringe.rotation.x + absomaster.rotation.x;
                                rot.y = recsyringe.rotation.y + absomaster.rotation.y;
                                rot.z = recsyringe.rotation.z + absomaster.rotation.z;
                                rot.w = recsyringe.rotation.w + absomaster.rotation.w;
                                syringes.id_list.Remove(recsyringe.id - 1);
                                syringes.Add(Instantiate(syringe, pos, rot));
                            }
                            else
                            {
                                pos.y = recsyringe.position.y + absomaster.position.y;
                                pos.x = recsyringe.position.x + absomaster.position.x;
                                pos.z = recsyringe.position.z + absomaster.position.z;
                                rot.x = recsyringe.rotation.x + absomaster.rotation.x;
                                rot.y = recsyringe.rotation.y + absomaster.rotation.y;
                                rot.z = recsyringe.rotation.z + absomaster.rotation.z;
                                rot.w = recsyringe.rotation.w + absomaster.rotation.w;
                                syringes.objlist[syringes.id_list.IndexOf(list)].transform.SetPositionAndRotation(pos, rot);
                            }

                        }
                    }
                }
                if (duped == -1)
                {
                    if (((list) == (recsyringe.id)) && (syringes.objlist.Count < recsyringe.id_list.Length))
                    {
                        //Debug.LogError(recsyringe.id + ";" + list + ";" + JsonConvert.SerializeObject(syringes.id_list));
                        syringes.Add(Instantiate(syringe, pos, rot), list);
                        if (!once)
                        {
                            pos.y = recsyringe.position.y + absomaster.position.y;
                            pos.x = recsyringe.position.x + absomaster.position.x;
                            pos.z = recsyringe.position.z + absomaster.position.z;
                            rot.x = recsyringe.rotation.x + absomaster.rotation.x;
                            rot.y = recsyringe.rotation.y + absomaster.rotation.y;
                            rot.z = recsyringe.rotation.z + absomaster.rotation.z;
                            rot.w = recsyringe.rotation.w + absomaster.rotation.w;
                            syringes.id_list.Remove(0);
                        }
                    }
                }
            }
            Objmgr<GameObject> syringetmp = new Objmgr<GameObject> { 
                objlist = new List<GameObject>(syringes.objlist),
                id_list = new List<int>(syringes.id_list)
            };
            foreach (var list in syringetmp.id_list)
            {
                duped = -1;
                //Debug.LogError(JsonConvert.SerializeObject(syringes.id_list));
                foreach (var list2 in recsyringe.id_list)
                {
                    if ((list) == (list2))
                    {
                        duped = list2;
                    }
                }
                if (duped == -1)
                {
                    int index = syringes.id_list.IndexOf(list);
                    Destroy(syringes.objlist[index]);
                    syringes.Remove(syringes.objlist[index]);
                }
            }
        }
    }
    public void RecvText(string text)
    {
        Debug.Log(text);
        var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(text);
        string type = jsonObject["type"].ToString();
        // int seq = int.Parse(jsonObject["seq"].ToString());
        // Debug.Log(type);
        // if (type != "data") { Debug.Log(text); }
        switch (type)
        {
            //case "login":
            //    clientdata = JsonConvert.DeserializeObject<Clientdata>(jsonObject["data"].ToString());
            //    Debug.Log(clientdata.seq);
            //    break;
            case "data":
                // Debug.Log(seq + " : " + clientdata.seq);
                //Debug.Log(jsonObject["data"].ToString());
                recdata = JsonConvert.DeserializeObject<Objdat>(jsonObject["data"].ToString());
                if (recdata != null) reclist[recdata.id] = recdata;
                break;
            case "d_glock":
                //Debug.Log(jsonObject["data"].ToString());
                recglock = JsonConvert.DeserializeObject<Objdat>(jsonObject["data"].ToString());
                break;
            case "d_sield":
                recsield = JsonConvert.DeserializeObject<sieldobj>(jsonObject["data"].ToString());
                break;
            case "d_bullet":
                recbullet = JsonConvert.DeserializeObject<Objdat>(jsonObject["data"].ToString());
                break;
            case "d_obstacles":
                recobstacles = JsonConvert.DeserializeObject<Objdat>(jsonObject["data"].ToString());
                break;
            case "d_obstacles2":
                recobstacles2 = JsonConvert.DeserializeObject<Objdat>(jsonObject["data"].ToString());
                break;
            case "d_syringe":
                recsyringe = JsonConvert.DeserializeObject<Objdat>(jsonObject["data"].ToString());
                break;
            case "ammos":
                status = JsonConvert.DeserializeObject<Status>(jsonObject["data"].ToString());
                ammo[status.id] = status.data;
                break;
            case "sield":
                status = JsonConvert.DeserializeObject<Status>(jsonObject["data"].ToString());
                //sieldhp[status.id] = status.data;
                break;
            case "hp":
                status = JsonConvert.DeserializeObject<Status>(jsonObject["data"].ToString());
                hps = status.data;
                Debug.Log(status.data);
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
        once = false;
        //接続処理。接続先サーバと、ポート番号を指定する
        ws = new WebSocket("ws://" + ip + ":" + port);
        ws.Connect();

        //送信ボタンが押されたときに実行する処理「SendText」を登録する
        // sendButton.onClick.AddListener(SendText);
        //サーバからメッセージを受信したときに実行する処理「RecvText」を登録する
        ws.OnMessage += (sender, e) => RecvText(e.Data);
        //サーバとの接続が切れたときに実行する処理「RecvClose」を登録する
        ws.OnClose += (sender, e) => RecvClose();
        duped = new int();
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

        reclist = new Objdat[20];
        glocks = new Objmgr<GameObject>
        {
            objlist = new List<GameObject>(),
            id_list = new List<int> { 0 }
        };
        sields = new Objmgr<GameObject>
        {
            objlist = new List<GameObject>(),
            id_list = new List<int> { 0 }
        };
        bullets = new Objmgr<GameObject>
        {
            objlist = new List<GameObject>(),
            id_list = new List<int> { 0 }
        };
        obstacless = new Objmgr<GameObject>
        {
            objlist = new List<GameObject>(),
            id_list = new List<int> { 0 }
        };
        obstacless2 = new Objmgr<GameObject>
        {
            objlist = new List<GameObject>(),
            id_list = new List<int> { 0 }
        };
        syringes = new Objmgr<GameObject>
        {
            objlist = new List<GameObject>(),
            id_list = new List<int> { 0 }
        };
        //dupedlist = new int[20];

        //Debug.Log(glock.name);

        pos = new Vector3();
        rot = new Quaternion();
        ammo = new string[8];
        sieldhp = new int[8];
    }
    public void Senddat()
    {
        ws.Send(JsonConvert.SerializeObject(senddata));
    }
}


