using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GlockScript : MonoBehaviour
{
    public bool firing = false;
    public bool reloading = false;

    [SerializeField] private AudioSource reloadAudio;
    [SerializeField] private AudioSource shootAudio;

    [SerializeField] private TextMeshProUGUI bulletCount;
    [SerializeField] private GunOperate gunOperate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void SetFiringEnd()
    {
        firing = false;
    }
    private void PlayShootAudio()
    {
        shootAudio.Play();
    }

    private void SetReloadingEnd()
    {
        reloading = false;
    }
    private void PlayReloadAudio()
    {
        reloadAudio.Play();
    }

    private void ReloadBullets()
    {
        gunOperate.bulletsLeft = gunOperate.maxBulletsLeft;
        bulletCount.text = $"{gunOperate.bulletsLeft} / ∞";
    }
}
