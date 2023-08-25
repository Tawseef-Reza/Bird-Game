using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Rotating,
    Moving,
    IdleLook,
    Idle,
    Searching,
    SearchingRotate,
    SearchingMove,
    Chasing
}
public class EnemyInfo : MonoBehaviour
{
    // Start is called before the first frame update
   
    public EnemyState _enemyState = EnemyState.Rotating;
    public GameObject target = null;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
