using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour , Oculus.Interaction.ISelector
{
    public event Action WhenSelected;
    public event Action WhenUnselected;

    private bool manualGrab = false;
    private GameObject targetObject;
    public Oculus.Interaction.GrabInteractor grabInteractor;

    // 任意のコードでGrabを実行するメソッド
    // public void TriggerGrab(GameObject obj)
    // {
    //     targetObject = obj;
    //     manualGrab = true;
    // }

    public bool Select(GameObject obj)
    {
        // 手動で設定した場合のみ対象を掴む
        if (manualGrab && obj == targetObject)
        {
            // manualGrab = false;
            return true;
        }
        return false;
    }

    void Update(){
        if (OVRInput.Get(OVRInput.Button.Two) ){
            manualGrab = true;
        }else{
            manualGrab = false;
        }
    }

    void Start(){
        grabInteractor.InjectSelector(this);
    }
}
