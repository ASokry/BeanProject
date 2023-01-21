using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPanel : MonoBehaviour
{
    public GameObject weaponPanel;
    public Text weaponName;
    public Text weaponDamage;
    public Text weaponAmmo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateWeaponPanel(bool enable)
    {
        if (enable)
        {
            weaponPanel.SetActive(true);
        }
        else
        {
            weaponPanel.SetActive(false);
        }
    }

    public void PopulateInformation(string weaponName, float weaponDamage)
    {
        this.weaponName.text = weaponName;
        this.weaponDamage.text = "Damage : " + weaponDamage.ToString();
    }

    public void UpdateAmmoCount(int curAmmo, int maxAmmo)
    {
        weaponAmmo.text = "Ammo : " + curAmmo.ToString() + " / " + maxAmmo.ToString();
    }
}
