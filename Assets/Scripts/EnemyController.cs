using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform[] positions;
    private int index = 0;
    [SerializeField] private float speed;
    [SerializeField] private float rotSpeed;
    private bool currentlyRotating = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (currentlyRotating)
        {
            Vector3 targetDir = positions[index].position - transform.position;
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDir.normalized, Time.deltaTime * rotSpeed, 0.0f));
            currentlyRotating = Vector3.Distance(transform.forward, targetDir.normalized) >= 0.01f;

        }

    }
    void FixedUpdate()
    {
        if (!currentlyRotating)
        {
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
                currentlyRotating = true;
            }
        }


    }

}
