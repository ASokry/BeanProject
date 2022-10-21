using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableObject : ItemObject
{
    public enum ConsumableType {Healing, Throwable, None};

    public ConsumableType consumableType;

    public int healAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [CustomEditor(typeof(ConsumableObject))]
    public class ConsumableObjectEditor : Editor
    {

    }

}
