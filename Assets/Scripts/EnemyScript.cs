using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float rotSpeed;
    [SerializeField] private Material _material;
    private GameObject currentTarget;
    private float initialRot;
    private bool forwards = true;
    private bool targetFound = false;
    // Start is called before the first frame update
    void Start()
    {
        initialRot = transform.eulerAngles.y; 
    }

    // Update is called once per frame
    void Update()
    {
        if (forwards && transform.eulerAngles.y >= initialRot + 90)
        {
            forwards = false;
        }
        else if (!forwards && transform.eulerAngles.y <= initialRot - 90)
        {
            forwards = true;
        }
        RaycastHit hitInfo;
        bool isHit = Physics.Raycast(transform.position, transform.forward, out hitInfo);
        if (!targetFound)
        {
            transform.Rotate(0, (forwards ? 1 : -1) * Time.deltaTime * rotSpeed, 0);
           
            if (isHit)
            {
                print(hitInfo.collider.name);
                if (hitInfo.collider.name == "FirstPersonController")
                {
                    _material.color = new Color(.67f, .200f, .190f);
                    currentTarget = hitInfo.collider.gameObject;
                    targetFound = true;
                }
                else
                {
                    _material.color = new Color(.200f, .79f, .67f);
                }
            }
        }
        else
        {
            if (transform.eulerAngles.y <= initialRot - 90 || transform.eulerAngles.y >= initialRot + 90)
            {
                targetFound = false;
                _material.color = new Color(.200f, .79f, .67f);
                currentTarget = null;
            }
            else if (isHit && hitInfo.collider.name != "FirstPersonController")
            {
                targetFound = false;
                _material.color = new Color(.200f, .79f, .67f);
                currentTarget = null;
            }
            else
            {
                transform.LookAt(currentTarget.transform);
            }

        }
        
        
        

        
    }
}
