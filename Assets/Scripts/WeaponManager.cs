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


    [Header("��ҩ Ammo")]
    public int totalRifleAmmo = 0;
    public int totalPistolAmmo = 0;

    [Header("��Ͷ���� Thorwable")]
    public int grenades = 0;
    public float throwForce = 10f;
    public GameObject grenadePrefab;
    public GameObject throwableSpawn;
    public float forceMultiplier = 0f;
    public float forceMultiplierLimit = 2f;//�����




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

        //����G����������Ͷ�����ӵ�ԽԶ
        if (Input.GetKey(KeyCode.G))
        {
            forceMultiplier += Time.deltaTime;

            //����������
            if (forceMultiplier > forceMultiplierLimit)
            {
                forceMultiplier = forceMultiplierLimit;
            }
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            if (grenades > 0)
            {
                ThrowLethal();
            }

            //����
            forceMultiplier = 0f;
        }

    }


    #region |--- ���� Weapon ---|
    public void PickupWeapon(GameObject pickedupWeapon)
    {
        //���������������(��������)
        AddWeaponIntoActiveSlot(pickedupWeapon);
    }
    #endregion

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

        //����box��ײ
        pickedupWeapon.GetComponent<BoxCollider>().enabled = false;

        //���߹���������е�����
        //pickedupWeapon.layer = LayerMask.NameToLayer(LAYER_PLAYERWEAPON);

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
            //weaponToDrop.layer = LayerMask.NameToLayer(LAYER_DEFAULT);
            

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

    #region |--- ��ҩ�� AmmoBox ---|
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
    #endregion

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

    #region |--- ��Ͷ���� Throwable ---|
    public void PickupThrowable(Throwable throwable)
    {
        switch (throwable.throwableType)
        {
            case Throwable.ThrowableType.Grenade:
                PickupThrowable();
                break;
        }
    }

    private void PickupThrowable()
    {
        grenades += 1;

        HUDManager.Instance.UpdateThrowable(Throwable.ThrowableType.Grenade);
    }

    private void ThrowLethal()
    {
        GameObject lethalPrefab = grenadePrefab;

        GameObject throwable = Instantiate(lethalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse );

        throwable.GetComponent<Throwable>().hasBeenThrown = true;

        //����UI
        grenades -= 1;
        HUDManager.Instance.UpdateThrowable(Throwable.ThrowableType.Grenade);

    }

    #endregion

}
