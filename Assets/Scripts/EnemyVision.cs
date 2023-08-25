using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private float angle;
    [SerializeField] private float rangeClose;
    [SerializeField] private float rangeChase;
    [SerializeField] private float rangeLeave;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private EnemyInfo enemyInfo;

    [SerializeField] private NavMeshEnemy navMeshEnemy;
    void Start()
    {
        
    }

    void Update()
    {
        
        if (enemyInfo._enemyState != EnemyState.Chasing)
        {
            //foreach (Collider obj in Physics.OverlapSphere(head.position, rangeChase, playerLayer))
            //{
            //    if (Vector3.Angle(head.forward, obj.transform.position - head.position) < angle)
            //    {
            //        enemyInfo.target = obj.gameObject;
            //        enemyInfo._enemyState = EnemyState.Chasing;
            //        break;
            //    }
            //}
            Collider[] rangeObjs = Physics.OverlapSphere(head.position, rangeChase, playerLayer);
            Collider[] closeObjs = Physics.OverlapSphere(head.position, rangeClose, playerLayer);

            
            if (rangeObjs.Length > 0)
            {
                bool checkObstacle = Physics.Raycast(head.position, rangeObjs[0].transform.position - head.position, out RaycastHit info);
                if (Vector3.Angle(head.forward, rangeObjs[0].transform.position - head.position) < angle && checkObstacle && info.collider.CompareTag("Player"))
                {
                    enemyInfo.target = rangeObjs[0].gameObject;
                    enemyInfo._enemyState = EnemyState.Chasing;
                }
            }
            if (closeObjs.Length > 0)
            {
                bool checkObstacle = Physics.Raycast(head.position, closeObjs[0].transform.position - head.position, out RaycastHit info);
                if (checkObstacle && info.collider.CompareTag("Player"))
                {
                    enemyInfo.target = closeObjs[0].gameObject;
                    enemyInfo._enemyState = EnemyState.Chasing;
                }
                
            }

            //if (enemyInfo._enemyState != EnemyState.Chasing)
            //{
            //    Collider[] closeObjs = Physics.OverlapSphere(head.position, rangeClose, playerLayer);
            //    if (closeObjs.Length > 0)
            //    { 
            //        enemyInfo.target = closeObjs[0].gameObject;
            //        enemyInfo._enemyState = EnemyState.Chasing;
            //    }
            //}
        }
        else
        {
            bool hit = Physics.Raycast(transform.position, enemyInfo.target.transform.position - transform.position, out RaycastHit info);
            
            //if (!info.collider.CompareTag("Player") && hit)
            //{
            //    enemyInfo._enemyState = EnemyState.Rotating;
            //    enemyInfo.target = null;
            //    navMeshEnemy.ResetPatrolPath();
            //    //StartCoroutine(BeginToSearch());
            //}
            //else 
            if (!Physics.CheckSphere(head.position, rangeLeave, playerLayer))
            {
                enemyInfo._enemyState = EnemyState.Rotating;
                enemyInfo.target = null;
                navMeshEnemy.ResetPatrolPath();
            }
            //Vector3 enemyPos = enemyInfo.target.transform.position;
            //Vector3 targetPos = new Vector3(enemyPos.x, transform.position.y, enemyPos.z);
            //transform.LookAt(targetPos);
            //Vector3 targetPosHead = new Vector3(enemyPos.x, enemyPos.y, enemyPos.z);
            //head.LookAt(targetPosHead);
        }

    }
    private void OnDrawGizmos()
    {
        float angleRot = angle;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(head.position, rangeChase);

        Gizmos.DrawLine(head.position, Quaternion.AngleAxis(-angleRot, head.up) * head.forward * rangeChase + head.position);
        Gizmos.DrawLine(head.position, Quaternion.AngleAxis(angleRot, head.up) * head.forward * rangeChase + head.position);
        Gizmos.DrawLine(head.position, Quaternion.AngleAxis(-angleRot, head.right) * head.forward * rangeChase + head.position);
        Gizmos.DrawLine(head.position, Quaternion.AngleAxis(angleRot, head.right) * head.forward * rangeChase + head.position);
        Gizmos.DrawLine(head.position, Quaternion.AngleAxis(angleRot, head.right) * head.forward * rangeChase + head.position);
        // cone of vision
        for (int i = 0; i < 360; i++)
        {
            Vector3 finalLine1 = Quaternion.AngleAxis(-angleRot, head.right) * head.forward;
            Vector3 finalLine2 = Quaternion.AngleAxis(i, head.forward) * finalLine1;
            Vector3 finalLine3 = finalLine2 * rangeChase + head.position;
            Gizmos.DrawLine(head.position, finalLine3);
        }
        
        

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(head.position, rangeLeave);
       
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(head.position, rangeClose);
    }

    private IEnumerator BeginToSearch()
    {
        yield return new WaitForSeconds(0.3f);
        enemyInfo._enemyState = EnemyState.Rotating;
    }
}
