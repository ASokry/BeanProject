using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridTile : MonoBehaviour
{
    [SerializeField] private Material[] highlightMaterials;
    [SerializeField] private Image image;
    private Material defualtMaterial;

    // Start is called before the first frame update
    void Start()
    {
        defualtMaterial = image.material;
    }

    public void SetEquipMat()
    {
        image.material = highlightMaterials[0];
    }

    public void SetDefualtMat()
    {
        image.material = defualtMaterial;
    }
}
