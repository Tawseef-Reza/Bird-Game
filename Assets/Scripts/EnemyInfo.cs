using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Rotating,
    Moving,
    Idle,
    Chasing
}
public class EnemyInfo : MonoBehaviour
{
    // Start is called before the first frame update
   
    public EnemyState _enemyState = EnemyState.Rotating;
    public List<GameObject> targets = new List<GameObject>();
    public GameObject target;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
