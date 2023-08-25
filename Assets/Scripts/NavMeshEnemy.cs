using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

public class NavMeshEnemy : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private float maxVelocityChange = 10f;
    private float chaseTimeCheck = 0;
    private NavMeshPath currentPath;
    private int currentCornerIndex = 0;
    private NavMeshPath chasePath;
    private int currentChaseCornerIndex = 1;
    [SerializeField] private Transform[] positions;
    private int currentPosToGo = 0;

    [SerializeField] private EnemyInfo enemyInfo;
    private string currentAnim = "IdleLook";
    [SerializeField] private Animator animator;
    [SerializeField] private float waitTime;
    [SerializeField] private float rotSpeed;
    [SerializeField] private float rotMult;
    [SerializeField] private float speed;
    [SerializeField] private float speedMult;

    // Start is called before the first frame update
    void Start()
    {
        currentPath = new NavMeshPath();
        chasePath = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyInfo._enemyState != EnemyState.Chasing)
        {
            Patrol();
        }
        else
        {
            Chase();
        }
        
    }

    void FixedUpdate()
    {
        if (enemyInfo._enemyState != EnemyState.Chasing)
        {
            PatrolMove();
        }
        else
        {
            ChaseMove();
        }
    }

    private void Patrol()
    {
        if (currentPath.status == NavMeshPathStatus.PathInvalid) {
            NavMesh.CalculatePath(transform.position, positions[currentPosToGo].position, NavMesh.AllAreas, currentPath);
        }
        else
        {
            
            if (enemyInfo._enemyState == EnemyState.IdleLook)
            {
                if (currentAnim != "IdleLook")
                { 
                    currentAnim = "IdleLook";
                    animator.CrossFade(currentAnim, .1f);
                    StartCoroutine(WaitIdle());
                }
            }
            else if (enemyInfo._enemyState == EnemyState.Rotating)
            {
                if (currentAnim != "Rotate")
                {
                    currentAnim = "Rotate";
                    animator.CrossFade(currentAnim, .1f);
                }
                Vector3 targetFixed = new Vector3(currentPath.corners[currentCornerIndex].x, transform.position.y, currentPath.corners[currentCornerIndex].z);
                Vector3 targetDir = targetFixed - transform.position;
                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDir.normalized, Time.deltaTime * rotSpeed, 0.0f));

                if (Vector3.Distance(transform.forward, targetDir.normalized) >= 0.01f && (Vector3.Distance(targetFixed, transform.position) >= 0.01f))
                {
                    enemyInfo._enemyState = EnemyState.Rotating;
                }
                else
                {
                    enemyInfo._enemyState = EnemyState.Moving;
                }
            }
        }
    }

    private void PatrolMove()
    {
        if (enemyInfo._enemyState == EnemyState.Moving)
        {
            //print("moving, with dist " + Vector3.Distance(currentPath.corners[currentCornerIndex], transform.position).ToString());
            if (currentAnim != "Walk")
            {
                currentAnim = "Walk";
                animator.CrossFade(currentAnim, 0f);
            }
            Vector3 movementDir = (currentPath.corners[currentCornerIndex] - transform.position).normalized * speed * Time.fixedDeltaTime;

            rb.velocity = movementDir;

            //transform.position = Vector3.MoveTowards(transform.position, currentPath.corners[currentCornerIndex], Time.fixedDeltaTime * speed);
            if (Vector3.Distance(currentPath.corners[currentCornerIndex], transform.position) <= 0.1f)
            {
                if (currentCornerIndex == currentPath.corners.Length - 1)
                {
                    currentPath = new NavMeshPath();
                    enemyInfo._enemyState = EnemyState.IdleLook;
                    currentCornerIndex = 0;
                    if (currentPosToGo == positions.Length - 1)
                    {
                        currentPosToGo = 0;
                    }
                    else
                    {
                        currentPosToGo++;
                    }
                }
                else
                {
                    currentCornerIndex++;
                    enemyInfo._enemyState = EnemyState.Rotating;
                }
            }
        }
    }

    private void Chase()
    {
        if (chasePath.status != NavMeshPathStatus.PathInvalid)
        {
            Vector3 targetFixed = new Vector3(chasePath.corners[currentChaseCornerIndex].x, transform.position.y, chasePath.corners[currentChaseCornerIndex].z);
            Vector3 targetDir = targetFixed - transform.position;

            //transform.position = Vector3.MoveTowards(transform.position, chasePath.corners[currentChaseCornerIndex], Time.fixedDeltaTime * speed * speedMult);
            if (Vector3.Distance(transform.forward, targetDir.normalized) >= 0.01f && (Vector3.Distance(targetFixed, transform.position) >= 0.01f))
            {
                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDir.normalized, Time.deltaTime * rotSpeed * rotMult, 0.0f));
            }
        }
        
    }

    private void ChaseMove()
    {
        //print("chase time check is " + chaseTimeCheck + " which is " + (chaseTimeCheck > 0.5f ? "greater" : "still lower"));
        if (currentAnim != "Run")
        {
            currentAnim = "Run";
            animator.CrossFade(currentAnim, 0.1f);
        }
        if (chasePath.status == NavMeshPathStatus.PathInvalid)
        {
            NavMesh.CalculatePath(transform.position, enemyInfo.target.transform.position, NavMesh.AllAreas, chasePath);
            print(chasePath.corners.Length);
        }
        else
        {
            
            if (Vector3.Distance(chasePath.corners[currentChaseCornerIndex], transform.position) <= 0.1f)
            {
                if (currentChaseCornerIndex == chasePath.corners.Length - 1)
                {
                    chasePath = new NavMeshPath();
                    currentChaseCornerIndex = 1;
                    chaseTimeCheck = 0;
                }
                else
                {
                    currentChaseCornerIndex++;
                }
            }
            else
            {
                Vector3 movementDir = (chasePath.corners[currentChaseCornerIndex] - transform.position).normalized * speed * speedMult * Time.fixedDeltaTime;

                rb.velocity = movementDir;
            }
            chaseTimeCheck += Time.fixedDeltaTime;
            if (chaseTimeCheck >= 0.5f)
            {
                chasePath = new NavMeshPath();
                currentChaseCornerIndex = 1;
                chaseTimeCheck = 0;
            }
        }
        


    }

    private IEnumerator WaitIdle()
    {
        yield return new WaitForSeconds(waitTime);
        enemyInfo._enemyState = EnemyState.Rotating;
    }

    public void ResetPatrolPath()
    {
        currentPath = new NavMeshPath();
        currentCornerIndex = 0;
        NavMesh.CalculatePath(transform.position, positions[currentPosToGo].position, NavMesh.AllAreas, currentPath);
        // resets the chase
        chasePath = new NavMeshPath();
        currentChaseCornerIndex = 1;
    }

    private string ShowCorners(Vector3[] arr)
    {
        string returnStr = "";
        foreach (Vector3 corner in arr)
        {
            returnStr += corner.ToString() + "\n";
        }
        return returnStr;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (currentPath != null)
        {
            if (currentPath.status != NavMeshPathStatus.PathInvalid)
            if (currentPath.status != NavMeshPathStatus.PathInvalid)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(currentPath.corners[currentCornerIndex], new Vector3(1, 1, 1));
            }
        }
        if (chasePath != null) {

            if (chasePath.status != NavMeshPathStatus.PathInvalid)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(chasePath.corners[currentChaseCornerIndex], new Vector3(1, 1, 1));

            }

        }
    }
}
//Vector3 targetFixed = new Vector3(currentPath.corners[currentCornerIndex].x, transform.position.y, currentPath.corners[currentCornerIndex].z);
////print("next corner at " + targetFixed);
//Vector3 targetDir = targetFixed - transform.position;
//if (Vector3.Distance(transform.forward, targetDir.normalized) >= 0.01f && (Vector3.Distance(targetFixed, transform.position) >= 0.01f))
//{

//    transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDir.normalized, Time.deltaTime * rotSpeed, 0.0f));
//    print(Vector3.Distance(transform.forward, targetDir.normalized) + " with transform forward as " + transform.forward + " and targetdir normalized as " + targetDir.normalized);
//    print(Vector3.Distance(targetFixed, transform.position));
//    //print("first if ran + " + (Vector3.Distance(transform.forward, targetDir.normalized) <= 0.01f || (targetDir.normalized.x == 0 && targetFixed.normalized.z == 0)));
//}
//else
//{
//    transform.position = currentPath.corners[currentCornerIndex];
//    if (currentCornerIndex == currentPath.corners.Length - 1)
//    {
//        currentCornerIndex = 0;
//    }
//    else
//    {
//        currentCornerIndex++;
//    }
//    //print("second if ran");
//}