using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandle : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private string currentAnim = "Idle";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            //gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            if (currentAnim != "OpenDoor")
            {
                currentAnim = "OpenDoor";
                _animator.CrossFade("OpenDoor", 0f);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            //gameObject.layer = LayerMask.NameToLayer("Wall");
            if (currentAnim != "CloseDoor")
            {
                currentAnim = "CloseDoor";
                _animator.CrossFade("CloseDoor", 0f);
            }

        }
    }


}
