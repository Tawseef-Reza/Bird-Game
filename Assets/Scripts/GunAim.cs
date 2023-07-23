using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAim : MonoBehaviour
{
    [SerializeField] private Rigidbody player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // left is negative
        print(player.velocity.x / 4);
        
        transform.localPosition = new Vector3 (-0.75f, -0.175f, player.velocity.x/16);  
    }
}
