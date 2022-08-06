using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTarget : MonoBehaviour
{
    public CharacterMotion characterMotion;
    public bool debugMode;
    private Vector3 targetPosition;

    private MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (!debugMode)
        {
            meshRenderer.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(targetPosition != null)
        {
            transform.position = targetPosition;
        }

    }

    public void AssignTarget(Vector3 newTargetPosition)
    {
        targetPosition = newTargetPosition;
    }
}
