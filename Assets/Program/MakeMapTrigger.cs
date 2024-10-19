using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeMapTrigger : MonoBehaviour
{
    public OVRGrabbable_DeadCOPY grabbable;
    private OVRInput.Controller controller;
    public OVRInput.Button shotButton;
    public MakeMap MapGenerator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MapGeneratorTrigger();
    }

    public void MapGeneratorTrigger()
    {
        if (grabbable.isGrabbed)
        {
            //controller = grabbable.grabbedBy.GetController();
        }//掴んだらコントローラー取得
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MapGenerator.MapGenerator();
        }
    }
}
