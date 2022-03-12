using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMouse : MonoBehaviour
{
    public static CanvasMouse Instance { get; private set; }

    [SerializeField] private LayerMask mouseColliderLayerMask = new LayerMask();
    private Vector2 lastPos;

    private void Awake() { Instance = this; }

    // Refactor code to work on canvas
    
}
