using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchHandling : MonoBehaviour
{

    public bool isCrouched = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        print(isCrouched + " is value of crouch");
        if (Input.GetKey(KeyCode.LeftControl))
        {
            isCrouched = true;
        }
        else
        {
            isCrouched = false;
        }
    }
}
