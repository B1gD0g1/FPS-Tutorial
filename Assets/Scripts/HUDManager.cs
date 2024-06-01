using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{

    public static HUDManager Instance { get; set; }


    [Header("弹药 Ammo")]
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    [Header("武器 Weapon")]
    public Image activeWeaponUI;
    public Image unActiveWeaponUI;

    [Header("可投掷物 Throwables")]
    public Image lethalUI;
    public TextMeshProUGUI lethalAmountUI;

    public Image tactialUI;
    public TextMeshProUGUI tactialAmountUI;

    public Sprite emptySlot;
    public Sprite greySlot;

    public GameObject middleDot;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon unActiveWeapon = GetUnActiveWeaponSlot().GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.BulletsLeft / activeWeapon.bulletsPerBurst}";
            totalAmmoUI.text = $"{WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.thisWeaponModel)}";

            Weapon.WeaponModel model = activeWeapon.thisWeaponModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);

            activeWeaponUI.sprite = GetWeaponSprite(model);

            if (unActiveWeapon)
            {
                unActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.thisWeaponModel);
            }

        }
        else
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";


            ammoTypeUI.sprite = emptySlot;
            
            activeWeaponUI.sprite = emptySlot;
            unActiveWeaponUI.sprite= emptySlot;

        }


        if (WeaponManager.Instance.lethalCount <= 0)
        {
            lethalUI.sprite = greySlot;
        }

        if (WeaponManager.Instance.tacticalCount <= 0)
        {
            tactialUI.sprite = greySlot;
        }

    }

    private Sprite GetWeaponSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.PistolM1911:
                return Resources.Load<GameObject>("PistolM1911_Weapon").GetComponent<SpriteRenderer>().sprite;

            case Weapon.WeaponModel.USP45:
                return Resources.Load<GameObject>("PistolM1911_Weapon").GetComponent<SpriteRenderer>().sprite;

            case Weapon.WeaponModel.M4:
                return Resources.Load<GameObject>("M4_Weapon").GetComponent<SpriteRenderer>().sprite;
            
            default:
                return null;
        }
    }

    private Sprite GetAmmoSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.PistolM1911:
                return Resources.Load<GameObject>("Pistol_Ammo").GetComponent<SpriteRenderer>().sprite;

            case Weapon.WeaponModel.USP45:
                return Resources.Load<GameObject>("Pistol_Ammo").GetComponent<SpriteRenderer>().sprite;

            case Weapon.WeaponModel.M4:
                return Resources.Load<GameObject>("Rifle_Ammo").GetComponent<SpriteRenderer>().sprite;

            default:
                return null;
        }
    }

    private GameObject GetUnActiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
        {
            if (weaponSlot != WeaponManager.Instance.activeWeaponSlot)
            {
                return weaponSlot;
            }
        }
        //这里是不执行的，但是需要返回一些东西
        return null;
    }

    internal void UpdateThrowablesUI()
    {
        lethalAmountUI.text = $"{WeaponManager.Instance.lethalCount}";
        tactialAmountUI.text = $"{WeaponManager.Instance.tacticalCount}";


        switch (WeaponManager.Instance.equippedLethalType)
        {
            case Throwable.ThrowableType.Grenade:
                lethalUI.sprite = Resources.Load<GameObject>("Grenade").GetComponent<SpriteRenderer>().sprite;
                break;

        }

        switch (WeaponManager.Instance.equippedTacticalType)
        {
            case Throwable.ThrowableType.Smoke_Grenade:
                tactialUI.sprite = Resources.Load<GameObject>("Smoke_Grenade").GetComponent<SpriteRenderer>().sprite;
                break;

        }

    }
}
