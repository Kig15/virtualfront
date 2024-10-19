
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespownPoint1 : MonoBehaviour
{
    public GameObject PlayerObj;
    public string TeamTag = "GhostTeam";
    private PlayerController playerController;
   
  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        PlayerObj = other.transform.root.gameObject;
        playerController = PlayerObj.GetComponent<PlayerController>();
        if (playerController != null)
        {
            PlayerObj.tag = TeamTag;
            playerController.ChangeMaterial();
            playerController.ButtleOK = true;
        }
    }


    public void OnTriggerStay(Collider other)
    {
       /*
        if (playerController != null && Input.GetKeyDown(KeyCode.L))
        {
            PlayerObj.tag = TeamTag;
            playerController.ChangeMaterial();
        }
       */
    }
}
