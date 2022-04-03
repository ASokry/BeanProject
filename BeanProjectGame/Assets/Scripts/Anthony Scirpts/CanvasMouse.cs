using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMouse : MonoBehaviour
{
    public static CanvasMouse Instance { get; private set; }

    public Canvas canvas;
    [SerializeField] private LayerMask mouseColliderLayerMask = new LayerMask();
    private Vector2 lastPos;

    private void Awake() { Instance = this; }

    // Refactor code to work on canvas

    /*private void Update()
    {
        //transform.position = Input.mousePosition;
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
        transform.position = canvas.transform.TransformPoint(pos);

        print(transform.position.x + " " + transform.position.y);
    }*/

    public static Vector2 GetMouseCanvasPosition() => Instance.GetMouseCanvasPosition_Instance();

    private Vector2 GetMouseCanvasPosition_Instance()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
        transform.position = canvas.transform.TransformPoint(pos);

        return pos;
    }
}
