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
    [SerializeField] private float ellipseExtension = 0.2f;
    [SerializeField] private float inverseScaleMinorAxisFactor = 2f;
    [SerializeField] private float inverseScaleMinorAxisFactorMin = 1.4f;
    [SerializeField] private float inverseScaleMinorAxisFactorMax = 4f;


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
        float dist = Vector3.Distance(LeftLeg.position, rightLegSavedPos);
        if (dist >= 0.67f)
        {
            inverseScaleMinorAxisFactor = inverseScaleMinorAxisFactorMax;
        }
        else if (dist <= 0f)
        {
            inverseScaleMinorAxisFactor = inverseScaleMinorAxisFactorMin;
        }
        else
        {
            inverseScaleMinorAxisFactor = Map(dist, 0, 0.67f, inverseScaleMinorAxisFactorMin, inverseScaleMinorAxisFactorMax);
        }

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Input.GetAxis("Vertical") * Time.deltaTime * speed + transform.right * Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        if (incrementingLeftLeg || incrementingRightLeg)
        {
            if (incrementingLeftLeg)
            {
                MoveLeftLeg();
            }
            if (incrementingRightLeg)
            {
                MoveRightLeg();
            }
        }
        else if (!checkIfInEllipse())
        {
            if (legDistToTransform(LeftLeg) > legDistToTransform(RightLeg))
            {
                leftLegSavedPos = 2 * transform.position - RightLeg.position;
                float dist = Vector3.Distance(LeftLeg.position, rightLegSavedPos);
                if (dist >= 0.67f)
                {
                    inverseScaleMinorAxisFactor = inverseScaleMinorAxisFactorMax;
                }
                else if (dist <= 0f)
                {
                    inverseScaleMinorAxisFactor = inverseScaleMinorAxisFactorMin;
                }
                else
                {
                    inverseScaleMinorAxisFactor = Map(dist, 0, 0.67f, inverseScaleMinorAxisFactorMin, inverseScaleMinorAxisFactorMax);
                }
                print(inverseScaleMinorAxisFactor + " is inverseScaleMinorAxisFactor with dist " + dist);

                incrementingLeftLeg = true;
            }
            else
            {
                rightLegSavedPos = 2 * transform.position - LeftLeg.position;
                float dist = Vector3.Distance(LeftLeg.position, rightLegSavedPos);
                if (dist >= 0.67f)
                {
                    inverseScaleMinorAxisFactor = inverseScaleMinorAxisFactorMax;
                }
                else if (dist <= 0f)
                {
                    inverseScaleMinorAxisFactor = inverseScaleMinorAxisFactorMin;
                }
                else
                {
                    inverseScaleMinorAxisFactor = Map(dist, 0, 0.67f, inverseScaleMinorAxisFactorMin, inverseScaleMinorAxisFactorMax);
                }
                print(inverseScaleMinorAxisFactor + " is inverseScaleMinorAxisFactor with dist " + dist);
                incrementingRightLeg = true;
            }
        }
        else
        {
            lockLegs();
        }
        //print(Vector3.Distance(LeftLeg.position, RightLeg.position).ToString());
        //left leg : Vector3(-0.231000006,0,0.128999993)
        // right leg: Vector3(0.231999993,0,0.167999998)
        


    }
    void FixedUpdate()
    {
        //transform.position += transform.forward * Time.fixedDeltaTime * speed;
    }
    private float legDistToTransform(Transform leg)
    {
        return Vector2.Distance(new Vector2(leg.position.x, leg.position.z), new Vector2(transform.position.x, transform.position.z));
    }

    private void lockLegs()
    {
        LeftLeg.position = leftLegSavedPos;
        RightLeg.position = rightLegSavedPos;
    }

    private void MoveLeftLeg()
    {
        
        LeftLeg.position = leftLegSavedPos;
        incrementingLeftLeg = false;
        
    }

    private void MoveRightLeg()
    {
        
        RightLeg.position = rightLegSavedPos;
        incrementingRightLeg = false;
    }
    private bool checkIfInEllipse()
    {
        Vector3 center = (LeftLeg.position + RightLeg.position) / 2;
        //print(RightLeg.parent.right.ToString() + " is right" + RightLeg.parent.forward.ToString() + " is left");
        //print(center.ToString() + " is center");
        //print(Vector3.SignedAngle(transform.right, RightLeg.position - center, -transform.up).ToString());
        // x, y >> transform.position - center
        // h, k >> 0, 0
        // A = Vector3.SignedAngle(transform.right, RightLeg.position - center, -RightLeg.parent.up)
        // semi major axis >> RightLeg.position, center << 2d distance then + some extension value
        // semi minor axis >> half semi major
        Vector3 XZOffset = transform.position - center;
        float x = XZOffset.x;
        float z = XZOffset.z;
        
        float semiMajor = Vector2.Distance(new Vector2(center.x, center.z), new Vector2(RightLeg.position.x, RightLeg.position.z)) + ellipseExtension;
        float semiMinor = semiMajor / inverseScaleMinorAxisFactor;
        float angleDeg = Vector3.SignedAngle(transform.right, RightLeg.position - center, -transform.up);
        float angle = Mathf.Deg2Rad * angleDeg;
        return Mathf.Pow(x * Mathf.Cos(angle) + z * Mathf.Sin(angle), 2) / (semiMajor * semiMajor) + Mathf.Pow(x * Mathf.Sin(angle) - z * Mathf.Cos(angle), 2) / (semiMinor * semiMinor) <= 1;
        



    }
    private void OnDrawGizmos()
    {
        Vector2 pointOne = new Vector2(LeftLeg.position.x, LeftLeg.position.z);
        Vector2 pointTwo = new Vector2(RightLeg.position.x, RightLeg.position.z);
        float XRadius = 1f;
        float YRadius = 2f;
        int Segments = 20;
        Color color = Color.red;
        Vector3 center = (RightLeg.position + LeftLeg.position) / 2;
        float semiMajor = Vector3.Distance(RightLeg.position, center) + ellipseExtension;
        DrawEllipse(center, transform.up, RightLeg.position - center, semiMajor / inverseScaleMinorAxisFactor, semiMajor, Segments, color);

        Gizmos.DrawCube((RightLeg.position + LeftLeg.position) / 2, new Vector3(.1f, .1f, .1f));
    }
    private static void DrawEllipse(Vector3 pos, Vector3 forward, Vector3 up, float radiusX, float radiusY, int segments, Color color, float duration = 0)
    {
        float angle = 0f;
        Quaternion rot = Quaternion.LookRotation(forward, up);
        Vector3 lastPoint = Vector3.zero;
        Vector3 thisPoint = Vector3.zero;

        for (int i = 0; i < segments + 1; i++)
        {
            thisPoint.x = Mathf.Sin(Mathf.Deg2Rad * angle) * radiusX;
            thisPoint.y = Mathf.Cos(Mathf.Deg2Rad * angle) * radiusY;

            if (i > 0)
            {
                Debug.DrawLine(rot * lastPoint + pos, rot * thisPoint + pos, color, duration);
            }

            lastPoint = thisPoint;
            angle += 360f / segments;
        }
    }
    private float Map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

}

