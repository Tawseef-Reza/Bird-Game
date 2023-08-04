using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform[] positions;
    private int index = 0;
    [SerializeField] private float speed;
    [SerializeField] private float rotSpeed;
    
    [SerializeField] private Animator animator;
    private string currentAnim = "IdleLook";
    [SerializeField] private float waitTime;
    [SerializeField] private EnemyInfo enemyInfo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyInfo._enemyState == EnemyState.Rotating)
        {
            if (currentAnim != "Rotate")
            {
                currentAnim = "Rotate";
                animator.CrossFade(currentAnim, .1f);
            }
            Vector3 targetDir = positions[index].position - transform.position;
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDir.normalized, Time.deltaTime * rotSpeed, 0.0f));
            enemyInfo._enemyState = Vector3.Distance(transform.forward, targetDir.normalized) >= 0.01f ? EnemyState.Rotating : EnemyState.Idle;

        }
        else if (enemyInfo._enemyState == EnemyState.Idle)
        {
            if (currentAnim != "IdleLook") {

                currentAnim = "IdleLook";
                animator.CrossFade(currentAnim, .1f);
                StartCoroutine(WaitIdle());
            }
        }

    }
    void FixedUpdate()
    {
        if (enemyInfo._enemyState == EnemyState.Moving)
        {
            if (currentAnim != "Walk")
            {
                currentAnim = "Walk";
                animator.CrossFade(currentAnim, 0f);
            }
            transform.position = Vector3.MoveTowards(transform.position, positions[index].position, Time.fixedDeltaTime * speed);
            if (transform.position == positions[index].position)
            {
                if (index == positions.Length - 1)
                {
                    index = 0;
                }
                else
                {
                    index++;
                }
                enemyInfo._enemyState = EnemyState.Rotating;
            }
        }
        else if (enemyInfo._enemyState == EnemyState.Chasing)
        {
            if (currentAnim != "Run")
            {
                currentAnim = "Run";
                animator.CrossFade(currentAnim, 0f);
            }
        }
    }

    

    private IEnumerator WaitIdle()
    {
        yield return new WaitForSeconds(waitTime);
        enemyInfo._enemyState = EnemyState.Moving;
    }

}
