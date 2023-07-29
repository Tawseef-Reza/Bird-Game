using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class AnimateEnemy : MonoBehaviour
{

    [SerializeField] private Transform LeftLeg;
    [SerializeField] private Transform RightLeg;
    [SerializeField] private float speed;
    [SerializeField] private float stepHeight;
    [SerializeField] private float liftSpeed;
    [SerializeField] private float legTransitionSpeed;


    private Vector3 leftLegSavedPos;
    private Vector3 rightLegSavedPos;

    private Vector3 leftLegSavedLocalPos;
    private Vector3 rightLegSavedLocalPos;

    private bool incrementingLeftLeg;
    private bool incrementingRightLeg;

    

    private float leftLerpLift = 0;
    private float rightLerpLift = 0;


    private enum CurrentState
    {
        Walking,
        Jumping,
        Running,
        Turning
    }
    private CurrentState currentState;
    // Start is called before the first frame update
    void Start()
    {
        leftLegSavedPos = LeftLeg.position;
        rightLegSavedPos = RightLeg.position;

        leftLegSavedLocalPos = LeftLeg.localPosition;
        rightLegSavedLocalPos = RightLeg.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //print("LeftLeg is " + Vector3.Distance(LeftLeg.position, transform.position) + " away, with with left leg at " + LeftLeg.position.ToString() + " and transform at " + transform.position.ToString());
        //print("RightLeg is " + Vector3.Distance(RightLeg.position, transform.position) + " away, with with right leg at " + RightLeg.position.ToString() + " and transform at " + transform.position.ToString());
        //LeftLeg.position = leftLegSavedPos;
        //RightLeg.position = rightLegSavedPos;
        //ManageLeftLeg();
        //ManageRightLeg();
    }
    void FixedUpdate()
    {
        //transform.position += transform.forward * Time.fixedDeltaTime * speed;
    }

    private void ManageLeftLeg()
    {
        if (incrementingLeftLeg)
        {
            if (leftLerpLift < 1 && Vector3.Distance(LeftLeg.localPosition, leftLegSavedLocalPos) > 0.01f)
            {
                LeftLeg.localPosition = new Vector3(Mathf.Lerp(LeftLeg.localPosition.x, leftLegSavedLocalPos.x, legTransitionSpeed * Time.deltaTime), leftLegSavedLocalPos.y + Mathf.Sin(Mathf.PI * leftLerpLift) * stepHeight, Mathf.Lerp(LeftLeg.localPosition.z, leftLegSavedLocalPos.z, legTransitionSpeed * Time.deltaTime));
                leftLerpLift += Time.deltaTime * liftSpeed;
            }
            else
            {
                leftLerpLift = 0;
                leftLegSavedPos = LeftLeg.position;
                incrementingLeftLeg = false;
            }
        }
        else if (Vector3.Distance(LeftLeg.position, transform.position) >= 0.4f)
        {
            incrementingLeftLeg = true;
        }
        else
        {
            LeftLeg.position = leftLegSavedPos;
        }
    }

    private void ManageRightLeg()
    {
        if (incrementingRightLeg)
        {
            if (rightLerpLift < 1 && Vector3.Distance(RightLeg.localPosition, rightLegSavedLocalPos) > 0.01f)
            {
                RightLeg.localPosition = new Vector3(Mathf.Lerp(RightLeg.localPosition.x, rightLegSavedLocalPos.x, legTransitionSpeed * Time.deltaTime), rightLegSavedLocalPos.y + Mathf.Sin(Mathf.PI * rightLerpLift) * stepHeight, Mathf.Lerp(RightLeg.localPosition.z, rightLegSavedLocalPos.z, legTransitionSpeed * Time.deltaTime));
                rightLerpLift += Time.deltaTime * liftSpeed;
            }
            else
            {
                rightLerpLift = 0;
                rightLegSavedPos = RightLeg.position;
                incrementingRightLeg = false;
            }
        }
        else if (Vector3.Distance(RightLeg.position, transform.position) >= 0.4f)
        {
            incrementingRightLeg = true;
        }
        else
        {
            RightLeg.position = rightLegSavedPos;
        }
    }

   
}

