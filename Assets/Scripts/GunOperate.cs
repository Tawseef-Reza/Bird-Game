using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

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

    private bool isFree = true;
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
        if (Input.GetMouseButtonDown(0) && isFree)
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
            if (currentGunAnim != "GunReload")
            {
                currentGunAnim = "GunReload";
                gunAnimator.CrossFade(currentGunAnim, 0.2f);
                glockScript.reloading = true;
            }
        }
        else if (isFree)
        {
            string anim = "";
            switch (Input.GetAxisRaw("Horizontal"))
            {
                case -1:
                    anim = "GunSwayLeft";
                    break;
                case 0:
                    anim = "GunIdle";
                    break;
                case 1:
                    anim = "GunSwayRight";
                    break;
            }
            if (currentGunAnim != anim)
            {
                currentGunAnim = anim;
                if (currentGunAnim == "GunIdle")
                {
                    gunAnimator.CrossFade(currentGunAnim, 0.2f);
                }
                else
                {
                    gunAnimator.CrossFade(currentGunAnim, 0.2f);
                }
            }
        }
    }
}
