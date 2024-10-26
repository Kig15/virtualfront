using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCOntroller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0, 0, 0.001f); 
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += new Vector3(0, 0, -0.001f);
        }
    }
}
