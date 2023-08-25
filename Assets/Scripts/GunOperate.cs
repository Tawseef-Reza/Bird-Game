using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.Animations.Rigging;
using UnityEngine.Animations;

public class GunOperate : MonoBehaviour
{
    [SerializeField] private GameObject gun;
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private TextMeshProUGUI bulletCount;
    public readonly int maxBulletsLeft = 5;
    public int bulletsLeft = 5;
    [SerializeField] private AudioSource emptyShoot;

    [SerializeField] private Animator gunAnimator;
    private string currentGunAnim = "GunIdle";

    [SerializeField] private GlockScript glockScript;

    [SerializeField] private MultiAimConstraint gunAim;
    [SerializeField] private MultiPositionConstraint gunPos;
    [SerializeField] private float snappiness = 50f;
    private bool isFree = true;
    private bool holstered = false;

    // Start is called before the first frame update
    void Start()
    {
        //transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        HandleLogic();
    }

    private void HandleLogic()
    {
        isFree = !glockScript.firing && !glockScript.reloading;
        if (Input.GetMouseButtonDown(0) && isFree && !holstered)
        {
            if (bulletsLeft > 0)
            {
                if (currentGunAnim != "GunShoot")
                {
                    currentGunAnim = "GunShoot";
                    gunAnimator.CrossFade(currentGunAnim, 0f);
                    glockScript.firing = true;
                    if (Physics.Raycast(_camera.position, _camera.forward, out RaycastHit info))
                    {
                        GameObject objectHit = info.collider.gameObject;
                        SonarShader script = objectHit.GetComponent<SonarShader>();
                        if (script != null)
                        {
                            print("shot at and active");
                            script.ShotSequence(info.point);
                        }
                        else
                        {
                            print("fail");
                        }
                    }
                    bulletsLeft--;
                    bulletCount.text = $"{bulletsLeft} / ∞";
                }

            }
            else
            {
                emptyShoot.Play();
            }
            

        }
        else if (Input.GetKeyDown(KeyCode.R) && isFree && bulletsLeft < maxBulletsLeft)
        {
            if (bulletsLeft == 0)
            {
                if (currentGunAnim != "GunReloadEmpty")
                {
                    currentGunAnim = "GunReloadEmpty";
                    gunAnimator.CrossFade(currentGunAnim, 0f);
                    glockScript.reloading = true;
                }
            }
            else
            {
                if (currentGunAnim != "GunReload")
                {
                    currentGunAnim = "GunReload";
                    gunAnimator.CrossFade(currentGunAnim, 0f);
                    glockScript.reloading = true;
                }
            }
            
        }
        else if (isFree)
        {
            if (currentGunAnim != "GunIdle")
            {
                currentGunAnim = "GunIdle";
                gunAnimator.CrossFade(currentGunAnim, 0f);
            }
       
        }
        bool hit = Physics.Raycast(_camera.position, _camera.forward, out RaycastHit generalInfo);

        if (hit && generalInfo.distance < 2.4f)
        {
            SetGunAway(generalInfo.distance);
        }
        else
        {
            SetGunSway();
        }
    }
    private void SetGunAway(float distance)
    {
        
        distance = Mathf.Max(0.5f, distance);
        float magnitude = Map(distance, 0.5f, 2.4f, 1f, 0f);
        holstered = magnitude > 0.4f;
        WeightedTransformArray aim = gunAim.data.sourceObjects;
        aim.SetWeight(0, Mathf.Lerp(aim.GetWeight(0), 0, Time.deltaTime * snappiness));
        aim.SetWeight(1, Mathf.Lerp(aim.GetWeight(1), 0, Time.deltaTime * snappiness));
        aim.SetWeight(2, Mathf.Lerp(aim.GetWeight(2), magnitude, Time.deltaTime * snappiness));
        gunAim.data.sourceObjects = aim;

        WeightedTransformArray pos = gunPos.data.sourceObjects;
        pos.SetWeight(0, Mathf.Lerp(pos.GetWeight(0), 0, Time.deltaTime * snappiness));
        pos.SetWeight(1, Mathf.Lerp(pos.GetWeight(1), 0, Time.deltaTime * snappiness));
        pos.SetWeight(2, Mathf.Lerp(pos.GetWeight(2), magnitude, Time.deltaTime * snappiness));
        gunPos.data.sourceObjects = pos;
    }
    private void SetGunSway()
    {
        holstered = false;
        WeightedTransformArray aim = gunAim.data.sourceObjects;
        aim.SetWeight(0, Mathf.Lerp(aim.GetWeight(0), Mathf.Clamp(-1 * Input.GetAxisRaw("Horizontal"), 0, 1), Time.deltaTime * snappiness));
        aim.SetWeight(1, Mathf.Lerp(aim.GetWeight(1), Mathf.Clamp(Input.GetAxisRaw("Horizontal"), 0, 1), Time.deltaTime * snappiness));
        aim.SetWeight(2, Mathf.Lerp(aim.GetWeight(2), 0, Time.deltaTime * snappiness));
        gunAim.data.sourceObjects = aim;

        WeightedTransformArray pos = gunPos.data.sourceObjects;
        pos.SetWeight(0, Mathf.Lerp(pos.GetWeight(0), Mathf.Clamp(Input.GetAxisRaw("Horizontal"), 0, 1), Time.deltaTime * snappiness));
        pos.SetWeight(1, Mathf.Lerp(pos.GetWeight(1), Mathf.Clamp(-1 * Input.GetAxisRaw("Horizontal"), 0, 1), Time.deltaTime * snappiness));
        pos.SetWeight(2, Mathf.Lerp(pos.GetWeight(2), 0, Time.deltaTime * snappiness));
        gunPos.data.sourceObjects = pos;
    }

    private float Map(float value, float fromLow, float fromHigh, float toLow, float toHigh)
    {
        return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
    }
    /*
    long map(long x, long in_min, long in_max, long out_min, long out_max) {
      return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
    */
}
