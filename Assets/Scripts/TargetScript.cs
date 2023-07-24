using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    [SerializeField] private Material redMat;
    [SerializeField] private Material greenMat;
    [SerializeField] private MeshRenderer meshRenderer;

    public bool shotAt = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ShotSequence()
    {
        print("mesh renderer[0] is " + meshRenderer.materials[0].color);
        meshRenderer.materials = new Material[] { greenMat, meshRenderer.materials[1] };
        print("mesh renderer[0] is now " + meshRenderer.materials[0].color);
        shotAt = true;
        yield return new WaitForSeconds(3);
        meshRenderer.materials = new Material[] { redMat, meshRenderer.materials[1] };
        shotAt = false;
    }
}
