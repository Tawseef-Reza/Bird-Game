using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshEnemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    private NavMeshPath currentPath;
    private int currentCornerIndex = 0;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform[] positions;
    private int index = 0;

    [SerializeField] private EnemyInfo enemyInfo;
    private string currentAnim = "IdleLook";
    [SerializeField] private Animator animator;
    [SerializeField] private float waitTime;
    [SerializeField] private float rotSpeed;
    [SerializeField] private float speed;


    // Start is called before the first frame update
    void Start()
    {
        agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();   
    }

    private void Patrol()
    {
        
        //print(agent.transform.rotation.eulerAngles);

        if (!agent.hasPath)
        {
            currentCornerIndex = 0;
            currentPath = new();
            agent.CalculatePath(positions[index].position, currentPath);
            agent.SetPath(currentPath);
            incrementIndex();
        }
        else
        {
            if (!agent.updatePosition && agent.updateRotation)
            {
                if (Vector3.Angle(transform.forward, currentPath.corners[currentCornerIndex] - transform.position) < 1f)
                {
                    print("rotation done, moving on");
                    agent.updatePosition = true;
                    agent.updateRotation = false;
                }
                else
                {
                    print("rotation not done");
                }
            }
            else if (Vector3.Distance(transform.position, currentPath.corners[currentCornerIndex]) <= 0.01f)
            {
                agent.updatePosition = false;
                agent.updateRotation = true;
                print("corner reached at " + transform.position + " at index " + currentCornerIndex + ", going to next");

                currentCornerIndex++;
            }
        }
    }


    private void incrementIndex()
    {
        if (index == positions.Length - 1)
        {
            index = 0;
        }
        else
        {
            index++;
        }
    }
    private IEnumerator WaitIdle()
    {
        yield return new WaitForSeconds(waitTime);
        enemyInfo._enemyState = EnemyState.Moving;
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
        Gizmos.DrawCube(currentPath.corners[currentCornerIndex], new Vector3(1,1,1));
    }
}
