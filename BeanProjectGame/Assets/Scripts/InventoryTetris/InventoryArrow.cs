using UnityEngine;
using UnityEngine.UI;

public class InventoryArrow : MonoBehaviour
{
    [SerializeField] private InventoryTetris inventoryTetris;
    [SerializeField] private Image image;
    private float currentValue;
    private float maxValue;

    private void Awake()
    {
        ResetFill();
        Hide();
    }

    public void Hide()
    {
        image.gameObject.SetActive(false);
    }

    public void Reveal()
    {
        image.gameObject.SetActive(true);
    }

    public void SetMax(float num)
    {
        maxValue = num;
    }

    public void ResetFill()
    {
        //print("reset");
        currentValue = 0;
        image.fillAmount = Normalize();
    }

    public void Fill()
    {
        currentValue++;

        if (currentValue > maxValue) { currentValue = maxValue; }
        image.fillAmount = Normalize();
    }

    private float Normalize()
    {
        //print(currentValue / maxValue);
        return currentValue / maxValue;
    }

    public void MoveArrow(int x, int y)
    {
        Vector3 position = inventoryTetris.GetGrid().GetWorldPosition(x, y);
        //print(position);
        GetComponent<RectTransform>().anchoredPosition = position;
    }
}
