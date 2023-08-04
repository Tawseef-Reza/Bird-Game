using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private float angle;
    [SerializeField] private float range;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject cube;
    [SerializeField] private Transform startPos;
    [SerializeField] private int cubesLength;
    [SerializeField] private float spacing;
    [SerializeField] private EnemyInfo enemyInfo;
    

    void Start()
    {
        
    }

    void Update()
    {
        if (enemyInfo._enemyState == EnemyState.Chasing)
        {
            Vector3 enemyPos = enemyInfo.target.transform.position;
            Vector3 targetPos = new Vector3(enemyPos.x, transform.position.y, enemyPos.z);
            transform.LookAt(targetPos);
            Vector3 targetPosHead = new Vector3(enemyPos.x, enemyPos.y, enemyPos.z);
            head.LookAt(targetPosHead);


        }
        else
        {
            Collider[] objs = Physics.OverlapSphere(head.position, range, playerLayer);
            foreach (Collider obj in objs)
            {
                if (Vector3.Angle(head.forward, obj.transform.position - head.position) < angle)
                {
                    enemyInfo.targets.Add(obj.gameObject);
                    enemyInfo.target = FindClosest(enemyInfo.targets);
                    enemyInfo._enemyState = EnemyState.Chasing;

                }
            }
        }
        

    }
    private GameObject FindClosest(List<GameObject> objs)
    {
        GameObject current = null;
        foreach (GameObject obj in objs)
        {
            if (current == null)
            {
                current = obj;
            }
            else if (Vector3.Distance(transform.position, current.transform.position) < Vector3.Distance(transform.position, obj.transform.position))
            {
                current = obj;
            }
        }
        return current;
    }
    private void OnDrawGizmos()
    { 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(head.position, range);
        //for (int x = 0; x < cubesLength; x++)
        //{
        //    for (int y = 0; y < cubesLength; y++)
        //    {
        //        for (int z = 0; z < cubesLength; z++)
        //        {
        //            Gizmos.DrawCube(startPos.position + new Vector3(x, y, z) * spacing, new Vector3(1, 1, 1));
        //        }
        //    }
        //}
    }
}
