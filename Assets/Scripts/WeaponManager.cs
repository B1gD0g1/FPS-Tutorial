using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class WeaponManager : MonoBehaviour
{

    public const string LAYER_PLAYERWEAPON = "PlayerWeapon";
    public const string LAYER_DEFAULT = "Default";



    public static WeaponManager Instance { get; set; }

    public List<GameObject> weaponSlots;


    public GameObject activeWeaponSlot;


    [Header("Ammo")]
    public int totalRifleAmmo = 0;
    public int totalPistolAmmo = 0;




    private void Start()
    {
        activeWeaponSlot = weaponSlots[0];
    }

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
        foreach (GameObject weaponSlot in weaponSlots)
        {
            if (weaponSlot == activeWeaponSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveSlot(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);
        }

    }

    public void PickupWeapon(GameObject pickedupWeapon)
    {
        //���������������(��������)
        AddWeaponIntoActiveSlot(pickedupWeapon);
    }

    private void AddWeaponIntoActiveSlot(GameObject pickedupWeapon)
    {

        DropCurrentWeapon(pickedupWeapon);

        pickedupWeapon.transform.SetParent(activeWeaponSlot.transform, false);

        Weapon weapon = pickedupWeapon.GetComponent<Weapon>();

        pickedupWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x,weapon.spawnPosition.y,weapon.spawnPosition.z);
        pickedupWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x,weapon.spawnRotation.y,weapon.spawnRotation.z);
        pickedupWeapon.transform.localScale = new Vector3(weapon.spawmScale.x,weapon.spawmScale.y,weapon.spawmScale.z);

        weapon.isActiveWeapon = true;
        weapon.animator.enabled = true;

        //���߹���������е�����
        pickedupWeapon.layer = LayerMask.NameToLayer(LAYER_PLAYERWEAPON);

    }

    private void DropCurrentWeapon(GameObject pickedupWeapon)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;
            Debug.Log(weaponToDrop.name);

            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;
            weaponToDrop.GetComponent<Weapon>().animator.enabled = false;

            weaponToDrop.transform.SetParent(pickedupWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedupWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedupWeapon.transform.localRotation;
            weaponToDrop.transform.localScale = (weaponToDrop.transform.localScale) / 2;//��֪��Ϊʲô���Զ��Ŵ����������Գ���2

            //�������������ָ����߹���
            weaponToDrop.layer = LayerMask.NameToLayer(LAYER_DEFAULT);
            

        }
    }

    //�л�����������
    public void SwitchActiveSlot(int slotNumber)
    {
        //���õ�һ��������
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }

        activeWeaponSlot = weaponSlots[slotNumber];

        //�����ڶ���������
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.isActiveWeapon = true;
        }

    }

    internal void PickupAmmo(AmmoBox ammo)
    {
        switch (ammo.ammoType)
        {
            case AmmoBox.AmmoType.PistolAmmo:
                totalPistolAmmo += ammo.ammoAmount;
                break;
            case AmmoBox.AmmoType.RifleAmmo:
                totalRifleAmmo += ammo.ammoAmount;
                break;
        }

    }

    internal void DecreaseTotalAmmo(int bulletsToDecrease, Weapon.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case Weapon.WeaponModel.PistolM1911:
                totalPistolAmmo -= bulletsToDecrease;
                break;
            case Weapon.WeaponModel.USP45:
                totalPistolAmmo -= bulletsToDecrease;
                break;
            case Weapon.WeaponModel.M4:
                totalRifleAmmo -= bulletsToDecrease;
                break;
        }
    }

    public int CheckAmmoLeftFor(Weapon.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case WeaponModel.PistolM1911:
                return totalPistolAmmo;
            case WeaponModel.USP45:
                return totalPistolAmmo;
            case WeaponModel.M4:
                return totalRifleAmmo;
            default:
                return 0;
        }
    }

}
