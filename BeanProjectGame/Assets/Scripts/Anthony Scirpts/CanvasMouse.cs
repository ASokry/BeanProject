using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMouse : MonoBehaviour
{
    public static CanvasMouse Instance { get; private set; }

    public RectTransform canvas;

    /*public RectTransform test;
    private bool once = false;*/

    private void Awake() { Instance = this; }

    // Refactor code to work on canvas

    private void Update()
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = GetMouseCanvasPosition_Instance();
    }

    public static Vector2 GetMouseCanvasPosition() => Instance.GetMouseCanvasPosition_Instance();

    private Vector2 GetMouseCanvasPosition_Instance()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Input.mousePosition, null, out pos);
        //print(pos);

        /*if (!once && Input.GetMouseButtonDown(0))
        {
            print(Input.mousePosition);
            GameObject tes = Instantiate(test.gameObject, Input.mousePosition, Quaternion.identity) as GameObject;
            tes.GetComponent<Transform>().SetParent(canvas);
            Vector3 tespos = tes.GetComponent<RectTransform>().position;
            Vector2 testdumby;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, tespos, null, out testdumby);
            print(testdumby);
            once = true;
        }*/

        return pos;
    }

    public static RectTransform GetCanvasFromMouse() { return Instance.GetCanvasFromMouse_Instance(); }

    private RectTransform GetCanvasFromMouse_Instance()
    {
        return canvas;
    }
}
